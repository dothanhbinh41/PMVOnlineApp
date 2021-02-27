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

        public static string TargetToString(this TaskTarget target)
        {
            switch (target)
            { 
                case TaskTarget.BuyCommodity:
                    return "Mua hàng"; 
                case TaskTarget.Payment:
                    return "Thanh toán"; 
                case TaskTarget.Storage:
                    return "Kiểm tra tồn kho"; 
                case TaskTarget.Make:
                    return "Sản xuất"; 
                case TaskTarget.BuyOther:
                    return "Mua sắm"; 
                case TaskTarget.Other:
                default:
                    return "Khác"; 
            }
        }
    }
}
