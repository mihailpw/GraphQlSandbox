using System.Collections.Generic;

namespace GQL.Client.Dto
{
    public class UserDto
    {
        public string Id { get; set; }

        public string Email { get; set; }

        public List<UserDto> Friends { get; set; }
    }
}