using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Req.Lesson
{
    public class LessonReq
    {
        public Guid? IdChapter { get; set; }
        public string? Description { get; set; }
        public int? Index { get; set; }
        public string? Video { get; set; }
        public double? Duration { get; set; }
        public string? Title { get; set; }
    }
}
