using System;
using System.Collections.Generic;

namespace DAL.Models
{
    public partial class Rate
    {
        public Guid IdUser { get; set; }
        public Guid? IdCourse { get; set; }
        public int? Rate1 { get; set; }

        public virtual Course? IdCourseNavigation { get; set; }
        public virtual User IdUserNavigation { get; set; } = null!;
    }
}
