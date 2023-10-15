using Microsoft.EntityFrameworkCore;
using MySql.EntityFrameworkCore;
using Sign_inWithGGAcc.Models;

namespace Sign_inWithGGAcc.Models
{
    public class DBContext : DbContext
    {
        public DbSet<GoogleAuthenModel.GgUserInDB> GoogleUser { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseMySQL("server=localhost;database=LoginProject;user=root;password=123456");
        }
    }
}


