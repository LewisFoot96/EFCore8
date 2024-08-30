using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using PubApi;
using PublishData;
using PublisherDomain;

namespace PubTests
{
    [TestClass]
    public class InMemoryTests
    {
        [TestMethod]
        public void CanInsertAuthroIntoDatabase()
        {
            var builder = new DbContextOptionsBuilder<PubContext>();

            var _connection = new SqliteConnection("Filename=:memory:"); //In memeory sql lite
            _connection.Open();
            builder.UseSqlite(_connection);

            using (var context = SetUpSQLiteMemoryContextWithOpenConnection())
            {
                //context.Database.EnsureDeleted();
                context.Database.EnsureCreated();
                var author = new Author { FirstName = "a", LastName = "b" };
                context.Authors.Add(author);
                //Debug.WriteLine($"Before save: {author.AuthorId}");
                context.SaveChanges();
                //Debug.WriteLine($"After save: {author.AuthorId}");

                Assert.AreNotEqual(0, author.AuthorId);
            }
        }

        [TestMethod]
        public void ChangeTrackerIdentifiesAddedAuthor()
        {
            var builder = new DbContextOptionsBuilder<PubContext>().UseSqlite("Filename=:memory:");
            using var context = new PubContext(builder.Options);
            var author = new Author { FirstName = "a", LastName = "b" };
            context.Authors.Add(author);
            Assert.AreEqual(EntityState.Added, context.Entry(author).State); //Checking pub context doing the correct thing, ef core behaviour not the database
        }

        [TestMethod]
        public void InsertAuthorsReturnsCorrectResultNumber()
        {
            using PubContext context = SetUpSQLiteMemoryContextWithOpenConnection();
            var authorList = new Dictionary<string, string>
                { { "a" , "b" },
                  { "c" , "d" },
                  { "d" , "e" }
                };

            var dl = new DataLogic(context);
            Assert.AreEqual(authorList.Count, dl.ImportAuthors(authorList));
        }

        private static PubContext SetUpSQLiteMemoryContextWithOpenConnection()
        {
            var builder = new DbContextOptionsBuilder<PubContext>().UseSqlite("Filename=:memory:");
            var context = new PubContext(builder.Options);
            context.Database.OpenConnection();
            context.Database.EnsureCreated();
            return context;
        }
    }
}