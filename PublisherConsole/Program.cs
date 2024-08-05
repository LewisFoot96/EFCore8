using Microsoft.EntityFrameworkCore;
using PublishData;
using PublisherDomain;

using (PubContext pubContext = new PubContext())
{
    //EF core will check to ensure database is creted and create if it doesn't
    //pubContext.Database.EnsureCreated();

    //AddAuthor();
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