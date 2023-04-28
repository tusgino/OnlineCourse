using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Rsp.DTO
{
    public class TradeDetailDTO
    {
        public Guid IdTrade { get; set; }
        public int? TypeOfTrade { get; set; }
        public int? TradeStatus { get; set; }
        public string? Balance { get; set; }
        public string? RequiredBalance { get; set; }
        public DateTime? DateOfTrade { get; set; }
        public UserDTO? User { get; set; }

    }
}
