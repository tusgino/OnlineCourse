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
    }
}
