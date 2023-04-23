using Common.DAL;
using DAL.Models;
using Microsoft.AspNetCore.JsonPatch;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL
{
    public class BankInfoRep : GenericRep<WebsiteKhoaHocOnline_V4Context, BankInfo>
    {
        public bool AddBankInfo(BankInfo bankInfo)
        {
            using (WebsiteKhoaHocOnline_V4Context context = new WebsiteKhoaHocOnline_V4Context())
            {
                context.BankInfos.Add(bankInfo);
                context.SaveChanges();
                return true;
            }
        }

        public bool AddBankInfoByIdUser(Guid idBankAccount, Guid idUser)
        {
            using (WebsiteKhoaHocOnline_V4Context context = new WebsiteKhoaHocOnline_V4Context())
            {
                User? user;
                if ((user = context.Users.SingleOrDefault(u => u.IdUser == idUser)) != null)
                {
                    user.IdBankAccount= idBankAccount;
                    context.Users.Update(user);
                    context.SaveChanges();
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }

        public BankInfo GetBankInfoByID(Guid idBankInfo)
        {
            using(WebsiteKhoaHocOnline_V4Context context = new WebsiteKhoaHocOnline_V4Context())
            {
                return context.BankInfos.SingleOrDefault(b => b.IdBankAccount == idBankInfo)!;
            }
        }

        public bool UpdateBankInfo(Guid idBankInfo, JsonPatchDocument patchDoc)
        { 
            using(WebsiteKhoaHocOnline_V4Context context = new WebsiteKhoaHocOnline_V4Context())
            {
                var bankInfo = context.BankInfos.SingleOrDefault(b => b.IdBankAccount==idBankInfo);
                if(bankInfo != null)
                {
                    patchDoc.ApplyTo(bankInfo);
                    context.SaveChanges() ;
                    return true;
                }
                return false;
                
            }
        }
    }
}
