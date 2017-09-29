using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AkademiKiraca.Models;
using System.Text;
using System.Net;

namespace AkademiKiraca.Areas.site.Controllers
{
    public class HomeController : Controller
    {
        DbDataContext db = new DbDataContext();

        public string GetErrorMessage(Exception ex)
        {
            StringBuilder errorMessage = new StringBuilder(200);

            errorMessage.AppendFormat("<div class=\"validation-summary-errors\" title=\"Server Error\">{0}</div>", ex.GetBaseException().Message);
            Response.StatusCode = (int)HttpStatusCode.InternalServerError;

            return errorMessage.ToString();
        }
        public ActionResult Index()
        {
            var model = db.Pictures.Where(a => a.Position == 2).ToList();
            return View(model);
        }

        public ActionResult subCategory(int id)
        {
            try
            {
                var temp = from s in db.SubCategories.ToList()
                           join c in db.Categories.ToList() on s.Categoryid equals c.id
                           join p in db.Pictures.ToList() on s.id equals p.SubCategoryid
                           where s.Categoryid == c.id && s.id == p.SubCategoryid && s.id == id
                           select new subCategoryModel
                           {
                               id = s.id,
                               subCategoryName = s.SubcategoryName,
                               description = s.Description,
                               position = p.Position,
                               picturePath = p.Path,
                               categoryName = c.CategoryName,
                               updatedDate = s.UpdatedDate
                           };
                var sub = db.SubCategories.Where(a => a.id == id).FirstOrDefault();
                ViewBag.catId = sub.id;
                return View(temp);
            }
            catch (Exception e)
            {

                return Content(GetErrorMessage(e));
            }
        }
        public PartialViewResult sentCategory()
        {

            ViewBag.academy = db.Categories.Where(a => a.id == 6).FirstOrDefault();
            var model = db.SubCategories.ToList();
            return PartialView("~/Areas/site/Views/Shared/MyPartial.cshtml", model);

        }
        public PartialViewResult Footer()
        {
            var models = db.SocialMedyas.ToList();
            return PartialView("~/Areas/site/Views/Shared/Footer.cshtml", models);
        }
        public ActionResult educatorList(int id)
        {
            try
            {

                var model = db.Educators.OrderByDescending(a=>a.id).ToList();

                Picture picture = new Picture();
                picture = db.Pictures.Where(a => a.SubCategoryid == id).FirstOrDefault();


                ViewBag.picture = picture.Path;

                ViewBag.id = id;

                return View(model);
            }
            catch (Exception e)
            {

                return Content(GetErrorMessage(e));
            }
        }

        public ActionResult News()
        {


            try
            {
                var models = db.News.ToList();


                List<newsModel> haberler = new List<newsModel>();


                foreach (var item in models)
                {

                    newsModel temp = new newsModel();

                    temp.id = item.id;
                    temp.shortTitle = item.ShortTitle;
                    temp.title = item.Title;
                    temp.contentNews = item.ContentNews;
                    temp.newsPicture = item.Pictures.Where(b => b.Newsid == item.id).ToList();

                    haberler.Add(temp);

                    temp = null;
                }

                //ilk haberin id si
                var ilkHaber = db.News.OrderByDescending(a=>a.id).First();
                ViewBag.ilkHaber = ilkHaber.id;



                return View(haberler.OrderByDescending(a=>a.id));
            }
            catch (Exception e)
            {

                return Content(GetErrorMessage(e));
            }
        }


        public string sentNews(int id)
        {



            newsModel haber = new newsModel();

            New model = new New();

            model = db.News.Where(a => a.id == id).FirstOrDefault();

            List<Picture> picture = new List<Picture>();
            picture = db.Pictures.Where(a => a.Newsid == id).ToList();

            haber.id = model.id;
            haber.shortTitle = model.ShortTitle;
            haber.title = model.Title;
            haber.contentNews = model.ContentNews;
            haber.newsPicture = picture;

            string dr;

            dr = "<p> <span class='boldbaslik'>" + haber.title + "</span><br><br>" + haber.contentNews + "</p><div class='popup-gallery'>";
            foreach (var item in haber.newsPicture)
            {
                dr = dr + "<a href=/Areas/site/Content/img/" + item.Path + " class='hoverZoomLink'><img src=/images/100x100/" + item.Path + "></a>";
            }
            dr = dr + "</div>";
            return (dr.ToString());
        }
        public PartialViewResult Contact()
        {

            return PartialView();
        }

        [HttpPost]
        public ActionResult userMessage(UserMessageModel user)
        {
            try
            {
                UserMessage u = new UserMessage();

                if (ModelState.IsValid)
                {
                    u.NameSurname = user.nameSurname;
                    u.Message = user.message;
                    u.Phone = user.phone;
                    u.Email = user.email;
                    u.Date = DateTime.Now;
                    u.IsReply = false;
                    u.IsRead = false;
                    db.UserMessages.InsertOnSubmit(u);
                    db.SubmitChanges();

                    TempData["message"] = "Mesajınız Başarılı Bir Şekilde Gönderildi";

                    return RedirectToAction("subCategory", new { id = 4 });
                }
            }
            catch (Exception e)
            {

                return Content(GetErrorMessage(e));
            }

            return RedirectToAction("subCategory", new { id = 4 });
        }
    }
}