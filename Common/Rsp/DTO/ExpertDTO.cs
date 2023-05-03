using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Rsp.DTO
{
    public class ExpertDTO
    {
        public Guid ID { get; set; } = new Guid();
        public string Name { get; set; } = string.Empty;
        public int NumOfUploadedCourse { get; set; } = 0;
        public List<long> CurrentYearRevenue { get; set; } = new List<long>();
    }
}
