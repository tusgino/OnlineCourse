using System;
using System.Collections.Generic;

namespace DAL.Models
{
    public partial class Study
    {
        public Guid IdUser { get; set; }
        public Guid? IdLesson { get; set; }
        public int? Status { get; set; }

        public virtual Lesson? IdLessonNavigation { get; set; }
        public virtual User IdUserNavigation { get; set; } = null!;
    }
}
