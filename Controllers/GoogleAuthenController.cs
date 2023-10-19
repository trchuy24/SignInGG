using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json.Serialization;
using System.Text;
using System.Net.WebSockets;
using Newtonsoft.Json;
using System.Reflection.Metadata;
using Sign_inWithGGAcc.Models;

namespace Sign_inWithGGAcc.Controllers
{
    [Route("google/callback")]
    [ApiController]
    public class GoogleAuthenController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        public GoogleAuthenController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task<string> GetUserProfile(string code)
        {
            //GET data From appsettings.json
            var clientId = _configuration.GetValue("GoogleSettings:ClientId", "");
            var clientSecret = _configuration.GetValue("GoogleSettings:ClientSecret", "");
            var redirect_uri = _configuration.GetValue("GoogleSettings:redirect_uri","");
            var grant_type = _configuration.GetValue("GoogleSettings:grant_type", "");

            //Get Token  
            var dataForGettoken = new[]
            {
                new KeyValuePair<string, string>("code", code),
                new KeyValuePair<string, string>("client_id", clientId),
                new KeyValuePair<string, string>("client_secret", clientSecret ),
                new KeyValuePair<string, string>("redirect_uri", redirect_uri),
                new KeyValuePair<string, string>("grant_type", grant_type)
            };
            var bodyRequest = new FormUrlEncodedContent(dataForGettoken);
            var httpClient = new HttpClient();
            HttpResponseMessage response;
            response = await httpClient.PostAsync("https://oauth2.googleapis.com/token",bodyRequest);
            string tokenString = await response.Content.ReadAsStringAsync();

            //get User data
            GoogleAuthenModel.DataFromGetTokenModel dataFromGetToken = JsonConvert.DeserializeObject<GoogleAuthenModel.DataFromGetTokenModel>(tokenString);
            var dataForGetUser = new[]
            {
                new KeyValuePair<string, string>("access_token", dataFromGetToken.access_token),
            };
            
            HttpResponseMessage response1 = await httpClient.GetAsync("https://www.googleapis.com/oauth2/v1/userinfo?access_token="+dataFromGetToken.access_token);
            string userString = await response1.Content.ReadAsStringAsync();

            //add data to GgUser
            GoogleAuthenModel.GgUserFromAPI ggUser = JsonConvert.DeserializeObject<GoogleAuthenModel.GgUserFromAPI>(userString);

            //add User to data Base
            var context = new DBContext();
            var ggUsers = context.GoogleUser.ToList();
            int trung = 0;
            for (int i = 0; i < ggUsers.Count; i++)
            {
                if (ggUser.id == ggUsers[i].google_id)
                {
                    trung = 1;
                    return "trung";
                    break;
                }
            }
            if (trung == 0)
            {
                context.Database.EnsureCreated();
                context.GoogleUser.Add(new GoogleAuthenModel.GgUserInDB
                {
                    google_id = ggUser.id,
                    name = ggUser.name,
                    given_name = ggUser.given_name,
                    family_name = ggUser.family_name,
                    picture = ggUser.picture,
                    locale = ggUser.locale,
                });
                context.SaveChanges();
                return "OK";
            }
            trung= 0;
            return userString;
        }
    }
}
