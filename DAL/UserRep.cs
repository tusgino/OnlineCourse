using Common.DAL;
using DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL
{
    public class UserRep : GenericRep<WebsiteKhoaHocOnline_V4Context, User>
    {
        public User GetUserByID(Guid? id)
        {
            using(WebsiteKhoaHocOnline_V4Context context = new WebsiteKhoaHocOnline_V4Context())
            {
                var user = context.Users.Where(u => u.IdUser== id).FirstOrDefault();
                if (user != null)
                    context.Entry(user)!.Reference(u => u.IdTypeOfUserNavigation).Load();
                return user;
            }
        }
    }
}
