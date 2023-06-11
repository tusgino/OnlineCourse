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
        public long? Price { get; set; }
        public double? FeePercent { get; set; }
        public string? Description { get; set; }
        public string? VideoPreview { get; set; }
        public string? Thumbnail { get; set; }
        public double? Discount { get; set; }
        public int? Status { get; set; }

        public CategoryDTO Category { get; set; }
        public UserDTO User { get; set; } = new UserDTO();
        public List<ChapterDTO> Chapters { get; set; } = new List<ChapterDTO>();
    }
}
