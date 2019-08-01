using System;
using System.Collections.Generic;
using GraphQL.Conventions;

namespace GQL.WebApp.Conventions.Schema.Input
{
    [InputType]
    [Description("A book")]
    public class BookInput
    {
        [Description("The book's title.")]
        public NonNull<string> Title { get; set; }

        [Description("The book's release date.")]
        public DateTime? ReleaseDate { get; set; }

        [Description("The IDs of the book's authors.")]
        public List<Id> Authors { get; set; }
    }
}
