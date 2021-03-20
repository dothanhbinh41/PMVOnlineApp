using PMVOnline.Common.Bases;
using PMVOnline.Tasks.Models;
using PMVOnline.Tasks.Services;
using Prism.Navigation;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.CommunityToolkit.ObjectModel;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace PMVOnline.Tasks.ViewModels
{
    public class CommentViewModel : ViewModelBase
    {
        public string Comment { get; set; }
        public ObservableCollection<FileModel> Files { get; set; }

        long taskId;
        readonly INavigationService navigationService;
        readonly IFileService fileService;
        readonly ITaskService taskService;

        public CommentViewModel(
            INavigationService navigationService,
            IFileService fileService,
            ITaskService taskService)
        {
            this.navigationService = navigationService;
            this.fileService = fileService;
            this.taskService = taskService;
        }

        public override void Initialize(INavigationParameters parameters)
        {
            base.Initialize(parameters);
            Files = new ObservableCollection<FileModel>();
        }

        public override void OnNavigatedTo(INavigationParameters parameters)
        {
            base.OnNavigatedTo(parameters);
            taskId = parameters.GetValue<long>(NavigationKey.TaskId);
        }

        ICommand _AddFileCommand;
        public ICommand AddFileCommand => _AddFileCommand = _AddFileCommand ?? new AsyncCommand(ExecuteAddFileCommand);
        async Task ExecuteAddFileCommand()
        {
            var files = await FilePicker.PickMultipleAsync(PickOptions.Default);
            if (files?.Any() == true)
            {
                foreach (var item in files)
                {
                    Files.Add(new FileModel { ContentType = item.ContentType, FileName = item.FileName, FullPath = item.FullPath });
                }
            }
        }

        ICommand _RemoveFileCommand;
        public ICommand RemoveFileCommand => _RemoveFileCommand = _RemoveFileCommand ?? new Command<FileModel>(ExecuteRemoveFileCommand);
        void ExecuteRemoveFileCommand(FileModel file)
        {
            Files?.Remove(file);
        }

        async Task<Guid[]> UploadFiles()
        {
            List<Task<FileModel>> files = new List<Task<FileModel>>();
            for (int i = 0; i < Files.Count; i++)
            {
                var file = Files[i];
                var stream = await (new FileResult(file.FullPath)).OpenReadAsync();
                files.Add(fileService.UploadAsync(stream, file.FileName));
            }

            return await System.Threading.Tasks.Task.WhenAll(files).ContinueWith(t => t.Result?.Select(c => c.Id)?.ToArray());
        }

        ICommand _SendCommand;
        public ICommand SendCommand => _SendCommand = _SendCommand ?? new AsyncCommand(ExecuteSendCommand);
        async Task ExecuteSendCommand()
        {
            if (string.IsNullOrEmpty(Comment))
            {
                return;
            }
            IsBusy = true;
            var files = await UploadFiles();
            var result = await taskService.SendCommentAsync(taskId, Comment, files);
            IsBusy = false;

            if (result)
            {
                Toast("Comment thành công");
                await navigationService.GoBackAsync(new NavigationParameters { { "Comment", true } });
            }
            else
            {
                Toast("Có lỗi xảy ra");
            }
        }

    }
}
