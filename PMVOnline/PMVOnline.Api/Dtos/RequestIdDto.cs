using System;
using System.Collections.Generic;
using System.Text;

namespace PMVOnline.Apis.Dtos
{
    public class RequestIdDto<T>
    {
        public T Id { get; set; }
    }
}
