using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Rsp.DTO
{
    public class CourseDTO
    {
        public Guid IdCourse { get; set; }
        public string? CourseName { get; set; }
        public List<ChapterDTO> Chapters { get; set; }
    }
}
