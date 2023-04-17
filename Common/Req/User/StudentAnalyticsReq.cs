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
    }
}
