using PMVOnline.Tasks.ViewModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace PMVOnline.Tasks.Models
{
    public class CreateTaskModel: TaskBaseModel
    {
        public TargetModel Target { get; set; }
    }
}
