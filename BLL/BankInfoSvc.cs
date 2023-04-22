using Common.BLL;
using Common.Rsp;
using DAL;
using DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL
{
    public class BankInfoSvc : GenericSvc<BankInfoRep, BankInfo>
    {
        private BankInfoRep _bankInfoRep = new BankInfoRep();

        public SingleRsp GetBankInfoByIDUser(Guid idUser)
        {
            var rsp = new SingleRsp();

            if((rsp.Data = _bankInfoRep.GetBankInfoByIDUser(idUser)) == null)
            {
                rsp.SetError($"Not found bankinfo of User have id = {idUser}");
            }

            return rsp;
        }
    }
}
