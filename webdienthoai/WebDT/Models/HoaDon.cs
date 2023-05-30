using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebDT.Models
{
    public class HoaDon
    {
        public int gioHangId { get; set; }
        public string TenKhachHang { get; set; }
        public string img { get; set; }
        public string name { get; set; }
        public int? SoLuong { get; set; }
        public double? Tien { get; set; }
        public DateTime? NgayTao { get; set; }
        public string DiaChi { get; set; }
        public string SDTKhachHang { get; set; }
        public string Email { get; set; }

        public bool? status { get; set; }
    }
}