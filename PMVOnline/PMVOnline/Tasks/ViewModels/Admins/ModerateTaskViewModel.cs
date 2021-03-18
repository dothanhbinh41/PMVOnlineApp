using PMVOnline.Common.Bases;
using PMVOnline.Tasks.Models;
using PMVOnline.Tasks.Services;
using Prism.Navigation;
using Prism.Services;
using Prism.Services.Dialogs;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.CommunityToolkit.ObjectModel;
using Xamarin.Forms;

namespace PMVOnline.Tasks.ViewModels.Admins
{
    public class ModerateTaskViewModel : ViewModelBase
    {
        public bool IsFollowed { get; set; } = true;
        public TaskDetailModel Task { get; set; }
        public List<FileModel> Files { get; set; }
        public List<CommentModel> Comments { get; set; }
        long taskId;

        readonly INavigationService navigationService;
        readonly ITaskService taskService; 

        public ModerateTaskViewModel(
            INavigationService navigationService,
            ITaskService taskService,
            IPageDialogService pageDialogService)
        {
            this.navigationService = navigationService;
            this.taskService = taskService; 
        }

        async Task GetDetails(long id)
        {
            Task = await taskService.GetTaskAsync(id);
        }

        async Task GetComments(long id)
        {
            Comments = new List<CommentModel>(await taskService.GetTaskCommentsAsync(id));
        }

        async Task GetFiles(long id)
        {
            Files = new List<FileModel>(await taskService.GetTaskFilesAsync(id));
        }

        public override void OnNavigatedTo(INavigationParameters parameters)
        {
            base.OnNavigatedTo(parameters);
            if (parameters.GetNavigationMode() == NavigationMode.Back)
            {
                return;
            }
            taskId = parameters.GetValue<long>(NavigationKey.TaskId);
            IsBusy = true;
            System.Threading.Tasks.Task.WhenAll(GetDetails(taskId), GetComments(taskId), GetFiles(taskId)).ContinueWith(t => IsBusy = false);
        }
         

        ICommand _CommentCommand;
        public ICommand CommentCommand => _CommentCommand = _CommentCommand ?? new AsyncCommand(ExecuteCommentCommand);
        async Task ExecuteCommentCommand()
        {
            await navigationService.NavigateAsync(Routes.Comment, new NavigationParameters { { NavigationKey.TaskId, taskId } });
        }

        ICommand _HistoryCommand;
        public ICommand HistoryCommand => _HistoryCommand = _HistoryCommand ?? new AsyncCommand(ExecuteHistoryCommand);
        async Task ExecuteHistoryCommand()
        {
            await navigationService.NavigateAsync(Routes.History, new NavigationParameters { { NavigationKey.TaskId, taskId } });
        }

        ICommand _FollowCommand;
        public ICommand FollowCommand => _FollowCommand = _FollowCommand ?? new AsyncCommand(ExecuteFollowCommand);
        async Task ExecuteFollowCommand()
        {
            IsFollowed = !IsFollowed;
            IsBusy = true;
            await taskService.FollowTaskAsync(taskId, IsFollowed);
            IsBusy = false;
        }


        ICommand _RejectCommand;
        public ICommand RejectCommand => _RejectCommand = _RejectCommand ?? new AsyncCommand(ExecuteRejectCommand);
        async Task ExecuteRejectCommand()
        {
            IsBusy = true;
            var result = await taskService.ApproveTaskAsync(taskId, false, string.Empty);
            IsBusy = false;
            if (result)
            {
                Toast("Không duyệt thành công");
                await navigationService.GoBackAsync();
            }
            else
            {
                Toast("Không duyệt không thành công");
            }
        }

        ICommand _ApproveCommand;
        public ICommand ApproveCommand => _ApproveCommand = _ApproveCommand ?? new AsyncCommand(ExecuteApproveCommand);
        async Task ExecuteApproveCommand()
        {
            IsBusy = true;
            var result = await taskService.ApproveTaskAsync(taskId, true, string.Empty);
            IsBusy = false;
            if (result)
            {
                Toast("Duyệt thành công");
                await navigationService.GoBackAsync();
            }
            else
            {
                Toast("Duyệt không thành công");
            }
        }
    }
}
