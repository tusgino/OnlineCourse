using BLL;
using Common.Req.Degree;
using DAL.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;

namespace OnlineCourse.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DegreeController : ControllerBase
    {
        private DegreeSvc _degreeSvc = new DegreeSvc();

        [HttpGet("{idUser}")]
        [Authorize(Roles = "Expert")]
        public IActionResult GetDegreesByIdUser(Guid idUser)
        {
            var res = _degreeSvc.GetDegreesByIdUser(idUser);
            if(res.Success)
            {
                return Ok(res);
            }
            else
            {
                return BadRequest(res.Message);
            }
        }

        [HttpGet("getByID/{idDegree}")]
        //[Authorize(Roles = "Expert")]
        public IActionResult GetDegreesByIdDegree(Guid idDegree)
        {
            var res = _degreeSvc.GetDegreeByIdDegree(idDegree);
            if (res.Success)
            {
                return Ok(res);
            }
            else
            {
                return BadRequest(res.Message);
            }
        }

        [HttpPost("Add-degree")]
        [Authorize(Roles = "Expert")]
        public IActionResult AddDegree([FromBody] DegreeReq degreeReq)
        {
            var res = _degreeSvc.AddDegree(degreeReq);
            if (res.Success)
            {
                return Ok(res);
            }
            else
            {
                return BadRequest(res.Message);
            }
        }

        [HttpPatch("Update-degree/{idDegree}")]
        [Authorize(Roles = "Expert")]
        public IActionResult UpdateDegree(Guid idDegree, JsonPatchDocument patchDoc)
        {
            var rsp = _degreeSvc.UpdateDegree(idDegree, patchDoc);
            if (rsp.Success)
            {
                return Ok(rsp);
            }
            else
            {
                return BadRequest(rsp.Message);
            }
        }
    }
}
