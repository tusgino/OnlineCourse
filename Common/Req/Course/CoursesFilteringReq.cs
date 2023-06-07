using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Req.Course
{
    public class CoursesFilteringReq
    {
        public string? title_like { get; set; }
        public string? category_name { get; set; }
        public DateTime? start_day { get; set; }
        public DateTime? end_day { get; set; }
        public bool? status_active { get; set; }
        public bool? status_store { get; set; }
        public int Page { get; set; }
        public void ValidateData()
        {
            title_like ??= String.Empty;
            category_name ??= String.Empty;
            start_day ??= new DateTime(1, 1, 1);
            end_day ??= new DateTime(9999, 1, 1);
            status_active ??= true;
            status_store ??= true;
        }
    }

}
