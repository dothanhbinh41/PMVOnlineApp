using PMVOnline.Accounts.Models;
using PMVOnline.Common.Bases;
using PMVOnline.Common.Services;
using PMVOnline.Tasks.Models;
using PMVOnline.Tasks.Services;
using Prism.Navigation;
using Prism.Services;
using Prism.Services.Dialogs;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.CommunityToolkit.ObjectModel;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace PMVOnline.Tasks.ViewModels
{
    public class TaskRatingViewModel : ViewModelBase
    {
        public bool Edited { get; set; }
        public bool Editable { get; set; } = false;
        public bool IsFollowed { get; set; }
        public TaskModel Task { get; set; }
        public List<TaskModel> ReferenceTasks { get; set; }
        public ObservableCollection<FileModel> Files { get; set; }
        public List<CommentModel> Comments { get; set; }
        long taskId;
        string note;
        UserModel user;
        UserModel[] users;
        UserModel assignee;

        readonly INavigationService navigationService;
        readonly ITaskService taskService;
        readonly IApplicationSettings applicationSettings;
        readonly IFileService fileService;
        readonly IPageDialogService pageDialogService;
        readonly IDialogService dialogService;

        public TaskRatingViewModel(
            INavigationService navigationService,
            ITaskService taskService,
            IApplicationSettings applicationSettings,
            IFileService fileService,
            IPageDialogService pageDialogService,
            IDialogService dialogService)
        {
            this.navigationService = navigationService;
            this.taskService = taskService;
            this.applicationSettings = applicationSettings;
            this.fileService = fileService;
            this.pageDialogService = pageDialogService;
            this.dialogService = dialogService;
        }

        public override void Initialize(INavigationParameters parameters)
        {
            base.Initialize(parameters);
            user = applicationSettings.User;
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
            Files = new ObservableCollection<FileModel>(await taskService.GetTaskFilesAsync(id));
        }

        async Task GetReferenceTasks(long id)
        {
            ReferenceTasks = new List<TaskModel>(await taskService.GetReferenceTasksAsync(id));
        }

        public override void OnNavigatedTo(INavigationParameters parameters)
        {
            base.OnNavigatedTo(parameters); 
            taskId = parameters.GetValue<long>(NavigationKey.TaskId);
            LoadData();
        }

        List<TaskModel> myTasks;
        async Task LoadData()
        {
            IsBusy = true;
            myTasks = new List<TaskModel>(await taskService.GetMyTasksAsync());
            await System.Threading.Tasks.Task.WhenAll(GetDetails(taskId), GetComments(taskId), GetFiles(taskId), GetReferenceTasks(taskId)).ContinueWith(t => IsBusy = false);
            if (Task == null)
            {
                return;
            }

            users = await taskService.GetAllUsersAsync(Task.Target.Id);
            assignee = new UserModel { Id = Task.AssigneeId, FullName = Task.Assignee };
            Edited = (Task.CreatorId == user.Id && Task.Status >= Models.TaskStatus.Completed && Task.Status != Models.TaskStatus.Rated && Task.Status != Models.TaskStatus.Done) ||
                (Task.LeaderId == user.Id && Task.Status >= Models.TaskStatus.Completed && Task.Status != Models.TaskStatus.LeaderRated && Task.Status != Models.TaskStatus.Done);
            if (Task.Status == Models.TaskStatus.Rejected || Task.Status == Models.TaskStatus.Completed || Task.Status == Models.TaskStatus.Incompleted)
            {
                note = (await taskService.GetNoteAsync(taskId));
            }
            IsBusy = false;
            if (!Edited)
            {
                return;
            }
            await Rate();
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

        ICommand _ReadCommand;
        public ICommand ReadCommand => _ReadCommand = _ReadCommand ?? new AsyncCommand(ExecuteReadCommand);
        async Task ExecuteReadCommand()
        {
            if (string.IsNullOrEmpty(note))
            {
                return;
            }
            await pageDialogService.DisplayAlertAsync("Lý do", note, "ok");
        }


        ICommand _DownloadCommand;
        public ICommand DownloadCommand => _DownloadCommand = _DownloadCommand ?? new AsyncCommand<FileModel>(ExecuteDownloadCommand);
        async Task ExecuteDownloadCommand(FileModel file)
        {
            await Xamarin.Essentials.Browser.OpenAsync(fileService.DownloadString(file.Id));
        }
         
        ICommand _RateCommand;
        public ICommand RateCommand => _RateCommand = _RateCommand ?? new AsyncCommand(ExecuteRateCommand);
        async Task ExecuteRateCommand()
        {
            await Rate();
        }

        async Task Rate()
        {
            if (!Edited)
            {
                return;
            }
            var ratingParam = await dialogService.ShowDialogAsync(DialogRoutes.Rating, new DialogParameters { { NavigationKey.TaskId, taskId } });
            if (!ratingParam.Parameters.ContainsKey(NavigationKey.Rating))
            {
                return;
            }
            var rating = ratingParam.Parameters.GetValue<int>(NavigationKey.Rating);
            IsBusy = true;
            var result = await taskService.RateAsync(taskId, rating);
            IsBusy = false;
            if (result)
            {
                Toast("Thanh cong");
                await navigationService.GoBackAsync(new NavigationParameters { { NavigationKey.Reload, true } });
            }
            else
            {
                await navigationService.GoBackAsync();
            }
        }
    }
}
