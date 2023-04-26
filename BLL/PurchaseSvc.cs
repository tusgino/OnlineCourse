using Common.Rsp;
using Common.Rsp.DTO;
using DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL
{
    public class PurchaseSvc
    {
        private PurchaseRep _purchaseRep = new PurchaseRep();

        public SingleRsp PurchaseACourse(PurchaseReq purchaseReq)
        {
            var rsp = new SingleRsp();
            if (Guid.Equals(rsp.Data = _purchaseRep.PurchaseACourse(purchaseReq.IdCourse, purchaseReq.Email, purchaseReq.TypeOfPurchase), Guid.Empty))
            {
                rsp.SetError("Can not purchase this course");
            }

            return rsp;
        }
    }
}
