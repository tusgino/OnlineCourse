using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Req.Account
{
    public class RegisterReq
    {
        public string Name { get; set; } = string.Empty;
        public string Username { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public string RePassword { get; set; } = string.Empty;
        public int TypeOfUser { get; set; }

    }
}
