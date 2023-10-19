using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using Microsoft.AspNetCore.Mvc;
using Sign_inWithGGAcc.Models;

namespace Sign_inWithGGAcc.Controllers
{
    public class Register : Controller
    {

        [Route ("/Register")]
        public IActionResult Index(InternalUserModel.InternalUser user)
        {
            return View();
        }

        public IActionResult AddUser(InternalUserModel.InternalUser user)
        {
            var context = new DBContext();
            var internalUsers = context.InternalUser.ToList();
            int idMax = 0;
            if( internalUsers.Count > 0) 
            {
                for (int i = 0; i < internalUsers.Count; i++)
                {
                    if (internalUsers[i].InternalID > idMax)
                    {
                        idMax = internalUsers[i].InternalID;
                    }
                }
            }
            

            string hashCode = InternalUserModel.HashCode(user.Password);

            context.Database.EnsureCreated();
            context.InternalUser.Add(new InternalUserModel.InternalUsertDB
            {
                InternalID = idMax+1,
                FamilyName = user.FamilyName,
                GivenName = user.GivenName,
                Email = user.Email,
                HashCode = hashCode,
            });
            context.SaveChanges();
            return View();
        }

    }
}
