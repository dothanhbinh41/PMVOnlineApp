using PMVOnline.Common.Bases;
using PMVOnline.Tasks.Models;
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

        public TaskDetailViewModel(INavigationService navigationService)
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
        }



        ICommand _CommentCommand;
        public ICommand CommentCommand => _CommentCommand = _CommentCommand ?? new AsyncCommand(ExecuteCommentCommand);
        async Task ExecuteCommentCommand()
        {
            await navigationService.NavigateAsync(Routes.Comment);
        }

    }
}
