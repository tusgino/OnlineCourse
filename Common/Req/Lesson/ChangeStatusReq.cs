using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Req.Lesson
{
    public class ChangeStatusReq
    {
        public Guid IdUser { get; set; }
        public Guid IdLesson { get; set; }
    }
}
