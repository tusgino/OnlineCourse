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
                    var user = context.Users.SingleOrDefault(u => u.Email == email);
                    var course = context.Courses.SingleOrDefault(c => c.IdCourse == idCourse);
                    if(user == null || course == null)
                    {
                        return Guid.Empty;
                    }
                    else
                    {
                        var tradeDetail = new TradeDetail
                        {
                            Balance = Convert.ToString(course.Price * course.Discount / 100),
                            DateOfTrade = DateTime.Now,
                            IdTrade = Guid.NewGuid(),
                            IdUser = user.IdUser,
                            Purchases = user.Purchases,
                            TradeStatus = 0,
                            TypeOfTrade = typeOfPurchase,
                        };
                        context.TradeDetails.Add(tradeDetail);
                        context.SaveChanges();

                        return tradeDetail.IdTrade;
                    }
                }
            }
            return Guid.Empty;
        }
    }
}
