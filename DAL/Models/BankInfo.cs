using System;
using System.Collections.Generic;

namespace DAL.Models
{
    public partial class BankInfo
    {
        public BankInfo()
        {
            Users = new HashSet<User>();
        }

        public Guid IdBankAccount { get; set; }
        public string? BankAccountNumber { get; set; }
        public string? AccountName { get; set; }
        public string? BankName { get; set; }

        public virtual ICollection<User> Users { get; set; }
    }
}
