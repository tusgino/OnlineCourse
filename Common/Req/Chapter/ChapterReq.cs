using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Req.Chapter
{
    public class ChapterReq
    {
        public Guid IdChapter { get; set; }
        public string? Name { get; set; }
        public int? Index { get; set; }
        public Guid? IdCourse { get; set; }
    }
}
