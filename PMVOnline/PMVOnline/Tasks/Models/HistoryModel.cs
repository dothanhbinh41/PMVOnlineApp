using PMVOnline.Common.Bases;
using System;
using System.Collections.Generic;
using System.Text;

namespace PMVOnline.Tasks.Models
{
    public class HistoryModel : ModelBase
    {
        public string Actor { get; set; }
        public string Action { get; set; }
        public DateTime Date { get; set; }
    }
}
