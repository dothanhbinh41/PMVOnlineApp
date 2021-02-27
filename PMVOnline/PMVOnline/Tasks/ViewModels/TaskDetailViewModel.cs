using PMVOnline.Common.Bases;
using PMVOnline.Tasks.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace PMVOnline.Tasks.ViewModels
{
    public class TaskDetailViewModel : ViewModelBase
    {
        public TaskDetailModel Task { get; set; }
        public List<FileModel> Files { get; set; }
        public List<CommentModel> Comments { get; set; }

        public TaskDetailViewModel()
        {
            Task = new TaskDetailModel
            {
                Assignee = "Do Thanh Binh",
                DueDate = DateTime.Now,
                Priority = TaskPriority.High,
                Status = TaskStatus.Pending,
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
        }
    }
}
