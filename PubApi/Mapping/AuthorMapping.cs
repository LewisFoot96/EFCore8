using PubApi.DTOs;
using PublisherDomain;

namespace PubApi.Mapping
{
    public static class AuthorMapping
    {
        public static AuthorDto ToAuthorDto(this Author author)
        {
            return new AuthorDto(author.AuthorId, author.FirstName, author.LastName);
        }

        public static Author ToAuthor(this AuthorDto authorDto)
        {
            return new Author
            {
                AuthorId = authorDto.authorId,
                FirstName= authorDto.FirstName,
                LastName= authorDto.LastName
            };
        }
    }
}
