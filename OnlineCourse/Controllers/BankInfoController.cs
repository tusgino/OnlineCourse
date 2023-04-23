using BLL;
using Common.Req.BankInfo;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using System.Data;

namespace OnlineCourse.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BankInfoController : ControllerBase
    {
        private BankInfoSvc _bankInfoSvc = new BankInfoSvc();

        [HttpGet("Get-By-IdBankInfo")]
        [Authorize(Roles = "Expert")]

        public IActionResult GetBankInfoByIDUser(Guid idBankInfo)
        {
            var res = _bankInfoSvc.GetBankInfoByID(idBankInfo);
            if(res.Success)
            {
                return Ok(res);
            }
            else
            {
                return BadRequest(res.Message);
            }
        }

        [HttpPost("Add-BankInfo")]
        [Authorize]
        public IActionResult AddBankInfo([FromBody] BankInfoReq bankInfoReq)
        {
            var rsp = _bankInfoSvc.AddBankInfo(bankInfoReq);
            if(rsp.Success)
            {
                return Ok(rsp);
            }
            else
            {
                return BadRequest(rsp.Message);
            }
        }

        [HttpPatch("Update-BankInfo/{idBankInfo}")]
        public IActionResult UpdateBanKInfo(Guid idBankInfo, [FromBody] JsonPatchDocument patchDoc)
        {
            var rsp = _bankInfoSvc.UpdateBankInfo(idBankInfo, patchDoc);
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
