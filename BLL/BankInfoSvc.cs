using Common.BLL;
using Common.Req.BankInfo;
using Common.Rsp;
using DAL;
using DAL.Models;
using Microsoft.AspNetCore.JsonPatch;
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

        public SingleRsp GetBankInfoByID(Guid idBankInfo)
        {
            var rsp = new SingleRsp();

            if((rsp.Data = _bankInfoRep.GetBankInfoByID(idBankInfo)) == null)
            {
                rsp.SetError($"Not found bankinfo id = {idBankInfo}");
            }

            return rsp;
        }

        public SingleRsp AddBankInfo(BankInfoReq bankInfoReq)
        {
            var rsp = new SingleRsp();
            var bankInfo = new BankInfo
            {
                IdBankAccount = Guid.NewGuid(),
                AccountName = bankInfoReq.AccountName,
                BankAccountNumber = bankInfoReq.BankAccountNumber,
                BankName = bankInfoReq.BankName,
            };

            if(_bankInfoRep.AddBankInfoByIdUser(bankInfo.IdBankAccount, bankInfoReq.IdUser))
            {
                rsp.SetError($"Not found user have id = {bankInfoReq.IdUser}");
            }
            else
            {
                _bankInfoRep.AddBankInfo(bankInfo);
            }
            return rsp;
        }

        public SingleRsp UpdateBankInfo(Guid idBankInfo, JsonPatchDocument patchDoc)
        {
            var rsp = new SingleRsp();

            if(!_bankInfoRep.UpdateBankInfo(idBankInfo, patchDoc))
            {
                rsp.SetError("Can not update Bankinfo");
            }

            return rsp;
        }
    }
}
