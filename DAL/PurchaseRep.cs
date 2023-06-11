using DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL
{
    public class PurchaseRep
    {
        public Guid PurchaseACourse(Guid idCourse, string? email, int typeOfPurchase = 3)
        {
            if(typeOfPurchase == 3)
            {
                using(WebsiteKhoaHocOnline_V4Context context = new WebsiteKhoaHocOnline_V4Context())
                {
                    if (context.Purchases.SingleOrDefault(p => p.IdCourse == idCourse && p.IdUserNavigation.Email == email) != null)
                    {
                        return Guid.Empty;
                    }

                    var user = context.Users.SingleOrDefault(u => u.Email == email);
                    var course = context.Courses.SingleOrDefault(c => c.IdCourse == idCourse);
                    if (user == null || course == null)
                    {
                        return Guid.Empty;
                    }
                    else
                    {
                        
                        var tradeDetail = new TradeDetail
                        {
                            Balance = Convert.ToString(course.Price * ((100 - course.Discount) / 100)),
                            DateOfTrade = DateTime.Now,
                            IdTrade = Guid.NewGuid(),
                            IdUser = user.IdUser,
                            Purchases = user.Purchases,
                            TradeStatus = 0,
                            TypeOfTrade = 0,
                        };
                        context.TradeDetails.Add(tradeDetail);
                        context.Purchases.Add(new Purchase
                        {
                            DateOfPurchase = DateTime.Now,
                            IdTrade = tradeDetail.IdTrade,
                            IdUser = user.IdUser,
                            IdCourse = course.IdCourse,
                        });
                        context.SaveChanges();

                        return tradeDetail.IdTrade;
                    }
                }
            }
            return Guid.Empty;
        }
    }
}
