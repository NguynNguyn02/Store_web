using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace WebBanHang.Models
{
    public class OrderViewModel
    {
        public string CustomerName { get; set; }
        [Required(ErrorMessage = "Số điện thoại không để trống!")]
        public string Phone { get; set; }
        [Required(ErrorMessage = "Địa chỉ không để trống!")]
        public string Address { get; set; }
        public int TypePayment { get; set; }
    }
}