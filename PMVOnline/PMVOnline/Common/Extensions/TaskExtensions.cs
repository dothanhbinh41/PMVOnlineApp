using PMVOnline.Tasks.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace PMVOnline.Common.Extensions
{
    public static class TaskExtensions
    {
        public static string PriorityToString(this TaskPriority priority)
        {
            switch (priority)
            {
                case TaskPriority.High:
                    return "Gấp";
                case TaskPriority.Highest:
                    return "ASAP";
                case TaskPriority.Normal:
                default:
                    return "Bình Thường";
            }
        }
    }
}
