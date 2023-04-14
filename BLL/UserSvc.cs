using Common.BLL;
using Common.Req.User;
using Common.Rsp;
using DAL;
using DAL.Models;
using Microsoft.EntityFrameworkCore.Query;
using Microsoft.AspNetCore.JsonPatch;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL
{
    public class UserSvc : GenericSvc<UserRep, User>
    {
        private readonly UserRep _userRep = new UserRep();
        public SingleRsp GetUserByID(Guid? id)
        {
            var rsp = new SingleRsp();
            if((rsp.Data = _userRep.GetUserByID(id)) == null)
            {
                rsp.SetError("Not found user");
            }
            return rsp;
        }
        public SingleRsp GetAllUsersByFiltering(UserFilteringReq userFilteringReq, int page)
        {
            if (userFilteringReq.start_date_create == null) userFilteringReq.start_date_create = new DateTime(1, 1, 1);
            if (userFilteringReq.end_date_create == null) userFilteringReq.end_date_create = new DateTime(9999, 1, 1);

            var users = _userRep.GetAllUsersByFiltering(userFilteringReq.text, userFilteringReq.start_date_create, userFilteringReq.end_date_create, userFilteringReq.is_student, userFilteringReq.is_expert, userFilteringReq.is_admin, userFilteringReq.status_active, userFilteringReq.status_banned);

            int limit = 10;
            int offset = (page - 1) * limit;
            int total = users.Count;
            int totalPage = (total % limit == 0) ? (total / limit) : (1 + total / limit);

            var data = users.Skip(offset).Take(limit).ToList();

            var rsp = new SingleRsp();

            if (data == null)
            {
                rsp.SetError("Not found user");
            }
            else
            {
                rsp.Data = data;
            }
            return rsp;
        }
        public SingleRsp UpdateUser(Guid iD_User, JsonPatchDocument patchDoc)
        {
            var rsp = new SingleRsp();
            if (!_userRep.UpdateUserByID(iD_User, patchDoc))
            {
                rsp.SetError("Update failed");
            }
            return rsp;
        }
    }
}
