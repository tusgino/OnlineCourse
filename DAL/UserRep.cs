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
    public class UserRep : GenericRep<WebsiteKhoaHocOnline_V4Context, User>
    {
        public User FindUserByID(Guid iD_User)
        {
            return All.SingleOrDefault(u => u.IdUser == iD_User)!;
        }

        public User GetUserByID(Guid? id)
        {
            using(WebsiteKhoaHocOnline_V4Context context = new WebsiteKhoaHocOnline_V4Context())
            {
                var user = context.Users.Where(u => u.IdUser== id).FirstOrDefault();
                return user;
            }
        }

        public bool UpdateUserByID(Guid id, JsonPatchDocument newUser) {
            var user = FindUserByID(id);
            if (user != null)
            {
                newUser.ApplyTo(user);
                Context.SaveChanges();
                return true;
            }
            return false;
        }
    }
}
