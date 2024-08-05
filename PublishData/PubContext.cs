using Microsoft.EntityFrameworkCore;
using PublisherDomain;

namespace PublishData
{
    public class PubContext : DbContext
    {

        public DbSet<Author> Authors { get; set; }

        public DbSet<Book> Books { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer("Data Source = (localdb)\\MSSQLLocalDB; Initial Catalog = PubDatabase");


            //Tracking ofDB objects in local mem can be displayed for performance
            //Also canuse .AsNoTracking on a specific query
            //optionsBuilder.UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);

        }

    }
}
