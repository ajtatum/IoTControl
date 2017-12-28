using System;
using System.Collections.Generic;
using System.Data.Entity.Migrations;
using System.Dynamic;
using System.Linq;
using System.Web.Mvc;
using IoTControl.Common.DAL;
using IoTControl.Models;
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

            return View(vm);
        }

        public ActionResult Edit(int id)
        {
            var userFavorite = dbContext.UserLifxFavorites.FirstOrDefault(x => x.Id == id && x.UserId == CurrentUserId);

            var vm = AutoMapper.Mapper.Map<UserLifxFavorite, LifxViewModel.FavoriteEditor>(userFavorite);
            vm.SelectorTypeList = lookupService.GetSelectListItems("Select Selector", "LifxSelector");

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
                currentState.color = $"hue:{Math.Round(lifxLight.Color.Hue, 0)} saturation:{Math.Round(lifxLight.Color.Saturation, 2)} kelvin:{lifxLight.Color.Kelvin}";
                currentState.brightness = Math.Round(lifxLight.Brightness, 2);
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

                return LifxApi.RunLifxClient(apiUrl, Method.PUT, lifxAccessToken, favorite.JsonValue);

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
    }
}