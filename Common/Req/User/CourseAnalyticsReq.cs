using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Req.User
{
    public class CourseAnalyticsReq
    {
        public string? title_like { get; set; }
        public int? start_reg_user { get; set; }
        public int? end_reg_user { get; set; }
        public int? start_rate { get; set; }
        public int? end_rate { get; set; }
    }
}
