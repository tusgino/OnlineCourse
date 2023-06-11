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
        public int Page { get; set; }
        public void ValidateData()
        {
            title_like ??= String.Empty;
            start_reg_user ??= 0;
            end_reg_user ??= int.MaxValue;
            start_rate ??= 1;
            end_rate ??= 5;
        }
    }
}
