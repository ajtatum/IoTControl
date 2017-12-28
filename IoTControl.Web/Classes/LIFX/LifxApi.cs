using RestSharp;

namespace IoTControl.Web.Classes.LIFX
{
    public class LifxApi
    {
        public static IRestResponse RunLifxClient(string url, Method requeMethod, string accessToken, string requestBody = null)
        {
            var client = new RestClient(url);
            var request = new RestRequest(requeMethod);

            request.AddHeader("Authorization", $"Bearer {accessToken}");
            request.AddHeader("Content-Type", "application/json");

            if (requestBody != null)
                request.AddParameter("favorite", requestBody, ParameterType.RequestBody);

            return client.Execute(request);
        }

        public const string BaseApiUrl = "https://api.lifx.com/v1";

        public class EndPoints
        {
            public static string ListLights(string selector) => $"{BaseApiUrl}/lights/{selector}";
            public static string SetState(string selector) => $"{BaseApiUrl}/lights/{selector}/state";
            public static string SetStates => $"{BaseApiUrl}/lights/states";
            public static string TogglePower(string selector) => $"{BaseApiUrl}/lights/{selector}/toggle";
            public static string BreatheEffect(string selector) => $"{BaseApiUrl}/lights/{selector}/effects/breathe";
            public static string PulseEffect(string selector) => $"{BaseApiUrl}/lights/{selector}/effects/pulse";
            public static string Cycle(string selector) => $"{BaseApiUrl}/lights/{selector}/cycle";
            public static string ListScenes => $"{BaseApiUrl}/scenes";
            public static string ActivateScene(string selector) => $"{BaseApiUrl}/scenes/{selector}/activate";
            public static string ValidateColor => $"{BaseApiUrl}/color";
        }
    }
}