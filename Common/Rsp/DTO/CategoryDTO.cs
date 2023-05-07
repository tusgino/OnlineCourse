using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Rsp.DTO
{
    public class CategoryDTO
    {
        public Guid IdCategory { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
    }
}
