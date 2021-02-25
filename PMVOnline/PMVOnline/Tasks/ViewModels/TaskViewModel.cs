using PMVOnline.Common.Bases;
using PMVOnline.Homes.Models;
using PMVOnline.Tasks.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace PMVOnline.Tasks.ViewModels
{
    public class TaskViewModel : ViewModelBase
    {
        public List<HotTaskModel> Tasks { get; set; }
        public TaskViewModel()
        {
            Tasks = new List<HotTaskModel> {
                new HotTaskModel{ Assignee = "Do THanh Binh", Action = "Phe Duyet", DueDate = DateTime.Now.AddDays(2), Id = 123, Priority =  TaskPriority.High},
                new HotTaskModel{ Assignee = "Do THanh Binh", Action = "Phe Duyet", DueDate = DateTime.Now.AddDays(2), Id = 123, Priority =  TaskPriority.Normal},
                new HotTaskModel{ Assignee = "Do THanh Binh", Action = "Phe Duyet", DueDate = DateTime.Now.AddDays(2), Id = 123, Priority =  TaskPriority.Highest},
            };
        }
    }
}
