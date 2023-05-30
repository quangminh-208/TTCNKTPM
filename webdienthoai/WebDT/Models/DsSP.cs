using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebDT.Models
{
    public class DsSP
    {   
        public String TaiKhoan { get; set; }
        public String  Tensp { get; set; }
        public String Hinh { get; set; }
        public int? SoLuong { get; set; }
        public double? Tien { get; set; }
        public DateTime? NgayDat { get; set; }
    }
}