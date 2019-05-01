using System.Collections.Generic;

namespace GQL.Client.QueryBuilders
{
    public class AppRequestDto
    {
        public UserDto User { get; set; }

        public List<UserDto> Users { get; set; }



        public class UserDto
        {
            public string Id { get; set; }

            public string Email { get; set; }
        }
    }
}