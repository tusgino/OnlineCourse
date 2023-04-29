﻿using BLL;
using Common.Req.Course;
using Common.Req.TradeDetail;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace OnlineCourse.Controllers
{
    [Route("api/private/[controller]")]
    [ApiController]
    public class TradeDetailController : ControllerBase
    {
        private readonly TradeDetailSvc _tradeDetailSvc = new TradeDetailSvc();
        [HttpGet("Get-all-tradedetail-by-filtering")]
        public IActionResult GetAllTradeDetailByFiltering(bool? _is_rent, bool? _is_purchase, bool? _is_success, bool? _is_pending, bool? _is_failed, DateTime? _start_date, DateTime? _end_date, string? _start_balance, string? _end_balance, int page)
        {
            TradeDetailFilteringReq tradeDetailFilteringReq = new TradeDetailFilteringReq
            {
                is_rent = _is_rent,
                is_purchase = _is_purchase,
                is_success = _is_success,
                is_pending = _is_pending,
                is_failed = _is_failed,
                start_date = _start_date,
                end_date = _end_date,
                start_balance = _start_balance,
                end_balance = _end_balance
            };

            CoursesPaginationReq coursesPaginationReq = new CoursesPaginationReq
            {
                Page = page,
                Limit = 10,
                Title_like = "",
            };

            var res = _tradeDetailSvc.GetAllTradeDetailsByFiltering(tradeDetailFilteringReq, coursesPaginationReq);

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
        public IActionResult UpdateCourse(Guid ID_Trade, [FromBody] JsonPatchDocument patchDoc)
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
    }
}