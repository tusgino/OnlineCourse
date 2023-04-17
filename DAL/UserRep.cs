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
        public List<User> GetAllUsersByFiltering(string? _title_like, DateTime? _start_date_create, DateTime? _end_date_create, bool? _is_student, bool? _is_expert, bool? _is_admin, bool? _status_active, bool? _status_banned)
        {
            using (WebsiteKhoaHocOnline_V4Context context = new WebsiteKhoaHocOnline_V4Context())
            {

                var id_accounts = context.Accounts.Where(account => account.DateCreate >= _start_date_create && account.DateCreate <= _end_date_create).Select(account => account.IdAccount).ToList();


                List<int> user_types = new List<int>();
                if (_is_admin == true) user_types.Add(0);
                if (_is_expert == true) user_types.Add(1);
                if (_is_student == true) user_types.Add(2);

                List<int> user_status = new List<int>();
                if (_status_active == true) user_status.Add(1);
                if (_status_banned == true) user_status.Add(0);

                return context.Users.Where(user =>
                    user.Name.Contains(_title_like == null ? "" : _title_like) &&
                    id_accounts.Contains(user.IdAccount ?? Guid.Empty) &&
                    user_types.Contains(user.IdTypeOfUser ?? -1) &&
                    user_status.Contains(user.Status ?? -1)
                ).ToList();
            }
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
