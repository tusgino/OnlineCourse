using BLL;
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
    }
}
