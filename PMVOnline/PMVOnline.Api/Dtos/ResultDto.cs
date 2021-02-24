using System;
using System.Collections.Generic;
using System.Text;

namespace PMVOnline.Apis.Dtos
{
    public class ResultDto<T>
    {
        public T Result { get; set; }
        public bool? Success { get; set; }
    }

    public class ResultListDto<T>
    {
        public int Count { get; set; }
        public T[] Items { get; set; }
    }
}
