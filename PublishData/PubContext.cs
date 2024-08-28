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

        public DbSet<Artist> Artists { get; set; }

        public DbSet<BookCover> BookCovers { get; set; }

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
            //Also can use .AsNoTracking on a specific query
            //optionsBuilder.UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //Seeding data, willbe translated in migration
            modelBuilder.Entity<Author>().HasData(new Author { AuthorId = 1, FirstName = "Dave", LastName = "Mac", Books = new List<Book>() });

            modelBuilder.Entity<Book>().HasData(new Book { BookId = 1, AuthorId = 1, Title = "Lewis Book", PublishDate = new DateOnly(1996, 4, 13) });

            modelBuilder.Entity<Book>().HasData(new Book { BookId = 2, AuthorId = 1, Title = "Lewis New Book", PublishDate = new DateOnly(1996, 4, 13) });

            var someArtists = new List<Artist>
            {
                new Artist
                {
                    ArtistId = 1,
                    FirstName = "Pablo",
                    LastName = "Foot"
                },
                 new Artist
                {
                    ArtistId = 2,
                    FirstName = "Lewis",
                    LastName = "Foot"
                },
            };

            modelBuilder.Entity<Artist>().HasData(someArtists);

            var someBookCovers = new List<BookCover>
            {
                new() {
                    BookCoverId = 1,
                    DesignIdeas = "Pablo",
                    DigitalOnly = true,
                    BookId = 1
                },
                new() {
                    BookCoverId = 2,
                    DesignIdeas = "Lewis",
                    DigitalOnly = false, BookId = 2
                },
            };

            modelBuilder.Entity<BookCover>().HasData(someBookCovers);
        }

    }
}
