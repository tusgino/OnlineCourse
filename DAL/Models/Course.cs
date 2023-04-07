using System;
using System.Collections.Generic;

namespace DAL.Models
{
    public partial class Course
    {
        public Guid IdCourse { get; set; }
        public Guid? IdUser { get; set; }
        public Guid? IdCategory { get; set; }
        public DateTime? DateOfUpload { get; set; }
        public string? CourseName { get; set; }
        public long? Price { get; set; }
        public double? SalePercent { get; set; }
        public string? Description { get; set; }
        public string? VideoPreview { get; set; }
        public string? Thumbnail { get; set; }
        public double? Discount { get; set; }
        public int? Status { get; set; }

        public virtual User? IdUserNavigation { get; set; }
    }
}
