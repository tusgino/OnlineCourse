using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Req.Course
{
    public class CoursesPaginationReq
    {
        public int Page { get; set; }
        public int Limit { get; set; }
        public string? Title_like { get; set; } = string.Empty;
    }
}
