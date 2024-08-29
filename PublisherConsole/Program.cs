using Microsoft.EntityFrameworkCore;
using PublishData;
using PublisherDomain;
using System.Reflection.Metadata.Ecma335;

PubContext _context = new PubContext();


//There are 4 methods to load data in EF core, get related data from the database: 
// - Eager loading -> include related objects in query
// - Query Projections -> define the shape of query results
// - Explicit loading -> explicitly request related fata fro obejcts in memory
// - Lazy loading -> on the fly retriveval of data related to objects in memory


using (PubContext pubContext = new PubContext())
{
    //EF core will check to ensure database is creted and create if it doesn't
    //pubContext.Database.EnsureCreated();

    //AddAuthor();
    // UpdateAuthor();
    GetAuthors();
}

void AddAuthor()
{
    var context = new PubContext();
    context.Authors.Add(new Author
    {
        FirstName = "Lewis",
        LastName = "Foot",
        Books = [new Book { Title = "MyBook", BasePrice = 1, PublishDate = DateOnly.MinValue }]
    });

    //Uses states of tracked objects to see what has changed and to 
    //create an SQL statement that matches

    Console.WriteLine(context.ChangeTracker.DebugView.ShortView);
    //Save changes calls this to detect state changes
    context.ChangeTracker.DetectChanges();
    Console.WriteLine(context.ChangeTracker.DebugView.ShortView);
    context.SaveChanges();

}

void AddBookToAuthor(string firstName)
{
    //Getting the author and then adding new book, using the context defined
    var author = _context.Authors.FirstOrDefault(a => a.FirstName == firstName);

    if (author != null)
    {
        author.Books.Add(new Book
        {
            Title = "New Book",
            PublishDate = DateOnly.MinValue,
        });
    }

    _context.SaveChanges();
}

void AddingANewBookUsingBook()
{
    //Assuming i know a correct foreign key for the author
    var book = new Book
    {
        AuthorId = 1,
        PublishDate = DateOnly.MaxValue,
        Title = "Book added from book",
        BasePrice = 1
    };

    _context.Books.Add(book);
    _context.SaveChanges();
}

void UpdateAuthor()
{
    var context = new PubContext();
    context.Authors.FirstOrDefault()!.FirstName = "NewLewis";
    context.SaveChanges();
}

void GetAuthors()
{
    var context = new PubContext();
    var authors = context.Authors
        .Include(x => x.Books)
        .Where(x => x.FirstName == "Lewis")
        .ToList();

    var lewiAuth = context.Authors
        .Include(x => x.Books)
        .Where(x => EF.Functions.Like(x.FirstName, "L%"))
        .ToList()
        .OrderBy(x => x.LastName)
        .ThenBy(x => x.FirstName);

    foreach (var author in authors)
    {
        Console.WriteLine(author.FirstName);
        Console.WriteLine(authors[1].Books.FirstOrDefault()!.Title);
    }

    var lewisAuthor = authors.FirstOrDefault(x => x.FirstName == "Lewis");

    //Skip and Take LINQ query can be useful for paging, getting a specific number of values returned
}

void DeleteAnAuthor()
{
    var context = new PubContext();

    var author = context.Authors.Find(1);

    if (author is not null)
    {
        context.Authors.Remove(author);
        // Could use this too: generic context.Remove(author);
        context.SaveChanges();
    }

}

void ExecuteAuthorDelete()
{
    var context = new PubContext();
    //Deleting without tracking. Executes straight away and runs sql
    var count = context.Authors.Where(x => x.FirstName == "Dave").ExecuteDelete();
}

void EagerLoadBooksWithAuthors()
{
    var authors = _context.Authors.Include(x => x.Books
    .Where(x => x.PublishDate > DateOnly.MinValue).OrderBy(b => b.Title)).ToList();

    authors.ForEach(a =>
        {
            Console.WriteLine(a.FirstName);
        });
}

