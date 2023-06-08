using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Req.User
{
    public class ExpertAnalyticsReq
    {
        public string? expert_name { get; set; }
        public int? start_upload_course { get; set; }
        public int? end_upload_course { get; set; }
        public long? start_revenue { get; set; }
        public long? end_revenue { get; set; }
        public int Page { get; set; }
        public void ValidateData()
        {
            expert_name ??= string.Empty;
            start_upload_course ??= 0;
            end_upload_course ??= int.MaxValue;
            start_revenue ??= 0;
            end_revenue ??= int.MaxValue;
        }
    }
}
