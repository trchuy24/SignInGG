using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using System.ComponentModel.DataAnnotations;
using System.Security.Policy;

namespace Sign_inWithGGAcc.Models
{
    public class InternalUserModel
    {

        public class InternalUser
        {
            public string FamilyName { get; set; }
            public string GivenName { get; set; }
            public string Password { get; set; }
            public string Email { get; set; }
        }

        public class InternalUsertDB
        {
            [Key]
            public int InternalID { get; set; }
            public string FamilyName { get; set; }
            public string GivenName { get; set; }
            public string HashCode { get; set; }
            public string Email { get; set; }
        }

        static public string HashCode(string pass)
        {
            byte[] salt = new byte[128 / 8];
            string hashed = Convert.ToBase64String(KeyDerivation.Pbkdf2(
            password: pass,
            salt: salt,
            prf: KeyDerivationPrf.HMACSHA256,
            iterationCount: 100000,
            numBytesRequested: 256 / 8));
            return hashed;
        }

    }
}
