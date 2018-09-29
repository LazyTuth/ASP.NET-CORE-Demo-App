using Demo.API.Models;
using Microsoft.EntityFrameworkCore;

namespace Demo.API.Data
{
    public class DemoContext : DbContext
    {
        public DemoContext(DbContextOptions<DemoContext> options) : base(options)
        { }

        public DbSet<User> User {get;set;}
        
    }
}