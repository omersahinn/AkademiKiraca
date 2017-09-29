using System.Web.Mvc;


namespace AkademiKiraca.Areas.site
{
    public class siteAreaRegistration : AreaRegistration 
    {
        public override string AreaName
        {
            get
            {
                return "site";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context) 
        {



            context.MapRoute(
         "educatorList",
         "icegitimcilerimiz-{id}/",
         new { areas = "site", controller = "Home", action = "educatorList", id = UrlParameter.Optional }
     );

            context.MapRoute(
             "News",
             "haberler/",
             new { areas = "site", controller = "Home", action = "News", id = UrlParameter.Optional }
         );

            context.MapRoute(
              "subCategory",
              "kategori-{id}/",
              new { areas = "site", controller = "Home", action = "subCategory", id = UrlParameter.Optional }
          );



            context.MapRoute(
                "site_default",
                "anasayfa/{controller}/{action}/{id}",
                new {controller = "Home", action = "Index", id = UrlParameter.Optional },
                new[] {"AkademiKiraca.Areas.site.Controllers"}
            );
         

        

    }
       
    }
}