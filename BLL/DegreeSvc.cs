using Common.BLL;
using Common.Rsp;
using DAL;
using DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL
{
    public class DegreeSvc : GenericSvc<DegreeRep, Degree>
    { 
        private DegreeRep _degreeRep = new DegreeRep();

        public SingleRsp GetDegreesByIdUser(Guid idUser)
        {
            var rsp = new SingleRsp();

            if((rsp.Data = _degreeRep.GetDegreesByIdUser(idUser)) == null)
            {
                rsp.SetError($"Not found any degrees of User have id = {idUser}");
            }
            
            return rsp;
        }
    }
}
