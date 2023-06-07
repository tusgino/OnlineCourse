using BLL;
using Common.Req.Course;
using Common.Req.TradeDetail;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Data;

namespace OnlineCourse.Controllers
{
    [Route("api/private/[controller]")]
    [ApiController]
    public class TradeDetailController : ControllerBase
    {
        private readonly TradeDetailSvc _tradeDetailSvc = new TradeDetailSvc();
        [HttpGet("Get-all-tradedetail-by-filtering")]
        [Authorize(Roles = "Admin")]
        public IActionResult GetAllTradeDetailByFiltering([FromQuery] TradeDetailFilteringReq tradeDetailFilteringReq)
        {
            var res = _tradeDetailSvc.GetAllTradeDetailsByFiltering(tradeDetailFilteringReq);

            if (res.Success)
            {
                return Ok(res);
            }
            else
            {
                return BadRequest(res.Message);
            }
        }
        [HttpPatch("Update-trade-by-{ID_Trade}")]
        [Authorize(Roles = "Admin")]
        public IActionResult UpdateTrade(Guid ID_Trade, [FromBody] JsonPatchDocument patchDoc)
        {
            var res = _tradeDetailSvc.UpdateTrade(ID_Trade, patchDoc);
            if (res.Success)
            {
                return Ok(res);
            }
            else
            {
                return BadRequest(res.Message);
            }
        }
        [HttpGet("Get-system-revenue")]
        [Authorize(Roles = "Admin")]
        public IActionResult GetSystemRevenue()
        {
            var res = _tradeDetailSvc.GetSystemRevenue();
            if (res.Success)
            {
                return Ok(res.Data);
            }
            else
            {
                return BadRequest(res.Message);
            }
        }
        [HttpGet("Get-trade-by-id-{ID_Trade}")]
        [Authorize]
        public IActionResult GetTradeByID(Guid ID_Trade)
        {
            var res = _tradeDetailSvc.GetTradeByID(ID_Trade);
            if(res.Success)
            {
                return Ok(res.Data);
            }
            else
            {
                return BadRequest(res.Message);
            }
        }

        [HttpGet("Get-rents-by-{IdExpert}")]
        [Authorize]
        public IActionResult GetRentByIdExpert(Guid IdExpert, int page)
        {
            var res = _tradeDetailSvc.GetRentByIdExpert(IdExpert, page);
            if (res.Success)
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
