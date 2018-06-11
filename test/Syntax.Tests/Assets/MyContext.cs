using EFCore.Sample.Models;
using Microsoft.EntityFrameworkCore;

namespace EFCore.Sample
{
    public class MyContext : DbContext
    {
        public DbSet<Food> Foods
        {
            get;
            set;
        }
    }
}