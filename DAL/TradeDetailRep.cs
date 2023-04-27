using Common.DAL;
using DAL.Models;
using Microsoft.AspNetCore.JsonPatch;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL
{
    public class TradeDetailRep : GenericRep<WebsiteKhoaHocOnline_V4Context, TradeDetail>
    {
        public List<TradeDetail> GetAllTradeDetailsByFiltering(bool? _is_rent, bool? _is_purchase, bool? _is_success, bool? _is_pending, bool? _is_failed, DateTime? _start_date, DateTime? _end_date, string? _start_balance, string? _end_balance)
        {
            using (WebsiteKhoaHocOnline_V4Context context = new WebsiteKhoaHocOnline_V4Context())
            {

                List<int> typesOfTrade = new List<int>();
                if (_is_purchase == true) typesOfTrade.Add(0);
                if (_is_rent == true) typesOfTrade.Add(1);

                List<int> tradeStatus = new List<int>();
                if (_is_success == true) tradeStatus.Add(1);
                if (_is_pending == true) tradeStatus.Add(0);
                if (_is_failed == true) tradeStatus.Add(-1);

                return context.TradeDetails.Where(trade => typesOfTrade.Contains(trade.TypeOfTrade ?? -1) &&
                    tradeStatus.Contains(trade.TradeStatus ?? -2) &&
                    (trade.DateOfTrade >= _start_date && trade.DateOfTrade <= _end_date) &&
                    (String.Compare(trade.Balance, _start_balance) >= 0 && String.Compare(trade.Balance, _end_balance) <= 0)
                ).ToList();
            }
        }
        public bool UpdateTrade(Guid id, JsonPatchDocument newTrade)
        {
            using (WebsiteKhoaHocOnline_V4Context context = new WebsiteKhoaHocOnline_V4Context())
            {
                var trade = context.TradeDetails.FirstOrDefault(trade => trade.IdTrade == id);

                if (trade != null)
                {
                    newTrade.ApplyTo(trade);
                    context.SaveChanges();
                    return true;
                }
                return false;
            }
        }
        public List<object> GetSystemRevenue()
        {
            using (WebsiteKhoaHocOnline_V4Context context = new WebsiteKhoaHocOnline_V4Context())
            {
                return context.TradeDetails.Where(trade => trade.TypeOfTrade == 1)
                                           .OrderBy(trade => trade.DateOfTrade)
                                           .GroupBy(trade => trade.DateOfTrade.Value.Month)
                                           .Select(group => new { Month = group.Key, Revenue = group.Sum(trade => Convert.ToInt64(trade.Balance)) })
                                           .ToList<object>();






            }
        }
    }
}
