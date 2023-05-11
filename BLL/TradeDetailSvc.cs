using Common.BLL;
using Common.Req.Course;
using Common.Req.TradeDetail;
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
    public class TradeDetailSvc : GenericSvc<TradeDetailRep, TradeDetail>
    {
        private readonly TradeDetailRep _tradeDetailRep = new TradeDetailRep();
        public SingleRsp GetAllTradeDetailsByFiltering(TradeDetailFilteringReq tradeDetailFilteringReq, CoursesPaginationReq coursesPaginationReq)
        {
            if (tradeDetailFilteringReq.start_date == null) tradeDetailFilteringReq.start_date = new DateTime(1754, 1, 1);
            if (tradeDetailFilteringReq.end_date == null) tradeDetailFilteringReq.end_date = new DateTime(9999, 1, 1);

            if (tradeDetailFilteringReq.start_balance == null) tradeDetailFilteringReq.start_balance = 0;
            if (tradeDetailFilteringReq.end_balance == null) tradeDetailFilteringReq.end_balance = 9999999999999999;


            var tradeDetails = _tradeDetailRep.GetAllTradeDetailsByFiltering(tradeDetailFilteringReq.is_rent, 
                                                                            tradeDetailFilteringReq.is_purchase, 
                                                                            tradeDetailFilteringReq.is_success, 
                                                                            tradeDetailFilteringReq.is_pending, 
                                                                            tradeDetailFilteringReq.is_failed, 
                                                                            tradeDetailFilteringReq.start_date, 
                                                                            tradeDetailFilteringReq.end_date, 
                                                                            tradeDetailFilteringReq.start_balance, 
                                                                            tradeDetailFilteringReq.end_balance);

            int limit = 10;
            int offset = (coursesPaginationReq.Page - 1) * limit;
            int total = tradeDetails.Count;
            int totalPages = (total % limit) == 0 ? (int)(total /limit) :
                (int)(1 + (total / limit));

            var data = tradeDetails.Skip(offset).Take(limit).ToList();

            object res = new
            {
                _data = data,
                _totalRows = total,
            };

            var rsp = new SingleRsp();

            if(data == null)
            {
                rsp.SetError("Not found trade");
            }
            else
            {
                rsp.Data = res;
            }

            return rsp;
        }
        public SingleRsp UpdateTrade(Guid id, JsonPatchDocument patchDoc)
        {
            var rsp = new SingleRsp();

            if (!_tradeDetailRep.UpdateTrade(id, patchDoc))
            {
                rsp.SetError("Update failed");
            }

            return rsp;
        }
        public SingleRsp GetSystemRevenue()
        {
            var data = _tradeDetailRep.GetSystemRevenue();

            var rsp = new SingleRsp();
            if(data == null)
            {
                rsp.SetError("Renevue not updated");
            }
            else
            {
                rsp.Data = data;
            }

            return rsp;
        }
        public SingleRsp GetTradeByID(Guid IdTrade)
        {
            var data = _tradeDetailRep.GetTradeByID(IdTrade);

            var rsp = new SingleRsp();

            if(data == null)
            {
                rsp.SetError("Not found trade");
            }
            else
            {
                rsp.Data = data;
            }
            return rsp;
        }
    }
}
