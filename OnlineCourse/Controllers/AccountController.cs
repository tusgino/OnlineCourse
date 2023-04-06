using BLL;
using Common.Req;
using Common.Req.Account;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace OnlineCourse.Controllers
{
    [Route("api/public/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly AccountSvc _accountSvc = new AccountSvc();
        [HttpGet("id")]
        public IActionResult GetAccountByID(Guid id)
        {
            var rsp = _accountSvc.GetAccountByID(id);
            if (rsp.Success)
            {
                return Ok(rsp);
            }
            else
            {
                return BadRequest(rsp.Message);
            }
        }

        [HttpPost("Login")]
        public IActionResult Login([FromForm] LoginReq loginReq)
        {
            var rsp = _accountSvc.Login(loginReq);

            if(rsp.Success)
            {
                return Ok(rsp);
            }
            else
            {
                return BadRequest(rsp.Message);
            }
        }

        [HttpPost("Register")]
        public IActionResult Register([FromForm] RegisterReq registerReq)
        {
            var rsp = _accountSvc.Register(registerReq);
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
