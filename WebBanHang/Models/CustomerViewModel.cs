using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebBanHang.Models
{
    public class CustomerViewModel
    {
        public string CustommerName { get; set; }
        public string Phone { get; set; }
        public string Adress { get; set; }
        public string Email { get; set; }
        public int Payment { get; set; }
    }
}