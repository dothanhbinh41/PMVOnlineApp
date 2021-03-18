using PMVOnline.Common.Bases;
using System;
using System.Collections.Generic;
using System.Text;

namespace PMVOnline.Accounts.Models
{
    public class UserModel : ModelBase
    {
        public Guid Id { get; set; }
        public string Email { get; set; }
        public string FullName => $"{Surname} {Name}";
        public string UserName { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public string PhoneNumber { get; set; }
        public bool HasPassword { get; set; }
        public bool IsExternal { get; set; }

        public RoleModel[] Roles { get; set; }
    }

    public class RoleModel : ModelBase
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
    }
}
