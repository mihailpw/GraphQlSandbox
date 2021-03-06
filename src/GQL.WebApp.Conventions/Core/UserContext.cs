using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using GQL.WebApp.Conventions.Data.Repositories;
using GQL.WebApp.Conventions.Schema.Types;
using GraphQL.Conventions;
using GraphQL.Conventions.Relay;

namespace GQL.WebApp.Conventions.Core
{
    public class UserContext : IUserContext, IDataLoaderContextProvider
    {
        private readonly IBookRepository _bookRepository = new BookRepository();

        private readonly IAuthorRepository _authorRepository = new AuthorRepository();

        public Task<T> Get<T>(Id id)
            where T : class
        {
            if (id.IsIdentifierForType<Book>())
            {
                var dto = _bookRepository.GetBookById(int.Parse(id.IdentifierForType<Book>()));
                var result = new Book(dto) as T;
                return Task.FromResult(result);
            }
            else if (id.IsIdentifierForType<Author>())
            {
                var dto = _authorRepository.GetAuthorById(int.Parse(id.IdentifierForType<Author>()));
                var result = new Author(dto) as T;
                return Task.FromResult(result);
            }
            throw new ArgumentException($"Unable to derive type from identifier '{id}'");
        }

        public IEnumerable<INode> Search(string searchString)
        {
            foreach (var dto in _bookRepository.SearchForBooksByTitle(searchString))
            {
                yield return new Book(dto);
            }
            foreach (var dto in _authorRepository.SearchForAuthorsByLastName(searchString))
            {
                yield return new Author(dto);
            }
        }

        public Task FetchData(CancellationToken token)
        {
            return Task.CompletedTask;
        }
    }
}
