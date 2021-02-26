using PMVOnline.Accounts.Models;
using PMVOnline.Tasks.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace PMVOnline.Tasks.Services
{
    public interface ITaskService
    {
        Task<UserModel> GetAssigneeAsync(TaskTarget target);
    }
}
