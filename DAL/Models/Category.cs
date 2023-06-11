using System;
using System.Collections.Generic;

namespace DAL.Models
{
    public partial class Category
    {
        public Category()
        {
            Courses = new HashSet<Course>();
        }

        public Guid IdCategory { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }

        public virtual ICollection<Course> Courses { get; set; }
    }
}
