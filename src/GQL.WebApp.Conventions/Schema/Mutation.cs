using System.Linq;
using GQL.WebApp.Conventions.Data;
using GQL.WebApp.Conventions.Data.Repositories;
using GQL.WebApp.Conventions.Schema.Input;
using GQL.WebApp.Conventions.Schema.Output;
using GQL.WebApp.Conventions.Schema.Types;
using GraphQL.Conventions;
using GraphQL.Conventions.Relay;

namespace GQL.WebApp.Conventions.Schema
{
    [ImplementViewer(OperationType.Mutation)]
    public class Mutation
    {
        [RelayMutation]
        public AddAuthorResult AddAuthor(
            [Inject] IAuthorRepository authorRepository,
            AddAuthorParams input)
        {
            var dto = new AuthorDto
            {
                FirstName = input.Author.Value.FirstName,
                LastName = input.Author.Value.LastName,
            };
            dto.Id = authorRepository.AddAuthor(dto);
            return new AddAuthorResult
            {
                Author = new Author(dto),
            };
        }

        [RelayMutation]
        public AddBookResult AddBook(
            [Inject] IBookRepository bookRepository,
            AddBookParams input)
        {
            var dto = new BookDto
            {
                Title = input.Book.Value.Title,
                AuthorIds = input.Book.Value.Authors
                    .Select(author => int.Parse(author.IdentifierForType<Author>()))
                    .ToList(),
                ReleaseDate = input.Book.Value.ReleaseDate,
            };
            dto.Id = bookRepository.AddBook(dto);
            return new AddBookResult
            {
                Book = new Book(dto),
            };
        }
    }
}
