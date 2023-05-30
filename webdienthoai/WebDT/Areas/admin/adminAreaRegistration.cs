using System.Web.Mvc;

namespace WebDT.Areas.admin
{
    public class adminAreaRegistration : AreaRegistration 
    {
        public override string AreaName 
        {
            get 
            {
                return "admin";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context) 
        {
            //Chi tiết hóa đơn
           // context.MapRoute(
           //    "detail order",
           //    "admin/chi-tiet-hoa-don/{gioHangId}",
           //    new { Controller = "HoaDon", action = "Index", id = UrlParameter.Optional }
           //);


            //Quản lý user
            context.MapRoute(
               "user manager",
               "admin/quan-ly-tai-khoan",
               new { Controller = "DangNhaps", action = "Index", id = UrlParameter.Optional }
           );

            context.MapRoute(
                "admin_default",
                "admin/{controller}/{action}/{id}",
                new { Controller = "Defaults", action="Index", id = UrlParameter.Optional }
            );
        }
    }
}