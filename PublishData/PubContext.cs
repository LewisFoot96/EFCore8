using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
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
            optionsBuilder.UseSqlServer("Data Source = (localdb)\\MSSQLLocalDB; Initial Catalog = PubDatabase")
                .LogTo(Console.WriteLine //can about to different things 
                , new[]
                {
                    DbLoggerCategory.Database.Command.Name,

                }, LogLevel.Information) //Logging to console, there are catogories of logs. Can filter on these as above. 
                .EnableSensitiveDataLogging(); //Can see incoming data, not recommened for produciton. 


            //Tracking ofDB objects in local mem can be displayed for performance
            //Also canuse .AsNoTracking on a specific query
            //optionsBuilder.UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //Seeding data, willbe translated in migration
            modelBuilder.Entity<Author>().HasData(new Author { AuthorId = 1, FirstName = "Dave", LastName = "Mac", Books = new List<Book>() });

            modelBuilder.Entity<Book>().HasData(new Book { BookId = 1, AuthorId = 1, Title = "Lewis Book", PublishDate = new DateOnly(1996, 4, 13) });
        }

    }
}
