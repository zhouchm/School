using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Schoolxm
{
    public class User
    {
        public int Id { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public string RealName { get; set; }
        public string Gender { get; set; }
        public string Email { get; set; }
    }
}