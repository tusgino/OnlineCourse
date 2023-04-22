using Common.DAL;
using DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL
{
    public class BankInfoRep : GenericRep<WebsiteKhoaHocOnline_V4Context, BankInfo>
    {
        public void AddBankInfo(BankInfo bankInfo)
        {
            using (WebsiteKhoaHocOnline_V4Context context = new WebsiteKhoaHocOnline_V4Context())
            {
                context.BankInfos.Add(bankInfo);
                context.SaveChanges();
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

        public BankInfo GetBankInfoByIDUser(Guid idUser)
        {
            using(WebsiteKhoaHocOnline_V4Context context = new WebsiteKhoaHocOnline_V4Context())
            {
                var user = context.Users.SingleOrDefault(u => u.IdUser == idUser);
                if(user == null)
                {
                    return null!;
                }
                else
                {
                    return context.BankInfos.SingleOrDefault(b => b.Users.Contains(user))!;
                }
            }
        }   
    }
}
