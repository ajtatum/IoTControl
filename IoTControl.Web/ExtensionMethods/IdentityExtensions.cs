using System.Linq;
using System.Security.Claims;
using System.Security.Principal;
using System.Web;
using Microsoft.Owin.Security;

namespace IoTControl.Web.ExtensionMethods
{
    public static class IdentityExtensions
    {
        //public static void AddUpdateClaim(this ClaimsIdentity identity, string key, string value)
        //{
        //    // check for existing claim and remove it
        //    var existingClaim = identity.FindFirst(key);
        //    if (existingClaim != null)
        //        identity.RemoveClaim(existingClaim);

        //    // add new claim
        //    identity.AddClaim(new Claim(key, value));
        //    var authenticationManager = HttpContext.Current.GetOwinContext().Authentication;
        //    authenticationManager.AuthenticationResponseGrant = new AuthenticationResponseGrant(new ClaimsPrincipal(identity), new AuthenticationProperties() { IsPersistent = true });
        //}

        //public static string GetClaimValue(ClaimsIdentity identity, string key)
        //{
        //    var claim = identity.Claims.FirstOrDefault(c => c.Type == key);
        //    return claim?.Value ?? string.Empty;
        //}

        public static void AddUpdateClaim(this IPrincipal currentPrincipal, string key, string value)
        {
            if (!(currentPrincipal.Identity is ClaimsIdentity identity))
                return;

            // check for existing claim and remove it
            var existingClaim = identity.FindFirst(key);
            if (existingClaim != null)
                identity.RemoveClaim(existingClaim);

            // add new claim
            identity.AddClaim(new Claim(key, value));
            var authenticationManager = HttpContext.Current.GetOwinContext().Authentication;
            authenticationManager.AuthenticationResponseGrant = new AuthenticationResponseGrant(new ClaimsPrincipal(identity), new AuthenticationProperties() { IsPersistent = true });
        }

        public static string GetClaimValue(this IPrincipal currentPrincipal, string key)
        {
            if (!(currentPrincipal.Identity is ClaimsIdentity identity))
                return null;

            var claim = identity.Claims.FirstOrDefault(c => c.Type == key);
            return claim?.Value ?? string.Empty;
        }
    }
}