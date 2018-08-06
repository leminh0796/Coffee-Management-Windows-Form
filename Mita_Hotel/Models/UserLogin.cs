using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Mita_Coffee.Models
{
    class UserLogin
    {
        public string Username { get; set; }
        public string MD5Password { get; set; }
        public string Fullname { get; set; }
        public DateTime LastLogin { get; set; }
        public string RoleID { get; set; }
        public string Email { get; set; }
        public int Phone { get; set; }
    }
}
