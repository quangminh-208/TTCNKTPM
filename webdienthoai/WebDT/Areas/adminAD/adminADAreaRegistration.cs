using System.Web.Mvc;

namespace WebDT.Areas.adminAD
{
    public class adminADAreaRegistration : AreaRegistration 
    {
        public override string AreaName 
        {
            get 
            {
                return "adminAD";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context) 
        {
            context.MapRoute(
                "adminAD_default",
                "adminAD/{controller}/{action}/{id}",
                new { action = "Index", Controller="Homes" , id = UrlParameter.Optional }
            );
        }
    }
}