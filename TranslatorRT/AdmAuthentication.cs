using System;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Net;
using System.Threading.Tasks;
using Windows.Foundation;
using System.Net.Http;

namespace MicrosoftTranslatorRT
{
    public sealed class AdmAuthentication
    {
        internal static readonly string DatamarketAccessUri = "https://datamarket.accesscontrol.windows.net/v2/OAuth2-13";
        private string clientId;
        private string clientSecret;
        private string request;
        private AdmAccessToken token;       

        public IAsyncOperation<AdmAuthentication> Initializer(string clientId, string clientSecret)
        {
            return Contractor(clientId, clientSecret).AsAsyncOperation();
        }

        private async Task<AdmAuthentication> Contractor(string clientId, string clientSecret)
        {
            AdmAuthentication obj = new AdmAuthentication();
            obj.clientId = clientId;
            obj.clientSecret = clientSecret;
            //If clientid or client secret has special characters, encode before sending request
            obj.request = string.Format("grant_type=client_credentials&client_id={0}&client_secret={1}&scope=http://api.microsofttranslator.com", WebUtility.UrlEncode(clientId), WebUtility.UrlEncode(clientSecret));
            obj.token = await HttpPost(DatamarketAccessUri, request);
            return obj;         
        }

        public AdmAccessToken GetAccessToken()
        {
            return token;
        }

        private async Task<AdmAccessToken> HttpPost(string DatamarketAccessUri, string requestDetails)
        {
            //Prepare OAuth request             
            string requestBody = "grant_type=client_credentials&client_id=MohamedAhmed&client_secret=bCGqd7PXvF3QcgSoL5QHRMuJ137xQsft1aRe%2bfs3Sh8&scope=http://api.microsofttranslator.com";
            byte[] bytes = Encoding.UTF8.GetBytes(requestBody);

            HttpResponseMessage response = null;
            try
            {
                HttpClient request = new HttpClient();
                request.DefaultRequestHeaders.Add("contentType", "application/x-www-form-urlencoded");
                response = await request.PostAsync(DatamarketAccessUri, new ByteArrayContent(bytes));
            }
            catch (Exception e)
            {

                throw;
            }

            DataContractJsonSerializer serializer = new DataContractJsonSerializer(typeof(AdmAccessToken));
            //Get deserialized object from JSON stream            
            token = (AdmAccessToken)serializer.ReadObject(await response.Content.ReadAsStreamAsync());
            return token;
        }
    }
}
