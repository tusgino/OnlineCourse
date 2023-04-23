using System;
using System.Collections.Generic;

namespace DAL.Models
{
    public partial class User
    {
        public User()
        {
            Courses = new HashSet<Course>();
            Degrees = new HashSet<Degree>();
            TradeDetails = new HashSet<TradeDetail>();
        }

        public Guid IdUser { get; set; }
        public Guid? IdAccount { get; set; }
        public Guid? IdBankAccount { get; set; }
        public string? Email { get; set; }
        public string? Name { get; set; }
        public string? IdCard { get; set; }
        public string? PhoneNumber { get; set; }
        public string? Avatar { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public int? Status { get; set; }
        public int? IdTypeOfUser { get; set; }

        public virtual Account? IdAccountNavigation { get; set; }
        public virtual BankInfo? IdBankAccountNavigation { get; set; }
        public virtual TypeOfUser? IdTypeOfUserNavigation { get; set; }
        public virtual ICollection<Course> Courses { get; set; }
        public virtual ICollection<Degree> Degrees { get; set; }
        public virtual ICollection<TradeDetail> TradeDetails { get; set; }
    }
}
