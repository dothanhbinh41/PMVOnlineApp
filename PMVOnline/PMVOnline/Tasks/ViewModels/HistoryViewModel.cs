using PMVOnline.Common.Bases;
using PMVOnline.Tasks.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace PMVOnline.Tasks.ViewModels
{
    public class HistoryViewModel : ViewModelBase
    {
        public List<HistoryModel> Histories { get; set; }
        public HistoryViewModel()
        {
            Histories = new List<HistoryModel>
            {
                 new HistoryModel
                 {
                     Action = "lam gi do",
                     Assignee = "Do Thanh Binh",
                     Date = DateTime.Now
                 }
            };
        }
    }
}
