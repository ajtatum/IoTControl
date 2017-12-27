using System;
using System.Collections.Generic;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using IoTControl.Common.DAL;
using IoTControl.Models;
using Microsoft.AspNet.Identity;

namespace IoTControl.Web.Controllers
{
    [Authorize]
    public class LifxController : Controller
    {
        private readonly IoTControlDbContext ioTControlDbContext;
        private string CurrentUserId => System.Web.HttpContext.Current.User.Identity.GetUserId();

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
    }
}