using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebDT.Areas.admin.Models
{
    public class TotalSale
    {
        public int month { get; set; }
        public double? total { get; set; }
        public string totalPrice { get; set; }
    }
}