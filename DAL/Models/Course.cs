using System;
using System.Collections.Generic;

namespace DAL.Models
{
    public partial class Course
    {
        public Course()
        {
            Chapters = new HashSet<Chapter>();
        }

        public Guid IdCourse { get; set; }
        public Guid? IdUser { get; set; }
        public Guid? IdCategory { get; set; }
        public DateTime? DateOfUpload { get; set; }
        public string? CourseName { get; set; }
        public long? Price { get; set; }
        public double? FeePercent { get; set; }
        public string? Description { get; set; }
        public string? VideoPreview { get; set; }
        public string? Thumbnail { get; set; }
        public double? Discount { get; set; }
        public int? Status { get; set; }

        public virtual Category? IdCategoryNavigation { get; set; }
        public virtual User? IdUserNavigation { get; set; }
        public virtual ICollection<Chapter> Chapters { get; set; }
    }
}
