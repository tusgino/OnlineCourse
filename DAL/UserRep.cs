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
        public User GetUserByID(Guid? id)
        {
            return All.Where(u => u.IdUser == id).FirstOrDefault()!;
        }
        public bool UpdateUserByID(Guid id, JsonPatchDocument newUser)
        {
            var user = GetUserByID(id);
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
