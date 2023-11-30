using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace WebBanHang.Models
{
    [Table("tb_Category")]
    public class CommonAbstract
    {
        public DateTime CreatedDate { get; set; }
        public string CreatedBy { get; set; }
        public  DateTime ModifiedDate { get; set; }
        public string ModifiedBy { get; set; }
    }
}