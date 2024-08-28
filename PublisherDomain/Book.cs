namespace PublisherDomain
{
    public class Book
    {
        public int BookId { get; set; }
        public string Title { get; set; }
        public DateOnly PublishDate { get; set; }
        public decimal BasePrice { get; set; }
        public Author Author { get; set; }
        //Can make this nullable, so it is not required for the relationship
        public int AuthorId { get; set; }
        //One to one, principlas are always required
        public BookCover BookCover { get; set; }   
    }
}
