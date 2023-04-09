using System;
using System.Collections.Generic;

namespace DAL.Models
{
    public partial class Chapter
    {
        public Chapter()
        {
            Lessons = new HashSet<Lesson>();
        }

        public Guid IdChapter { get; set; }
        public string? Name { get; set; }
        public int? Index { get; set; }
        public Guid? IdCourse { get; set; }

        public virtual Course? IdCourseNavigation { get; set; }
        public virtual ICollection<Lesson> Lessons { get; set; }
    }
}
