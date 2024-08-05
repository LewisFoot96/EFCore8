using Microsoft.EntityFrameworkCore;
using PublishData;
using PublisherDomain;

using (PubContext pubContext = new PubContext())
{
    pubContext.Database.EnsureCreated();

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
    var authors = context.Authors.Include(x => x.Books).ToList();

    foreach (var author in authors)
    {
        Console.WriteLine(author.FirstName);
        Console.WriteLine(authors[1].Books.FirstOrDefault()!.Title);
    }
}