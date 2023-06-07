using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Req.User
{
    public class ExpertRegisterReq
    {
        public string? name { get; set; }
        public DateTime? date_create_from { get; set; }
        public DateTime? date_create_to { get; set; }
        public int Page { get; set; }
        public void ValidateData()
        {
            name ??= String.Empty;
            date_create_from ??= new DateTime(1, 1, 1);
            date_create_to ??= new DateTime(9999, 1, 1);
        }
    }
}
