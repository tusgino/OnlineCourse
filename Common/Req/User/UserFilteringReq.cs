using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Req.User
{
    public class UserFilteringReq
    {
        public string? text { get; set; }
        public DateTime? start_date_create { get; set; }
        public DateTime? end_date_create { get; set; }
        public bool? is_student { get; set; }
        public bool? is_expert { get; set; }
        public bool? is_admin { get; set; }
        public bool? status_active { get; set; }
        public bool? status_banned { get; set; }
    }
}
