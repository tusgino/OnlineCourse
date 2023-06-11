using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Rsp.DTO
{
    public class ChapterDTO
    {
        public Guid IdChapter { get; set; }
        public string? Name { get; set; }
        public int? Index { get; set; }

        public List<LessonDTO>? Lessons { get; set; }
    }
}
