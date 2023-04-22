using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Req.BankInfo
{
    public class BankInfoReq
    {
        public string? BankAccountNumber { get; set; }
        public string? AccountName { get; set; }
        public string? BankName { get; set; }
        public Guid IdUser { get; set; }
    }
}
