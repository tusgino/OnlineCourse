using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Rsp.DTO

{
    public class UserDTO
    {
        public Guid IdUser { get; set; }
        public string? Email { get; set; }
        public string? Name { get; set; }
        public string? IdCard { get; set; }
        public string? PhoneNumber { get; set; }
        public string? Avatar { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public int? Status { get; set; }

        public AccountDTO? Account { get; set; }
        public int? IdTypeOfUser { get; set; }
    }
}

