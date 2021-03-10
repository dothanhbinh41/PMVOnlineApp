using PMVOnline.Common.Bases;
using PMVOnline.Tasks.Models;
using PMVOnline.Tasks.Services;
using Prism.Navigation;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.CommunityToolkit.ObjectModel;

namespace PMVOnline.Tasks.ViewModels
{
    public class TaskDetailViewModel : ViewModelBase
    {
        public bool IsFollowed { get; set; } = true;
        public TaskDetailModel Task { get; set; }
        public List<FileModel> Files { get; set; }
        public List<CommentModel> Comments { get; set; }


        private readonly INavigationService navigationService;
        private readonly ITaskService taskService;

        public TaskDetailViewModel(
            INavigationService navigationService, 
            ITaskService taskService)
        {




            Task = new TaskDetailModel
            {
                Assignee = "Do Thanh Binh",
                DueDate = DateTime.Now,
                Priority = TaskPriority.High,
                Status = Models.TaskStatus.Pending,
                Title = "Chua biet",
                Content = "ai ma biet duoc",
                Target = new TargetModel { Target = TaskTarget.BuyOther, },
                ReferenceTasks = new long[] { 1123, 1313, 5555 }
            };

            Files = new List<FileModel> {
            new FileModel
            {
                FileName  = "a.jadfkald",
                FullPath = "https://media.congluan.vn/files/thanhduyen/2020/05/01/ngoc-trinh-lay-chong-1231.jpg"
            }
            };

            Comments = new List<CommentModel>
            {
                new CommentModel
                {
                    Content = "Toi nay gap em anh nhe, voi ngay mai chung minh co the di choi xa duoc khong anh nhi,hehe ",
                    Date = DateTime.Now,
                    Sender = "Do Thanh Binh",
                     Files = new FileModel[]
                     {
                        new FileModel
                        {
                            FileName  = "a.jadfkald",
                            FullPath = "https://media.congluan.vn/files/thanhduyen/2020/05/01/ngoc-trinh-lay-chong-1231.jpg"
                        }
                    }
                }
            };
            this.navigationService = navigationService;
            this.taskService = taskService;
        }

        public override void OnNavigatedTo(INavigationParameters parameters)
        {
            base.OnNavigatedTo(parameters); 
        }

        ICommand _CommentCommand;
        public ICommand CommentCommand => _CommentCommand = _CommentCommand ?? new AsyncCommand(ExecuteCommentCommand);
        async Task ExecuteCommentCommand()
        {
            await navigationService.NavigateAsync(Routes.Comment);
        }
         
        ICommand _HistoryCommand;
        public ICommand HistoryCommand => _HistoryCommand = _HistoryCommand ?? new AsyncCommand(ExecuteHistoryCommand);
        async Task ExecuteHistoryCommand()
        {
            await navigationService.NavigateAsync(Routes.History);
        }
         
        ICommand _FollowCommand;
        public ICommand FollowCommand => _FollowCommand = _FollowCommand ?? new AsyncCommand(ExecuteFollowCommand);
        async Task ExecuteFollowCommand()
        {
            IsFollowed = !IsFollowed;
        }
         

        ICommand _ReOpenCommand;
        public ICommand ReOpenCommand => _ReOpenCommand = _ReOpenCommand ?? new AsyncCommand(ExecuteReOpenCommand);
        async Task ExecuteReOpenCommand()
        {
        } 
    }
}
