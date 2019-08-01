using System;
using System.Collections.Generic;
using GQL.WebApp.Conventions.Data;
using GQL.WebApp.Conventions.Data.Repositories;
using GraphQL.Conventions;
using GraphQL.Conventions.Relay;

namespace GQL.WebApp.Conventions.Schema.Types
{
    [Description("A book")]
    public class Book : INode
    {
        private readonly BookDto _dto;

        public Book(BookDto dto)
        {
            _dto = dto;
        }

        [Description("A unique book identifier.")]
        public Id Id => Id.New<Book>(_dto.Id);

        [Description("The book's title.")]
        public NonNull<string> Title => _dto.Title;

        [Description("The book's release date.")]
        public DateTime? ReleaseDate => _dto.ReleaseDate;

        [Description("The authors of the book.")]
        public IEnumerable<NonNull<Author>> Authors(
            [Inject] IAuthorRepository authorRepository)
        {
            if (_dto?.AuthorIds != null)
            {
                foreach (var authorId in _dto.AuthorIds)
                {
                    var dto = authorRepository.GetAuthorById(authorId);
                    yield return new Author(dto);
                }
            }
        }
    }
}
