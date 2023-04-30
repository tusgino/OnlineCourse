using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Rsp.DTO
{
    public class TradeModel_Admin
    {
        public Guid ID { get; set; }
        public int? TypeOfTrade { get; set; }
        public string? Username { get; set; } = string.Empty;
        public string? Balance { get; set; } = string.Empty;
        public string? RequiredBalance { get; set; } = string.Empty;
        public string? DateOfTrade { get; set; } = string.Empty;
        public int? TradeStatus { get; set; }
    }
}
