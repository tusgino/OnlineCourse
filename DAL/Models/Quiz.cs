﻿using System;
using System.Collections.Generic;

namespace DAL.Models
{
    public partial class Quiz
    {
        public Guid IdQuiz { get; set; }
        public Guid? IdLesson { get; set; }
        public string? Question { get; set; }
        public string? Image { get; set; }
        public string? Option1 { get; set; }
        public string? Option2 { get; set; }
        public string? Option3 { get; set; }
        public string? Option4 { get; set; }
        public int? Answer { get; set; }

        public virtual Lesson? IdLessonNavigation { get; set; }
    }
}
