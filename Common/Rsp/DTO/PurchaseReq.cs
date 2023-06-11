using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Rsp.DTO
{
    public class PurchaseReq
    {
        public string? Email { get; set; }
        public Guid IdCourse { get; set; }
        public int TypeOfPurchase { get; set; }
    }
}
