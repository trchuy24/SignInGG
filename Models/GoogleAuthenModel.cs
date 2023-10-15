using System.ComponentModel.DataAnnotations;

namespace Sign_inWithGGAcc.Models
{
    public class GoogleAuthenModel
    {
        public class DataFromGetTokenModel
        {
            public string access_token { get; set; }
            public int expires_in { get; set; }
            public string scope { get; set; }
            public string token_type { get; set; }
            public string id_token { get; set; }
        }

        public class GgUserFromAPI
        {
            public string id { get; set; }
            public string name { get; set; }
            public string given_name { get; set; }
            public string family_name { get; set; }
            public string picture { get; set; }
            public string locale { get; set; }
        }

        public class GgUserInDB
        {
            [Key]
            public string google_id { get; set; } 
            public string name { get; set; }
            public string given_name { get; set; }
            public string family_name { get; set; }
            public string picture { get; set; }
            public string locale { get; set; }
        }
    }
}
