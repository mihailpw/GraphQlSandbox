using System.Collections.Generic;

namespace GQL.WebApp.Conventions.Data.Repositories
{
    public interface IAuthorRepository
    {
        int AddAuthor(AuthorDto author);

        AuthorDto GetAuthorById(int id);

        IEnumerable<AuthorDto> GetAuthorsByIds(List<int> ids);

        IEnumerable<AuthorDto> SearchForAuthorsByLastName(string searchString);
    }
}
