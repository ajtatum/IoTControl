using System;
using System.Collections.Generic;
using System.Data.Entity.Migrations;
using System.Dynamic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using IoTControl.Common.DAL;
using IoTControl.Models;
using IoTControl.Web.Classes.Lighting;
using IoTControl.Web.Classes.LIFX;
using IoTControl.Web.Services;
using IoTControl.Web.ViewModels;
using Microsoft.AspNet.Identity;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using RestSharp;

namespace IoTControl.Web.Controllers
{
    [Authorize]
    public class LifxController : Controller
    {
        private readonly IoTControlDbContext dbContext;
        private readonly LookupService lookupService;
        private string CurrentUserId => System.Web.HttpContext.Current.User.Identity.GetUserId();

        private const string LifxApiUrl = "https://api.lifx.com/v1/lights/{0}:{1}/state";

        public LifxController(IoTControlDbContext dbContext)
        {
            this.dbContext = dbContext;
            this.lookupService = new LookupService(dbContext);
        }

        // GET: Lifx
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult AccessToken()
        {
            var vm = dbContext.UserLifxAccessTokens
                         .FirstOrDefault(x => x.UserId == CurrentUserId)
                     ?? new UserLifxAccessToken()
                     {
                         UserId = CurrentUserId
                     };

            return View(vm);
        }

        [HttpPost]
        public ActionResult AccessToken(UserLifxAccessToken vm)
        {
            var isUpdate = vm.Id > 0;

            var lifxAccessToken = new UserLifxAccessToken();

            if (isUpdate)
            {
                lifxAccessToken = dbContext.UserLifxAccessTokens.FirstOrDefault(x => x.Id == vm.Id);
                if (lifxAccessToken != null)
                    lifxAccessToken.Id = vm.Id;
            }

            lifxAccessToken.UserId = vm.UserId;
            lifxAccessToken.AccessToken = vm.AccessToken;

            dbContext.UserLifxAccessTokens.AddOrUpdate(lifxAccessToken);
            dbContext.SaveChanges();

            return View(vm);
        }

        public ActionResult Favorites()
        {
            var vm = dbContext.UserLifxFavorites
                .Where(x => x.UserId == CurrentUserId)
                .ToList();

            return View(vm);
        }

        public ActionResult Add()
        {
            var userFavorite = new UserLifxFavorite
            {
                UserId = CurrentUserId
            };

            var vm = AutoMapper.Mapper.Map<UserLifxFavorite, LifxViewModel.FavoriteEditor>(userFavorite);
            vm.SelectorTypeList = lookupService.GetSelectListItems("Select Selector", "LifxSelector");
            vm.PowerOptionsList = lookupService.GetSelectListItems("Select Power", "LifxPowerOption");
            vm.KelvinList = dbContext.Kelvins.OrderBy(x => x.Id).ToList();

            return View(vm);
        }

        public ActionResult Edit(int id)
        {
            var userFavorite = dbContext.UserLifxFavorites.FirstOrDefault(x => x.Id == id && x.UserId == CurrentUserId);
            if (userFavorite == null)
                return RedirectToAction("Favorites", "Lifx");

            var vm = AutoMapper.Mapper.Map<UserLifxFavorite, LifxViewModel.FavoriteEditor>(userFavorite);
            vm.SelectorTypeList = lookupService.GetSelectListItems("Select Selector", "LifxSelector");
            vm.PowerOptionsList = lookupService.GetSelectListItems("Select Power", "LifxPowerOption");
            vm.KelvinList = dbContext.Kelvins.OrderBy(x => x.Id).ToList();

            var lifxFavoriteJson = JsonConvert.DeserializeObject<LifxViewModel.LifxFavoriteJson>(userFavorite.JsonValue);
            vm.ColorPicker = $"hsv({lifxFavoriteJson.Hue}, {lifxFavoriteJson.Saturation}, {lifxFavoriteJson.Brightness})";

            vm.LifxFavoriteJson = new LifxViewModel.LifxFavoriteJson
            {
                Power = lifxFavoriteJson.Power,
                Duration = lifxFavoriteJson.Duration
            };

            return View(vm);
        }

        [HttpPost]
        public ActionResult Add(LifxViewModel.FavoriteEditor vm)
        {
            var favorite = new UserLifxFavorite
            {
                Name = vm.Name,
                SelectorType = vm.SelectorType,
                SelectorValue = vm.SelectorValue,
                PickRandom = vm.PickRandom,
                JsonValue = vm.JsonValue,
                CreatedOn = DateTime.Now,
                UpdatedOn = DateTime.Now,
                UserId = CurrentUserId
            };

            dbContext.UserLifxFavorites.AddOrUpdate(favorite);
            dbContext.SaveChanges();

            return RedirectToAction("Favorites");
        }

        [HttpPost]
        public ActionResult Edit(LifxViewModel.FavoriteEditor vm)
        {
            var favorite = dbContext.UserLifxFavorites.FirstOrDefault(x => x.Id == vm.Id);

            if (favorite != null)
            {
                favorite.Name = vm.Name;
                favorite.SelectorType = vm.SelectorType;
                favorite.SelectorValue = vm.SelectorValue;
                favorite.PickRandom = vm.PickRandom;
                favorite.JsonValue = vm.JsonValue;
                favorite.UpdatedOn = DateTime.Now;

                dbContext.UserLifxFavorites.AddOrUpdate(favorite);
                dbContext.SaveChanges();
            }

            return RedirectToAction("Favorites");
        }

