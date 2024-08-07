using Microsoft.EntityFrameworkCore;
using PublisherDomain;

namespace PublishData
{
    //DbContext represents a session with the database.
    //This session is started when you do something with the database
    //DbContext has change tracker, manages collection of entity entry objects
    //Entity entry objects state info for each entity 
    //Tracking entity is created when EF begins to track, e.g.used in query
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
