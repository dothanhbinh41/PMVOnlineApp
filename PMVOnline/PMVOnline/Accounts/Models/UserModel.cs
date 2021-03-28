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

        public DepartmentModel[] Departments { get; set; }
    }

    public class DepartmentModel : ModelBase
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public bool IsLeader { get; set; }
    }

    public class DepartmentName
    {
        public const string Stocker = "stocker";
        public const string Accountant = "accountant";
        public const string Buy = "buy";
        public const string Director = "director";
    }
}
