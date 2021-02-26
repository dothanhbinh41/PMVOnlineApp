using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace PMVOnline.Common.Services
{
    public interface IDateTimeService
    {
        Task<DateTime?> PickDateTimeAsync(DateTime? min = null, DateTime? current = null, DateTime? max = null);
        //Task<DateTime?> PickDateAsync(DateTime? min = null, DateTime? current = null, DateTime? max = null);
        //Task<(int, int)> PickTimeAsync();
    }
}
