using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Req.TradeDetail
{
    public class TradeDetailFilteringReq
    {
        public bool? is_rent { get; set; }
        public bool? is_purchase { get; set; }
        public bool? is_success { get; set; }
        public bool? is_pending { get; set; }
        public bool? is_failed { get; set; }
        public DateTime? start_date { get; set; }
        public DateTime? end_date { get; set; }
        public long? start_balance { get; set; }
        public long? end_balance { get; set; }
        public int Page { get; set; }
        public void ValidateData()
        {
            is_rent ??= true;
            is_purchase ??= true;
            is_success ??= true;
            is_pending ??= true;
            is_failed ??= true;
            start_date ??= new DateTime(1800, 1, 1);
            end_date ??= new DateTime(9999, 1, 1);
            start_balance ??= 0;
            end_balance ??= long.MaxValue;
        }
    }
}
