using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Rsp.DTO
{
    public class LessonDTO
    {
        public Guid IdLesson { get; set; }
        public int? Index { get; set; }
        public string? Title { get; set; }
        public string? Video { get; set; }

    }
}
