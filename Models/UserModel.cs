using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Doanphanmem.Models
{
    public class UserModel
    {
        public int MaKH { get; set; }
        public string TenKH { get; set; }
        public string Roleuser { get; set; }
        // Các trường thông tin khác của người dùng
        public string TK{ get; set; }
        public string Pass { get; set; }
    }

}