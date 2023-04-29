using BLL;
using Common.Rsp.DTO;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace OnlineCourse.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PurchaseController : ControllerBase
    {
        private PurchaseSvc _purchaseSvc = new PurchaseSvc();
        [HttpPost("Purchase-a-course")]
        [Authorize]
        public IActionResult PurchaseACourse(PurchaseReq purchaseReq)
        {
            var res = _purchaseSvc.PurchaseACourse(purchaseReq);
            if (res.Success)
            {
                return Ok(res);
            }
            else
            {
                return BadRequest(res.Message);
            }
            
        }
    }
}
