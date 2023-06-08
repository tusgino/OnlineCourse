using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Req.User
{
    public class StudentAnalyticsReq
    {
        public string? student_name_like { get; set; }
        public int? start_purchase_course { get; set; }
        public int? end_purchase_course { get; set; }
        public int? start_finish_course { get; set; }
        public int? end_finish_course { get; set; }
        public int Page { get; set; }
        public void ValidateData()
        {
            student_name_like ??= String.Empty;
            start_purchase_course ??= 0;
            end_purchase_course ??= int.MaxValue;
            start_finish_course ??= 0;
            end_finish_course= int.MaxValue;
        }
    }
}
