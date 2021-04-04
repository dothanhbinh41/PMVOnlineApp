using PMVOnline.Accounts.Models;
using PMVOnline.Common.Bases;
using Prism.Services.Dialogs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.CommunityToolkit.ObjectModel;
using Xamarin.Forms;

namespace PMVOnline.Tasks.ViewModels.Dialogs
{
    public class UserWrapper : ModelBase
    {
        public bool IsSelected { get; set; }
        public UserModel User { get; set; }
    }
    public class ChooseUsersViewModel : DialogViewModelBase
    {
        public override event Action<IDialogParameters> RequestClose;
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public List<UserWrapper> Users { get; set; }
        public List<UserModel> SelectedUsers { get; set; }


        int count = int.MaxValue;
        public ChooseUsersViewModel()
        {
        }

        public override void OnDialogOpened(IDialogParameters parameters)
        {
            base.OnDialogOpened(parameters);
            SelectedUsers = parameters.GetValue<List<UserModel>>(NavigationKey.SelectedUsers) ?? new List<UserModel>();
            if (parameters.ContainsKey(NavigationKey.Count))
            {
                count = parameters.GetValue<int>(NavigationKey.Count);
            }
            Users = parameters.GetValue<List<UserModel>>(NavigationKey.Users).Select(d => new UserWrapper { User = d, IsSelected = SelectedUsers.Any(c => c.Id == d.Id) }).ToList();
        }

        ICommand _DoneCommand;
        public ICommand DoneCommand => _DoneCommand = _DoneCommand ?? new AsyncCommand(ExecuteDoneCommand);
        async Task ExecuteDoneCommand()
        {
            RequestClose?.Invoke(new DialogParameters { { NavigationKey.SelectedUsers, Users.Where(d => d.IsSelected).Select(d => d.User).ToList() } });
        }

        ICommand _CloseCommand;
        public ICommand CloseCommand => _CloseCommand = _CloseCommand ?? new AsyncCommand(ExecuteCloseCommand);
        async Task ExecuteCloseCommand()
        {
            RequestClose?.Invoke(new DialogParameters());
        }

        ICommand _SelectCommand;
        public ICommand SelectCommand => _SelectCommand = _SelectCommand ?? new Command<UserWrapper>(ExecuteSelectCommand);
        void ExecuteSelectCommand(UserWrapper user)
        {
            if (user != null)
            {
                if (Users.Count(d => d.IsSelected) >= count)
                {
                    Users.FirstOrDefault(d => d.IsSelected).IsSelected = false;
                }
                user.IsSelected = !user.IsSelected;
            }
        }
    }
}
