using System;
using System.Collections.Generic;

namespace DAL.Models
{
    public partial class Purchase
    {
        public Guid IdUser { get; set; }
        public Guid IdCourse { get; set; }
        public Guid? IdTrade { get; set; }
        public DateTime? DateOfPurchase { get; set; }

        public virtual Course IdCourseNavigation { get; set; } = null!;
        public virtual TradeDetail? IdTradeNavigation { get; set; }
        public virtual User IdUserNavigation { get; set; } = null!;
    }
}
