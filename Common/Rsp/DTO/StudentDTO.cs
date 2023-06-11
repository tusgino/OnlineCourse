using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Rsp.DTO
{
    public class StudentDTO
    {
        public string Name { get; set; } = string.Empty;
        public int NumOfPurchasedCourse { get; set; } = 0;
        public int NumOfFinishedCourse { get; set; } = 0;
    }
}
