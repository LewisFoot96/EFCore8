using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.OpenApi;
using PublishData;
using PublisherDomain;
using PubApplication.Mapping;
using PubApplication.DTOs;
using MediatR;
using PublishApplication.Commands;
namespace PubApi;

public static class AuthorEndpoints
{
    public static void MapAuthorEndpoints(this IEndpointRouteBuilder routes)
    {
        var group = routes.MapGroup("/api/Author").WithTags(nameof(Author));

        group.MapGet("/", async (PubContext db) =>
        {
            return await db.Authors.AsNoTracking().ToListAsync();
        })
        .WithName("GetAllAuthors")
        .WithOpenApi();

        group.MapGet("/{authorid}", async Task<Results<Ok<AuthorDto>, NotFound>> (int authorid, PubContext db, IMediator mediator) =>
        {
            //var resultnew = mediator.Send(new CreateAuthorCommand() { AuthorId = authorid });
            var result = await db.Authors.AsNoTracking()
                .FirstOrDefaultAsync(a => a.AuthorId == authorid);

            return result!.ToAuthorDto()
                is AuthorDto model
                    ? TypedResults.Ok(model)
                    : TypedResults.NotFound();
        })
        .WithName("GetAuthorById")
        .WithOpenApi();

        group.MapPut("/{authorid}", async Task<Results<Ok, NotFound>> (int authorid, AuthorDto author, PubContext db) =>
        {
            var affected = await db.Authors
                .Where(model => model.AuthorId == authorid)
                .ExecuteUpdateAsync(setters => setters
                    .SetProperty(m => m.FirstName, author.FirstName)
                    .SetProperty(m => m.LastName, author.LastName)
                    );
            return affected == 1 ? TypedResults.Ok() : TypedResults.NotFound();
        })
        .WithName("UpdateAuthor")
        .WithOpenApi();

        group.MapPost("/", async (AuthorDto author, PubContext db) =>
        {
            var auth = author.ToAuthor();
            db.Authors.Add(auth);
            await db.SaveChangesAsync();
            return TypedResults.Created($"/api/Author/{auth.AuthorId}", author);
        })
        .WithName("CreateAuthor")
        .WithOpenApi();

        group.MapDelete("/{authorid}", async Task<Results<Ok, NotFound>> (int authorid, PubContext db) =>
        {
            var affected = await db.Authors
                .Where(model => model.AuthorId == authorid)
                .ExecuteDeleteAsync();
            return affected == 1 ? TypedResults.Ok() : TypedResults.NotFound();
        })
        .WithName("DeleteAuthor")
        .WithOpenApi();
    }
}
