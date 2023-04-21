using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Rsp.DTO
{
    public class UserModel_Admin
    {
        public Guid ID { get; set; }
        public string? Name { get; set; } = string.Empty;
        public string? TypeOfUser { get; set; } = string.Empty;
        public string? DateOfBirth { get; set; } = string.Empty;
        public string? PhoneNumber { get; set; } = string.Empty;
        public string? IDCard { get; set; } = string.Empty;
        public string? Email { get; set; } = string.Empty;
        public string? DateCreate { get; set; } = string.Empty;
        public string? Status { get; set; } = string.Empty;
    }
}
