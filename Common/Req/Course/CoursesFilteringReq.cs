using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Req.Course
{
    public class CoursesFilteringReq
    {
        public string? text { get; set; } = string.Empty;
        public string? category_name { get; set; } = string.Empty;
        public DateTime? start_day { get; set; }
        public DateTime? end_day { get; set; }
        public int status_active { get; set; }
        public int status_store { get; set; }
    }

}
