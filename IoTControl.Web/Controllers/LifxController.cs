using System;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Web.Mvc;
using IoTControl.Common.DAL;
using IoTControl.Models;
using IoTControl.Web.Models;
using Microsoft.AspNet.Identity;
using Newtonsoft.Json;
using RestSharp;

namespace IoTControl.Web.Controllers
{
    [Authorize]
    public class LifxController : Controller
    {
        private readonly IoTControlDbContext ioTControlDbContext;
        private string CurrentUserId => System.Web.HttpContext.Current.User.Identity.GetUserId();

        private const string LifxApiUrl = "https://api.lifx.com/v1/lights/{0}/state";

        public LifxController(IoTControlDbContext ioTControlDbContext)
        {
            this.ioTControlDbContext = ioTControlDbContext;
        }

        // GET: Lifx
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult AccessToken()
        {
            var vm = ioTControlDbContext.UserLifxAccessTokens
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
            var isUpdate = vm.UserLifxAccessTokenId > 0;

            var lifxAccessToken = new UserLifxAccessToken();

            if (isUpdate)
            {
                lifxAccessToken = ioTControlDbContext.UserLifxAccessTokens.FirstOrDefault(x => x.UserLifxAccessTokenId == vm.UserLifxAccessTokenId);
                if (lifxAccessToken != null)
                    lifxAccessToken.UserLifxAccessTokenId = vm.UserLifxAccessTokenId;
            }

            lifxAccessToken.UserId = vm.UserId;
            lifxAccessToken.AccessToken = vm.AccessToken;

            ioTControlDbContext.UserLifxAccessTokens.AddOrUpdate(lifxAccessToken);
            ioTControlDbContext.SaveChanges();

            return View(vm);
        }

        public ActionResult Favorites()
        {
            var vm = ioTControlDbContext.UserLifxFavorites
                .Where(x => x.UserId == CurrentUserId)
                .ToList();

            return View(vm);
        }

        public ActionResult Add()
        {
            var vm = new UserLifxFavorite();
            vm.UserId = CurrentUserId;

            return View(vm);
        }

        public ActionResult Edit(int id)
        {
            var vm = ioTControlDbContext.UserLifxFavorites
                .FirstOrDefault(x => x.UserLifxFavoriteId == id && x.UserId == CurrentUserId);

            return View(vm);
        }

        [HttpPost]
        public ActionResult Add(UserLifxFavorite vm)
        {
            var favorite = new UserLifxFavorite
            {
                Name = vm.Name,
                Selector = vm.Selector,
                JsonValue = vm.JsonValue,
                CreatedOn = DateTime.Now,
                UpdatedOn = DateTime.Now,
                UserId = CurrentUserId
            };

            ioTControlDbContext.UserLifxFavorites.AddOrUpdate(favorite);
            ioTControlDbContext.SaveChanges();

            return RedirectToAction("Favorites");
        }

        [HttpPost]
        public ActionResult Edit(UserLifxFavorite vm)
        {
            var favorite = ioTControlDbContext.UserLifxFavorites.FirstOrDefault(x => x.UserLifxFavoriteId == vm.UserLifxFavoriteId);

            if (favorite != null)
            {
                favorite.Name = vm.Name;
                favorite.Selector = vm.Selector;
                favorite.JsonValue = vm.JsonValue;
                favorite.UpdatedOn = DateTime.Now;

                ioTControlDbContext.UserLifxFavorites.AddOrUpdate(favorite);
                ioTControlDbContext.SaveChanges();
            }

            return RedirectToAction("Favorites");
        }

        public ActionResult Run(int id)
        {
            var favorite = ioTControlDbContext.UserLifxFavorites
                .FirstOrDefault(x => x.UserLifxFavoriteId == id && x.UserId == CurrentUserId);

            if (favorite != null)
            {
                var result = LifxRestClient(favorite);
                if (result != null)
                {
                    var resultData = JsonConvert.DeserializeObject<LifxStateResponse>(result.Content);
                }
            }

            return RedirectToAction("Favorites");
        }

        private IRestResponse LifxRestClient(UserLifxFavorite favorite)
        {
            var lifxAccessToken = ioTControlDbContext.UserLifxAccessTokens.FirstOrDefault(x => x.UserId == CurrentUserId);

            if (lifxAccessToken != null)
            {
                var client = new RestClient(string.Format(LifxApiUrl, favorite.Selector));
                var request = new RestRequest(Method.PUT);

                request.AddHeader("Authorization", $"Bearer {lifxAccessToken.AccessToken}");
                request.AddHeader("Content-Type", "application/json");

                request.AddParameter("favorite", favorite.JsonValue, ParameterType.RequestBody);

                return client.Execute(request);
            }

            return null;
        }
    }
}