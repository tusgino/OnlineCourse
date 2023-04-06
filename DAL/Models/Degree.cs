using System;
using System.Collections.Generic;

namespace DAL.Models
{
    public partial class Degree
    {
        public Guid IdDegree { get; set; }
        public Guid? IdUser { get; set; }
        public string? Image { get; set; }
        public string? Description { get; set; }

        public virtual User? IdUserNavigation { get; set; }
    }
}
