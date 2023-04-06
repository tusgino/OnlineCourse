using System;
using System.Collections.Generic;

namespace DAL.Models
{
    public partial class TypeOfUser
    {
        public TypeOfUser()
        {
            Users = new HashSet<User>();
        }

        public int IdTypeOfUser { get; set; }
        public string? TypeOfUserName { get; set; }

        public virtual ICollection<User> Users { get; set; }
    }
}
