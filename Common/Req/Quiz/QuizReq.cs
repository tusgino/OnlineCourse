﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Req.Quiz
{
    public class QuizReq
    {
        public Guid? IdLesson { get; set; }
        public string? Question { get; set; }
        public string? Image { get; set; }
        public string? Option1 { get; set; }
        public string? Option2 { get; set; }
        public string? Option3 { get; set; }
        public string? Option4 { get; set; }
        public int? Answer { get; set; }
    }
}
