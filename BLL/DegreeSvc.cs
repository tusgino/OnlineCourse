using Common.BLL;
using Common.Req.BankInfo;
using Common.Req.Degree;
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
        public SingleRsp GetDegreeByIdDegree(Guid idDegree)
        {
            var rsp = new SingleRsp();

            if ((rsp.Data = _degreeRep.GetDegreeByIdDegree(idDegree)) == null)
            {
                rsp.SetError($"Not found any degree has id = {idDegree}");
            }

            return rsp;
        }

        public SingleRsp AddDegree(DegreeReq degreeReq)
        {
            var rsp = new SingleRsp();

            if(!_degreeRep.AddDegree(new Degree
            {
                IdDegree = Guid.NewGuid(),
                Description= degreeReq.Description,
                IdUser= degreeReq.IdUser,
                Image = degreeReq.Image,
                Name= degreeReq.Name,
            }))
            {
                rsp.SetError("Can not Add degree because Not found IdUser");
            }

            return rsp;
        }

        public SingleRsp UpdateDegree(Guid idDegree, JsonPatchDocument patchDoc)
        {
            var rsp = new SingleRsp();

            if (!_degreeRep.UpdateDegree(idDegree, patchDoc))
            {
                rsp.SetError("Can not update Degree");
            }

            return rsp;
        }
    }
}
