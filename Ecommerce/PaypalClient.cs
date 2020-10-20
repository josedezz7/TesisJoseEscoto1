using PayPalCheckoutSdk.Core;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Web;

namespace Ecommerce
{
    public class PaypalClient
    {
        public static PayPalEnvironment environment()
        {
            return new SandboxEnvironment("AX9ZkhBD3eJQL8wWY85K0Z35nx0iPH7w4hYk9psYXKiKfxcrlv7BvqvBpGssGAoQ4dw_K67M0D3b0b0X", "EGZvB5Q8B8gAEZcQLeqhnLjoXqOQc2ZVCYhXqma0eWAcIPgUDlWdJp7sPCoefYvmaThVHGEYBZqIYyhe");
        }

        /**
            Returns PayPalHttpClient instance to invoke PayPal APIs.
         */
        public static PayPalHttpClient client()
        {
            return  new PayPalHttpClient(environment());
        }

        public static PayPalHttpClient client(string refreshToken)
        {
            return new PayPalHttpClient(environment(), refreshToken);
        }

        /**
            Use this method to serialize Object to a JSON string.
        */
        public static String ObjectToJSONString(Object serializableObject)
        {
            MemoryStream memoryStream = new MemoryStream();
            var writer = JsonReaderWriterFactory.CreateJsonWriter(
                        memoryStream, Encoding.UTF8, true, true, "  ");
            DataContractJsonSerializer ser = new DataContractJsonSerializer(serializableObject.GetType(), new DataContractJsonSerializerSettings { UseSimpleDictionaryFormat = true });
            ser.WriteObject(writer, serializableObject);
            memoryStream.Position = 0;
            StreamReader sr = new StreamReader(memoryStream);
            return sr.ReadToEnd();
        }
    }
}