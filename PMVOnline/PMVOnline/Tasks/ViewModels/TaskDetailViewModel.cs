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
    public class TaskDetailViewModel : ViewModelBase
    {
        public string ButonText => Task == null ? "" : Task.Status == Models.TaskStatus.Pending ? "Yêu cầu duyệt" : "Kết thúc";

        public string ButtonEdit => Edited ? "Lưu" : "Sửa";
        public bool Edited { get; set; }
        public bool Editable { get; set; }
        public bool IsFollowed { get; set; } = true;
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
        readonly IDateTimeService dateTimeService;

        public TaskDetailViewModel(
            INavigationService navigationService,
            ITaskService taskService,
            IApplicationSettings applicationSettings,
            IFileService fileService,
            IPageDialogService pageDialogService,
            IDialogService dialogService,
            IDateTimeService dateTimeService)
        {
            this.navigationService = navigationService;
            this.taskService = taskService;
            this.applicationSettings = applicationSettings;
            this.fileService = fileService;
            this.pageDialogService = pageDialogService;
            this.dialogService = dialogService;
            this.dateTimeService = dateTimeService;
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
            if (parameters.GetNavigationMode() == Prism.Navigation.NavigationMode.Back)
            {
                if (parameters.ContainsKey("Comment"))
                {
                    IsBusy = true;
                    GetComments(taskId).ContinueWith(t => IsBusy = false);
                }
                if (parameters.ContainsKey(NavigationKey.Reload))
                {
                    LoadData();
                }
                return;
            }
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

            users = await taskService.GetAllUsersAsync(Task.Target.Target);
            assignee = new UserModel { Id = Task.AssigneeId, FullName = Task.Assignee };
            Editable = (user.Id == Task.AssigneeId && Task.Status == Models.TaskStatus.Approved) || (user.Id == Task.CreatorId && Task.Status == Models.TaskStatus.Pending);
            if (Task.Status == Models.TaskStatus.Rejected || Task.Status == Models.TaskStatus.Completed || Task.Status == Models.TaskStatus.Incompleted)
            {
                note = (await taskService.GetNoteAsync(taskId));
            }
            IsBusy = false;
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
            var result = await taskService.FollowTaskAsync(taskId, IsFollowed);
            IsBusy = false;
            if (result)
            {
                Toast(IsFollowed ? "Theo dõi thành công" : "Bỏ theo dõi thành công");
            }
            else
            {
                Toast("Lỗi xảy ra");
            }
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


        ICommand _ReOpenCommand;
        public ICommand ReOpenCommand => _ReOpenCommand = _ReOpenCommand ?? new AsyncCommand(ExecuteReOpenCommand);
        async Task ExecuteReOpenCommand()
        {
            if (Task.Status == Models.TaskStatus.Approved || Task.Status == Models.TaskStatus.Pending)
            {
                return;
            }
            IsBusy = true;
            var result = await taskService.ReopenAsync(taskId);
            IsBusy = false;
            if (result)
            {
                Toast("Mở lại thành công");
                await navigationService.GoBackAsync(new NavigationParameters { { NavigationKey.Reload, true } });
            }
            else
            {
                Toast("Lỗi xảy ra");
            }
        }

        ICommand _FinishCommand;
        public ICommand FinishCommand => _FinishCommand = _FinishCommand ?? new AsyncCommand(ExecuteFinishCommand);
        async Task ExecuteFinishCommand()
        {
            if (Task.Status == Models.TaskStatus.Approved)
            {
                await navigationService.NavigateAsync(Routes.CompleteTask, new NavigationParameters { { NavigationKey.TaskId, taskId } });
            }
            else
            {
                IsBusy = true;
                var result = await taskService.RequestAsync(taskId);
                IsBusy = false;
                if (result)
                {
                    Toast("Yêu cầu duyệt thành công");
                    await navigationService.GoBackAsync(new NavigationParameters { { NavigationKey.Reload, true } });
                }
                else
                {
                    Toast("Lỗi xảy ra");
                }
            }
        }


        ICommand _DownloadCommand;
        public ICommand DownloadCommand => _DownloadCommand = _DownloadCommand ?? new AsyncCommand<FileModel>(ExecuteDownloadCommand);
        async Task ExecuteDownloadCommand(FileModel file)
        {
            await Xamarin.Essentials.Browser.OpenAsync(fileService.DownloadString(file.Id));
        }

        ICommand _ChooseTargetCommand;
        public ICommand ChooseTargetCommand => _ChooseTargetCommand = _ChooseTargetCommand ?? new AsyncCommand(ExecuteChooseTargetCommand);
        async Task ExecuteChooseTargetCommand()
        {
            var result = await dialogService.ShowDialogAsync(DialogRoutes.ChooseTarget, Task.Target != null ? new DialogParameters { { NavigationKey.Target, Task.Target } } : new DialogParameters { });
            if (result?.Parameters?.ContainsKey(NavigationKey.Target) == true)
            {
                Task.Target = result.Parameters.GetValue<TargetModel>(NavigationKey.Target);
                IsBusy = true;
                assignee = await taskService.GetAssigneeAsync(Task.Target.Target); 
                users = await taskService.GetAllUsersAsync(Task.Target.Target);
                IsBusy = false;
                if (assignee == null)
                {
                    return;
                }
                Task.Assignee = assignee.FullName;
                Task.AssigneeId = assignee.Id;
            }
        }

        ICommand _ChoosePriorityCommand;
        public ICommand ChoosePriorityCommand => _ChoosePriorityCommand = _ChoosePriorityCommand ?? new AsyncCommand(ExecuteChoosePriorityCommand);
        async Task ExecuteChoosePriorityCommand()
        {
            var result = await dialogService.ShowDialogAsync(DialogRoutes.ChoosePriority, new DialogParameters { { NavigationKey.Priority, Task.Priority } });
            if (result?.Parameters?.ContainsKey(NavigationKey.Priority) == true)
            {
                Task.Priority = result.Parameters.GetValue<TaskPriority>(NavigationKey.Priority);
            }
        }

        ICommand _ChooseDueDateCommand;
        public ICommand ChooseDueDateCommand => _ChooseDueDateCommand = _ChooseDueDateCommand ?? new AsyncCommand(ExecuteChooseDueDateCommand);
        async Task ExecuteChooseDueDateCommand()
        {
            if (Task == null)
            {
                return;
            }
            var date = await dateTimeService.PickDateTimeAsync(DateTime.Now, Task.Date);
            if (date.HasValue)
            {
                Task.Date = date.Value;
                Task.DueDate = date.Value;
            }
        }

        ICommand _ReferenceTasksCommand;
        public ICommand ReferenceTasksCommand => _ReferenceTasksCommand = _ReferenceTasksCommand ?? new AsyncCommand(ExecuteReferenceTasksCommand);
        async Task ExecuteReferenceTasksCommand()
        {
            if (Edited)
            {
                var param = await dialogService.ShowDialogAsync(DialogRoutes.MultiSelectTask, new DialogParameters { { NavigationKey.ReferenceTasks, ReferenceTasks }, { NavigationKey.MyTasks, myTasks }, { NavigationKey.Editable, true } });
                if (param?.Parameters?.ContainsKey(NavigationKey.ReferenceTasks) == true)
                {
                    ReferenceTasks = param.Parameters.GetValue<List<TaskModel>>(NavigationKey.ReferenceTasks).ToList();
                }
                return;
            }

            await dialogService.ShowDialogAsync(DialogRoutes.MultiSelectTask, new DialogParameters { { NavigationKey.ReferenceTasks, ReferenceTasks }, { NavigationKey.MyTasks, ReferenceTasks }, { NavigationKey.Editable, false } });
        }

        ICommand _AddFileCommand;
        public ICommand AddFileCommand => _AddFileCommand = _AddFileCommand ?? new AsyncCommand(ExecuteAddFileCommand);
        async Task ExecuteAddFileCommand()
        {
            if (!Edited)
            {
                return;
            }

            var files = await FilePicker.PickMultipleAsync(PickOptions.Default);
            if (files?.Any() == true)
            {
                foreach (var item in files)
                {
                    Files?.Add(new FileModel { ContentType = item.ContentType, FileName = item.FileName, FullPath = item.FullPath });
                }
            }
        }

        ICommand _RemoveFileCommand;
        public ICommand RemoveFileCommand => _RemoveFileCommand = _RemoveFileCommand ?? new AsyncCommand<FileModel>(ExecuteRemoveFileCommand);
        async Task ExecuteRemoveFileCommand(FileModel file)
        {
            if (Edited)
                Files?.Remove(file);
            else
                await ExecuteDownloadCommand(file);
        }

        ICommand _EditCommand;
        public ICommand EditCommand => _EditCommand = _EditCommand ?? new AsyncCommand(ExecuteEditCommand);
        async Task ExecuteEditCommand()
        {
            if (user.Id != Task.CreatorId)
            {
                return;
            }

            Edited = !Edited;
            if (!Edited)
            {
                IsBusy = true;
                await UploadFiles();
                var result = await taskService.UpdateTaskAsync(Task);
                IsBusy = false;
                if (result)
                {
                    Toast("Cập nhật thành công");
                }
                else
                {
                    Toast("Cập nhật thất bại");
                }
            }
        }

        ICommand _ChooseAssigneeCommand;
        public ICommand ChooseAssigneeCommand => _ChooseAssigneeCommand = _ChooseAssigneeCommand ?? new AsyncCommand(ExecuteChooseAssigneeCommand);
        async Task ExecuteChooseAssigneeCommand()
        {
            if (users == null || assignee == null)
            {
                return;
            }

            var selected = await dialogService.ShowDialogAsync(DialogRoutes.ChooseUsers, new DialogParameters { { NavigationKey.Count, 1 }, { NavigationKey.SelectedUsers, new List<UserModel> { assignee } }, { NavigationKey.Users, new List<UserModel>(users) } });
            if (selected.Parameters.ContainsKey(NavigationKey.SelectedUsers))
            {
                assignee = selected.Parameters.GetValue<List<UserModel>>(NavigationKey.SelectedUsers).FirstOrDefault();
                Task.AssigneeId = assignee?.Id ?? Guid.Empty;
                Task.Assignee = assignee?.FullName;
            }
        }


        public void OnReferenceTasksChanged()
        {
            if (Task == null)
            {
                return;
            }
            Task.ReferenceTasks = ReferenceTasks?.ToArray() ?? new TaskModel[0];
        }

        public void OnFilesChanged()
        {
            if (Task == null)
            {
                return;
            }
            Task.Files = Files?.Select(d => d.Id)?.ToArray() ?? new Guid[0];
        }

        async Task UploadFiles()
        {
            List<Task<FileModel>> files = new List<Task<FileModel>>();
            var notUpload = Files?.Where(d => d.Id == Guid.Empty)?.ToList() ?? new List<FileModel>();
            var uploaded = Files?.Where(d => d.Id != Guid.Empty)?.ToArray() ?? new FileModel[0];
            for (int i = 0; i < notUpload.Count; i++)
            {
                var file = notUpload[i];
                var stream = await (new FileResult(file.FullPath)).OpenReadAsync();
                files.Add(fileService.UploadAsync(stream, file.FileName));
            }

            var result = await System.Threading.Tasks.Task.WhenAll(files);
            Task.Files = result?.Select(d => d.Id)?.Concat(uploaded.Select(d => d.Id))?.ToArray() ?? new Guid[0];
        }
    }
}