        public ActionResult Run(int id)
        {
            var favorite = dbContext.UserLifxFavorites
                .FirstOrDefault(x => x.Id == id && x.UserId == CurrentUserId);

            if (favorite != null)
            {
                var result = RunLifxFavorite(favorite);
                if (result != null)
                {
                    var resultData = result;
                }
            }

            return RedirectToAction("Favorites");
        }

        [HttpPost]
        public JsonResult GetCurrentColor(LifxViewModel.FavoriteEditor vm)
        {
            //var favorite = dbContext.UserLifxFavorites.FirstOrDefault(x => x.Id == vm.Id);
            var lifxLight = GetLifxLights(vm.SelectorType, vm.SelectorValue).FirstOrDefault();

            if (lifxLight != null)
            {
                dynamic currentState = new ExpandoObject();
                currentState.power = lifxLight.Power;
                currentState.hue = Convert.ToInt32(lifxLight.Color.Hue.GetValueOrDefault(0));
                currentState.saturation = Math.Round(lifxLight.Color.Saturation.GetValueOrDefault(0), 2);
                currentState.brightness = Math.Round(lifxLight.Brightness, 2);
                currentState.kelvin = lifxLight.Color.Kelvin;
                currentState.duration = !string.IsNullOrEmpty(vm.JsonValue) 
                    ? (JObject.Parse(vm.JsonValue).Value<int?>("duration") ?? 10) 
                    : 10;

                return Json(JsonConvert.SerializeObject(currentState, Formatting.Indented), JsonRequestBehavior.AllowGet);
            }

            return null;
        }

        private IRestResponse RunLifxFavorite(UserLifxFavorite favorite)
        {
            var lifxAccessToken = dbContext.UserLifxAccessTokens.FirstOrDefault(x => x.UserId == CurrentUserId)?.AccessToken;

            if (lifxAccessToken != null)
            {
                var selector = $"{favorite.SelectorType}:{favorite.SelectorValue}";
                var apiUrl = LifxApi.EndPoints.SetState(selector);

                var lifxFavoriteJson = JsonConvert.DeserializeObject<LifxViewModel.LifxFavoriteJson>(favorite.JsonValue);
                var lifxColor = AutoMapper.Mapper.Map<LifxColor>(lifxFavoriteJson);

                dynamic lifxJson = DeconstructFavoriteJson(favorite.JsonValue);

                return LifxApi.RunLifxClient(apiUrl, Method.PUT, lifxAccessToken, JsonConvert.SerializeObject(lifxJson));

                //LifxClient client = new LifxClient(lifxAccessToken.AccessToken);
                //var response = await client.SetColor(new Selector.GroupLabel("Bedroom"), new LifxColor.HSBK(190, 0.67F, 0.25F, 3500), 5, PowerState.On);
                //return response;
            }

            return null;
        }

        private List<LifxLight> GetLifxLights(string selectorType, string selectorValue)
        {
            var lifxAccessToken = dbContext.UserLifxAccessTokens.FirstOrDefault(x => x.UserId == CurrentUserId)?.AccessToken;

            if (lifxAccessToken != null)
            {
                var selector = $"{selectorType}:{selectorValue}";
                var apiUrl = LifxApi.EndPoints.ListLights(selector);

                return JsonConvert.DeserializeObject<List<LifxLight>>(LifxApi.RunLifxClient(apiUrl, Method.GET, lifxAccessToken, null).Content);
            }

            return null;
        }

        [HttpPost]
        public JsonResult GetLifxColorJson(LifxViewModel.LifxFavoriteJson model)
        {
            //var color = AutoMapper.Mapper.Map<LifxColor>(hsvColor);

            //var lifxJson = AutoMapper.Mapper.Map<LifxViewModel.LifxFavoriteJson>(hsvColor);
            //lifxJson.Power = "on";
            //lifxJson.Duration = 5;
            if (string.IsNullOrEmpty(model.Power))
                model.Power = "on";

            model.Saturation = Math.Round(model.Saturation.GetValueOrDefault(0), 2);
            model.Brightness = Math.Round(model.Brightness.GetValueOrDefault(0), 2);


            return Json(JsonConvert.SerializeObject(model, Formatting.Indented), JsonRequestBehavior.AllowGet);
        }

        private dynamic DeconstructFavoriteJson(string favoriteJson)
        {
            var lifxFavoriteJson = JsonConvert.DeserializeObject<LifxViewModel.LifxFavoriteJson>(favoriteJson);
            var lifxColor = AutoMapper.Mapper.Map<LifxColor>(lifxFavoriteJson);

            dynamic lifxJson = new ExpandoObject();
            lifxJson.color = lifxColor.ToString();
            lifxJson.duration = lifxFavoriteJson.Duration;
            lifxJson.power = lifxFavoriteJson.Power;

            return lifxJson;
        }
    }
}