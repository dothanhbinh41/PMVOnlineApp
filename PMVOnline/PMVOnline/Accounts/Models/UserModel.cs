using PMVOnline.Common.Bases;
using System;
using System.Collections.Generic;
using System.Text;

namespace PMVOnline.Accounts.Models
{
    public class UserModel : ModelBase
    {
        public string Email { get; set; }
        public string FullName { get; set; }
    }
}
