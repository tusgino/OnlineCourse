using Common.DAL;
using Common.Rsp.DTO;
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
        public List<TradeDetailDTO> GetAllTradeDetailsByFiltering(bool? _is_rent, bool? _is_purchase, bool? _is_success, bool? _is_pending, bool? _is_failed, DateTime? _start_date, DateTime? _end_date, long? _start_balance, long? _end_balance)
        {
            using (WebsiteKhoaHocOnline_V4Context context = new WebsiteKhoaHocOnline_V4Context())
            {
                List<TradeDetailDTO> list = new List<TradeDetailDTO>();

                List<int> typesOfTrade = new List<int>();
                if (_is_purchase == true) typesOfTrade.Add(0);
                if (_is_rent == true) typesOfTrade.Add(1);

                List<int> tradeStatus = new List<int>();
                if (_is_success == true) tradeStatus.Add(1);
                if (_is_pending == true) tradeStatus.Add(0);
                if (_is_failed == true) tradeStatus.Add(-1);

                foreach(var tradeDetail in context.TradeDetails.Where(trade => typesOfTrade.Contains(trade.TypeOfTrade ?? -1) &&
                    tradeStatus.Contains(trade.TradeStatus ?? -2) &&
                    (trade.DateOfTrade >= _start_date && trade.DateOfTrade <= _end_date) &&
                    (Convert.ToInt64(trade.Balance) >= Convert.ToInt64(_start_balance) && Convert.ToInt64(trade.Balance) <= Convert.ToInt64(_end_balance))
                ).ToList())
                {
                    var user = context.Users.SingleOrDefault(u => u.IdUser == tradeDetail.IdUser);
                    if(tradeDetail.TypeOfTrade == 0)
                    {
                        var purchase = context.Purchases.SingleOrDefault(p => p.IdTrade == tradeDetail.IdTrade);
                        context.Entry(purchase).Reference(c => c.IdCourseNavigation).Load();
                        list.Add(new TradeDetailDTO
                        {
                            IdTrade = tradeDetail.IdTrade,
                            Balance = tradeDetail.Balance,
                            DateOfTrade = tradeDetail.DateOfTrade,
                            RequiredBalance = Convert.ToString(purchase.IdCourseNavigation.Price * purchase.IdCourseNavigation.Discount / 100),
                            TradeStatus = tradeDetail.TradeStatus,
                            TypeOfTrade = tradeDetail.TypeOfTrade ?? -1,
                            User = new UserDTO
                            {
                                Name = user.Name
                            }
                        });
                    }
                    else
                    {
                        double? requiredBalance = 0;

                        var courses = context.Courses.Where(course => course.IdUser == tradeDetail.IdUser).ToList();
                        foreach (Course course in courses)
                        {
                            var purchases = context.Purchases.Where(purchase => purchase.IdCourse == course.IdCourse  && purchase.DateOfPurchase.Value.Month == DateTime.Now.Month - 1).ToList();
                            foreach(Purchase purchase in purchases)
                            {
                                var trade = context.TradeDetails.FirstOrDefault(trade => trade.IdTrade == purchase.IdTrade);

                                requiredBalance += Convert.ToInt64(trade.Balance) * course.FeePercent  / 100;
                            }
                        }
                        list.Add(new TradeDetailDTO
                        {
                            IdTrade = tradeDetail.IdTrade,
                            Balance = tradeDetail.Balance,
                            DateOfTrade = tradeDetail.DateOfTrade,
                            RequiredBalance = Convert.ToString(requiredBalance),
                            TradeStatus = tradeDetail.TradeStatus,
                            TypeOfTrade = tradeDetail.TypeOfTrade ?? -1,
                            User = new UserDTO
                            {
                                Name = user.Name
                            }
                        });
                    }
                }

                return list;
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
