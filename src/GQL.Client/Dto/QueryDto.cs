using System.Collections.Generic;

namespace GQL.Client.Dto
{
    public class QueryDto
    {
        public UserDto User { get; set; }

        public List<UserDto> Users { get; set; }
    }
}