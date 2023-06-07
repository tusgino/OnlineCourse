using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Req.User
{
    public class UserFilteringReq
    {
        public string? title_like { get; set; }
        public DateTime? start_date_create { get; set; }
        public DateTime? end_date_create { get; set; }
        public bool? is_student { get; set; }
        public bool? is_expert { get; set; }
        public bool? is_admin { get; set; }
        public bool? status_active { get; set; }
        public bool? status_banned { get; set; }
        public int Page { get; set; }
        public void ValidateData()
        {
            title_like ??= String.Empty;
            start_date_create ??= new DateTime(1, 1, 1);
            end_date_create ??= new DateTime(9999, 1, 1);
            is_student ??= true;
            is_expert ??= true;
            status_active ??= true;
            status_active ??= true;
            status_banned ??= true;
        }
    }
}
