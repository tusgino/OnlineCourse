using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Rsp.DTO
{
    public class CourseModel_Admin
    {
        public Guid ID { get; set; }
        public string? Name { get; set; } = string.Empty;
        public string? Category { get; set; } = string.Empty;
        public string? UploadUser { get; set; } = string.Empty;
        public string? DateUpload { get; set; } = string.Empty;
        public string? Price { get; set; } = string.Empty;
        public string? Discount { get; set; } = string.Empty;
        public string? FeePercent { get; set; } = string.Empty;
        public string? Status { get; set; } = string.Empty;
    }
}
