using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace WebDT.Models
{
    public class RegisterModel
    {
        [Key]
        public long id { get; set; }

        [Display(Name = "Tên đăng nhập")]
        [StringLength(100, MinimumLength = 2, ErrorMessage = "Tên ít nhất 2 ký tự ")]
        [Required(ErrorMessage = "Yêu cầu nhập tên đăng nhập")]
        public string username { get; set; }


        [Display(Name = "Mật khẩu")]
        [StringLength(20, MinimumLength = 6, ErrorMessage = "Độ dài mật khẩu ít nhất 6 kí tự :)")]
        [Required(ErrorMessage = "Yêu cầu nhập mật khẩu")]
       
        public string password { get; set; }
        [Display(Name = "Xác nhận mật khẩu")]
        [Compare("password", ErrorMessage = "Xác nhận mật khẩu không đúng =.=")]
        
        public string confirmpassword { get; set; }
        [Display(Name = "Họ tên")]
        [Required(ErrorMessage = "Yêu cầu nhập họ tên")]
        public string name { get; set; }
        [Display(Name = "Địa chỉ")]
        public string address { get; set; }
        [Display(Name = "Email")]
        [Required(ErrorMessage = "Yêu cầu nhập email")]

        public string email { get; set; }

        [Display(Name = "Điện thoại")]
       
        public string phone { get; set; }

    }
}