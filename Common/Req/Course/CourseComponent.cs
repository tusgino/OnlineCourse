﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Req.Course
{
    public class CourseComponent
    {
        public Guid Id { get; set; }
        public string? Thumbnail { get; set; } = string.Empty;
        public string? Title { get; set; } = string.Empty;

        public string? NameUser { get; set; } = string.Empty;

        public string? AvatarUser { get; set; } = string.Empty;
    }
}
