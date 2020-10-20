
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Ecommerce
{
    public class PaypalConfiguration
    {
        public readonly static string clientId;
        public readonly static string clientSecret;

        static PaypalConfiguration()
        {
            var config = getconfig();
            clientId = "AX9ZkhBD3eJQL8wWY85K0Z35nx0iPH7w4hYk9psYXKiKfxcrlv7BvqvBpGssGAoQ4dw_K67M0D3b0b0X";
            clientSecret = "EGZvB5Q8B8gAEZcQLeqhnLjoXqOQc2ZVCYhXqma0eWAcIPgUDlWdJp7sPCoefYvmaThVHGEYBZqIYyhe";
        }

        /*private static Dictionary<string, string> getconfig()
        {
            return PayPal.Api.ConfigManager.Instance.GetProperties();
        }*/

        /*private static string GetAccessToken()
        {
            string accessToken = new OAuthTokenCredential(clientId, clientSecret, getconfig()).GetAccessToken();
            return accessToken;
        }*/

        public static APIContext GetAPIContext()
        {
            APIContext apicontext = new APIContext(GetAccessToken());
            apicontext.Config = getconfig();
            return apicontext;
        }


    }
}