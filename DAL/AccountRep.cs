using Common.DAL;
using Common.Req.Account;
using DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;

namespace DAL
{
    public class AccountRep : GenericRep<WebsiteKhoaHocOnline_V4Context, Account>
    {
        public Account GetAccountByID(Guid id)
        {
            return All.SingleOrDefault(acc => acc.IdAccount == id);
        }

        public User GetUserByLogin(string username, string password)
        {
            using (WebsiteKhoaHocOnline_V4Context context = new WebsiteKhoaHocOnline_V4Context())
            {
                if(All.SingleOrDefault(acc => acc.Username== username && acc.Password == password) != null)
                {
                    var account = All.SingleOrDefault(acc => acc.Username == username && acc.Password == password);
                    var user = context.Users.SingleOrDefault(user => user.IdAccountNavigation == account);
                    context?.Entry(user).Reference(u => u.IdTypeOfUserNavigation).Load();
                    return user;
                }

                return null;

            }
        }

        public bool ValidRegister(RegisterReq registerReq)
        {
            using(WebsiteKhoaHocOnline_V4Context context = new WebsiteKhoaHocOnline_V4Context())
            {
                return All.SingleOrDefault(acc => acc.Username == registerReq.Username) != null;
            }
        }

        public void AddAccount(Account account)
        {
            using(WebsiteKhoaHocOnline_V4Context context = new WebsiteKhoaHocOnline_V4Context())
            {
                context.Accounts.Add(account);
                context.SaveChanges();
            }
        }

        public void AddUser(User user)
        {
            using (WebsiteKhoaHocOnline_V4Context context = new WebsiteKhoaHocOnline_V4Context())
            {
                context.Users.Add(user);
                context.SaveChanges();
            }
        }
    }
}
