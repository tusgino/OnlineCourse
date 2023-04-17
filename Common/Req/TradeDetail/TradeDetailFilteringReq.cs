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
        public string? start_balance { get; set; }
        public string? end_balance { get; set; }
    }
}
