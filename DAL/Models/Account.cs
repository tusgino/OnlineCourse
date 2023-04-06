using System;
using System.Collections.Generic;

namespace DAL.Models
{
    public partial class Account
    {
        public Account()
        {
            Users = new HashSet<User>();
        }

        public Guid IdAccount { get; set; }
        public string? Username { get; set; }
        public string? Password { get; set; }
        public DateTime? DateCreate { get; set; }

        public virtual ICollection<User> Users { get; set; }
    }
}
