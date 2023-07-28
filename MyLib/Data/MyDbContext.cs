using Microsoft.EntityFrameworkCore;
using MyLib.Models;

namespace MyLib.Data
{
    public class MyDbContext : DbContext
    {
        public MyDbContext(DbContextOptions<MyDbContext> options) : base(options)
        {
        }
        public DbSet<UserData> userDatas { get; set; }
        public DbSet<AuthData> authDatas { get; set; }
    }
}
