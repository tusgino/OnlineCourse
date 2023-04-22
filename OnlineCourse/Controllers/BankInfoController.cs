using BLL;
using Common.Req.BankInfo;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace OnlineCourse.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BankInfoController : ControllerBase
    {
        private BankInfoSvc _bankInfoSvc = new BankInfoSvc();

        [HttpGet("Get-By-IdUser")]
        public IActionResult GetBankInfoByIDUser(Guid idUser)
        {
            var res = _bankInfoSvc.GetBankInfoByIDUser(idUser);
            if(res.Success)
            {
                return Ok(res.Data);
            }
            else
            {
                return BadRequest(res.Message);
            }
        }

        [HttpPost("Add-BankInfo")]
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
    }
}
