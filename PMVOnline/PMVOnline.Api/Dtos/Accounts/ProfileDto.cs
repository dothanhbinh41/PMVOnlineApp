using System;
using System.Collections.Generic;
using System.Text;

namespace PMVOnline.Api.Dtos.Accounts
{
    public class ProfileDto
    {
        public string UserName { get; set; }
        public string Email { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public string PhoneNumber { get; set; }
        public bool HasPassword { get; set; }
        public bool IsExternal { get; set; } 
    }
}
