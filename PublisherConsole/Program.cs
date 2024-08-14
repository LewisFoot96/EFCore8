using Microsoft.EntityFrameworkCore;
using PublishData;
using PublisherDomain;

PubContext _context = new PubContext();

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