using System;
using System.Collections.Generic;

namespace GQL.WebApp.Conventions.Data
{
    public class BookDto
    {
        public int Id { get; set; }

        public string Title { get; set; }

        public List<int> AuthorIds { get; set; }

        public DateTime? ReleaseDate { get; set; }
    }
}
