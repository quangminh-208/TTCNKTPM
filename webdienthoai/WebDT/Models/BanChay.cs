using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebDT.Models
{
    public class BanChay
    {
        public int Id { get; set; }
        public string Ten_sp { get; set; }
        public string HinhAnh { get; set; }
        public string meta { get; set; }
        public double? Price { get; set; }
        public double? NewPrice { get; set; }
        public int? Tong_SL { get; set; }
    }
}