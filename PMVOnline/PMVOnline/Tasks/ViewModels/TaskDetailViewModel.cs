﻿using PMVOnline.Accounts.Models;
using PMVOnline.Common.Bases;
using PMVOnline.Common.Services;
using PMVOnline.Tasks.Models;
using PMVOnline.Tasks.Services;
using Prism.Navigation;
using Prism.Services;
using Prism.Services.Dialogs;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.CommunityToolkit.ObjectModel;

namespace PMVOnline.Tasks.ViewModels
{
    public class TaskDetailViewModel : ViewModelBase
    {
        public string ButonText => Task == null ? "" : Task.Status == Models.TaskStatus.Pending ? "Yêu cầu duyệt" : "Kết thúc";
        public bool Editable { get; set; }
        public bool IsFollowed { get; set; } = true;
        public TaskDetailModel Task { get; set; }
        public List<FileModel> Files { get; set; }
        public List<CommentModel> Comments { get; set; }
        long taskId;
        string note;
        UserModel user;


        readonly INavigationService navigationService;
        readonly ITaskService taskService;
        readonly IApplicationSettings applicationSettings;
        readonly IFileService fileService;
        readonly IPageDialogService dialogService;

        public TaskDetailViewModel(
            INavigationService navigationService,
            ITaskService taskService,
            IApplicationSettings applicationSettings,
            IFileService fileService,
            IPageDialogService dialogService)
        {
            this.navigationService = navigationService;
            this.taskService = taskService;
            this.applicationSettings = applicationSettings;
            this.fileService = fileService;
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
            Files = new List<FileModel>(await taskService.GetTaskFilesAsync(id));
        }

        public override void OnNavigatedTo(INavigationParameters parameters)
        {
            base.OnNavigatedTo(parameters);
            if (parameters.GetNavigationMode() == NavigationMode.Back)
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

        async Task LoadData()
        {
            IsBusy = true;
            await System.Threading.Tasks.Task.WhenAll(GetDetails(taskId), GetComments(taskId), GetFiles(taskId)).ContinueWith(t => IsBusy = false);
            if (Task == null)
            {
                return;
            }
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
            await dialogService.DisplayAlertAsync("Lý do", note, "ok");
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
    }
}
