﻿using System;
using System.Collections.Generic;

namespace DAL.Models
{
    public partial class Lesson
    {
        public Lesson()
        {
            Quizzes = new HashSet<Quiz>();
            Studies = new HashSet<Study>();
        }

        public Guid IdLesson { get; set; }
        public Guid? IdChapter { get; set; }
        public string? Description { get; set; }
        public int? Index { get; set; }
        public string? Video { get; set; }
        public double? Duration { get; set; }
        public string? Title { get; set; }

        public virtual Chapter? IdChapterNavigation { get; set; }
        public virtual ICollection<Quiz> Quizzes { get; set; }
        public virtual ICollection<Study> Studies { get; set; }
    }
}
