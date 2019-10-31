using CutitBz.Models;
using Microsoft.EntityFrameworkCore;

namespace CutitBz.Data
{
    public class DataContext: DbContext
    {
        public DataContext(DbContextOptions<DataContext> options)
           : base(options)
        {
        }

        public DbSet<Link> Links { get; set; }
        public DbSet<View> Views { get; set; }
    }
}