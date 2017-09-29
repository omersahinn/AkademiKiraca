using System.Web.Mvc;

namespace AkademiKiraca.Areas.admin
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

 
            context.MapRoute(
  "readMessage",
  "mesajlar-{id}/",
  new { areas = "admin", controller = "Home", action = "readMessage", id = UrlParameter.Optional }
  );

            context.MapRoute(
  "createAdmin",
  "adminekle/",
  new { areas = "admin", controller = "Home", action = "createAdmin", id = UrlParameter.Optional }
  );




            context.MapRoute(
      "settings",
      "ayarlar/",
      new { areas = "admin", controller = "Home", action = "settings", id = UrlParameter.Optional }
      );

            context.MapRoute(
          "editNews",
          "haberdüzenle-{id}/",
          new { areas = "admin", controller = "Home", action = "editNews", id = UrlParameter.Optional }
          );

            context.MapRoute(
          "resimyükle",
          "resimyükle-{id}/",
          new { areas = "admin", controller = "Home", action = "uploadPicture", id = UrlParameter.Optional }
          );

            context.MapRoute(
          "sosyalmedya",
          "sosyalmedya/",
          new { areas = "admin", controller = "Home", action = "socialMedya", id = UrlParameter.Optional }
          );

            context.MapRoute(
                      "academy",
                      "benimakademim/",
                      new { areas = "admin", controller = "Home", action = "Academy", id = UrlParameter.Optional }
                      );

            context.MapRoute(
           "messages",
           "kullanıcı-mesajları/",
           new { areas = "admin", controller = "Home", action = "seeAll", id = UrlParameter.Optional }
           );

            context.MapRoute(
            "haberler",
            "haberlerdüzenle/",
            new { areas = "admin", controller = "Home", action = "newsAdmin", id = UrlParameter.Optional }
            );

            context.MapRoute(
             "egitimciler",
             "icegitimcilistesi-{id}/",
             new { areas = "admin", controller = "Home", action = "educatorList", id = UrlParameter.Optional }
         );

            context.MapRoute(
             "getSubCategories",
             "altkategori-{id}/",
             new { areas = "admin", controller = "Home", action = "getSubCategories", id = UrlParameter.Optional }
         );

            context.MapRoute(
                "admin_default",
                "admin/{controller}/{action}/{id}",
                new {controller="Home", action = "Index", id = UrlParameter.Optional },
                new[] { "AkademiKiraca.Areas.admin.Controllers" }

            );
        }
    }
}