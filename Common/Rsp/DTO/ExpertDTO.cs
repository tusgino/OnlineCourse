using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Rsp.DTO
{
    public class ExpertDTO : UserDTO
    {
        public BankInfoDTO? BankInfo { get; set; }
        public List<DegreeDTO>? Degrees { get; set; }
    }
}