void QueryProjections()
{
    var someType = _context.Authors.Select(a => new { Name = a.FirstName, AuthorId = a.AuthorId, Books = a.Books.Count }).ToList();
}

//Already in memory methods start
void ExplicitLoading()
{
    //Would need an actual author, will update. Author already in memory, load a collection. Can use reference to get the author from a book in memory.
    var author = _context.Authors.FirstOrDefault();
    if (author != null)
    {
        //Enrtry used to get sepcific EF core entry, fine grain control over change tracker
        _context.Entry(author).Collection(a => a.Books).Load();
    }
}

void LazyLoading()
{
    //Requires lazy loading to be setup in your app. Quite a few steps to do, not advised. 
    var author = _context.Authors.FirstOrDefault();
    if (author != null)
    {
        foreach (var book in author.Books)
        {
            Console.WriteLine(book.Title);
        }
    }
}
//Already in memory end

void ConnectArtistsAndBookCovers()
{
    var artist = _context.Artists.Find(1);
    var bookCover = _context.BookCovers.Find(1);
    if (bookCover != null && artist != null)
    {
        var newBookCover = new BookCover
        {
            BookCoverId = 2,
            DesignIdeas = "Animal",
            DigitalOnly = true
        };
        artist.Covers.Add(newBookCover);
        artist.Covers.Add(bookCover);

        _context.SaveChanges();
    }
    //to delete would need both book cover and artist in the memeory, as using skippnig mapping. 
    //To move an artist from a book cover would have to delete original and then add new
    //You can do skip navigations with payloads, that can have a date and extra fields as payloads, ceate our own join table with the property
}

//One to one relationships

void GetAllBooksWithTheirCovers()
{
    var booksAndCovers = _context.Books.Include(b => b.BookCover).ToList();


    booksAndCovers.ForEach(book => Console.WriteLine(book.BookCover.DesignIdeas));
}

void GetThenInclude()
{
    //Getting related data, then include
    var authorGraph = _context.Authors
        .Include(a => a.Books)
        .ThenInclude(b => b.BookCover)
        .ThenInclude(bc => bc.Artists)
        .FirstOrDefault(x => x.AuthorId == 1);

}

void SimpleRawSql()
{
    //Never use SQL with parameters embedded directly into the string
    //Concatenation strings are not safe, directly into the SQL, never use
    //Formatted strings are safe if you just add the string into the parameters
    //Interprolated strings, use FromSql method for it to be safe
    var authors = _context.Authors.FromSqlRaw("select * from authors").TagWith("Raw SQL").Include(a => a.Books).ToList(); //Exact sql that is sent to the database. Can extend to use linq queries ontop

    //Stored procedure
    int start = 2010;
    int end = 2020;
    var authorsStoredProcedure = _context.Authors.FromSql($"AuthorsPuiblishedinYearRange {start}, {end}").ToList();
}

void QuerySql()
{
    //Can return objects too. Can create an internal class for example and put the returns into it. 
    var ids = _context.Database.SqlQuery<int>($"SELECT AuthordId FROM Authors").ToList();
}

//Keyless entities - always read only. Ca use database views -> to be added at a later data. Need to use HasNoKey, default EF core maps entities to tables and not views, need to use the ToView mapping
//Keyless entities will never be tracked
//We can then execute a query on a view on the database.
// TODO


//Execute query commands, get back number of rows that have been affected
void DeleteCover(int coverId)
{
    //Can delete entities without bringing them into memory
    var rowCount = _context.Database.ExecuteSqlRaw("Delete Cover {0}", coverId); //Calling stored procedure, these do not interact with the change tracker
    Console.WriteLine(rowCount);
}

//Mapping stored procedures, maybe for legacy and an explicit need to do so
void InsertNewAuthor()
{
    var author = new Author { FirstName = "Aimee", LastName = "Hardy" };
    _context.Authors.Add(author);
    _context.SaveChanges();
}