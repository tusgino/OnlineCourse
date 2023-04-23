﻿using System;
using System.Collections.Generic;

namespace DAL.Models
{
    public partial class TradeDetail
    {
        public Guid IdTrade { get; set; }
        public Guid? IdUser { get; set; }
        public int? TypeOfTrade { get; set; }
        public int? TradeStatus { get; set; }
        public string? Balance { get; set; }
        public DateTime? DateOfTrade { get; set; }

        public virtual User? IdUserNavigation { get; set; }
    }
}
