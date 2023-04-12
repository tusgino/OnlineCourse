using Common.BLL;
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

        public SingleRsp UpdateUser(Guid iD_User, JsonPatchDocument patchDoc)
        {
            var rsp = new SingleRsp();
            if(!_userRep.UpdateUserByID(iD_User, patchDoc))
            {
                rsp.SetError("Update failed");
            }
                       
            return rsp;
        }
    }
}
