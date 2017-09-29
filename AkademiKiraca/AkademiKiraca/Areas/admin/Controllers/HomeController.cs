using AkademiKiraca.HtmlHelper;
using AkademiKiraca.Models;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace AkademiKiraca.Areas.admin.Controllers
{

    public class FileUploadJsonResult : JsonResult
    {

        public override void ExecuteResult(ControllerContext context)
        {
            this.ContentType = "text/html";
            base.ExecuteResult(context);

        }
    }

    public static class FileResize
    {
        public static void ImageResize(int width, int height, string srcImagePath, string trgImagePath)
        {
            Bitmap myBitmap = new Bitmap(HttpContext.Current.Server.MapPath(srcImagePath));//Burda alacağımız resmin yolunu belirtiyor
            Bitmap image = new Bitmap(myBitmap, width, height);

            image.Save(HttpContext.Current.Server.MapPath(trgImagePath));

            image.Dispose();
            myBitmap.Dispose();
        }
    }

    public class HomeController : Controller
    {

        DbDataContext db = new DbDataContext();
        // GET: admin/Admin

        public string GetErrorMessage(Exception ex)
        {
            StringBuilder errorMessage = new StringBuilder(200);

            errorMessage.AppendFormat("<div class=\"validation-summary-errors\" title=\"Server Error\">{0}</div>", ex.GetBaseException().Message);
            Response.StatusCode = (int)HttpStatusCode.InternalServerError;

            return errorMessage.ToString();
        }
        [AdminSessionControl]
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Login()
        {
            adminModel admin = new adminModel();
            return View(admin);
        }
        [HttpPost]
        public ActionResult Login(adminModel admin)
        {

            if (!ModelState.IsValidField("eMail") || !ModelState.IsValidField("password"))
            {
                return View(admin);
            }



            try
            {
                admin.password = FormsAuthentication.HashPasswordForStoringInConfigFile(admin.password, "md5");

                Admin ad = db.Admins.Where(a => a.Email == admin.eMail).Where(a => a.Password == admin.password).FirstOrDefault();

                if (ad == null)
                {
                    ModelState.AddModelError("LoginIncorrect", "Email veya Şifre Yanlış");

                    return View(admin);
                }

                Session["admin"] = ad;
                Session["id"] = ad.id;
                Session["adminEmail"] = ad.Email;
                Session["adminName"] = ad.NameSurname;
                Session["password"] = ad.Password;

            }
            catch (Exception e)
            {

                return Content(GetErrorMessage(e));
            }


            return RedirectToAction("Index");
        }

        public ActionResult LogOut()
        {
            FormsAuthentication.SignOut();
            Session.Abandon(); // session'u temizliyor
            return RedirectToAction("Login", "Home");
        }

        [AdminSessionControl]
        public ActionResult Settings()
        {
            Admin admin = (Admin)Session["admin"];
            adminModel temp = new adminModel();

            if (admin == null)
            {
                return RedirectToAction("Login");
            }
            temp.id = admin.id;
            temp.nameSurname = admin.NameSurname;      
            temp.eMail = admin.Email;
            temp.password = admin.Password;

            return View(temp);
        }

        [HttpPost]
        public ActionResult Settings(adminModel model)
        {
            int id = (Int32)Session["id"];
            var admin = db.Admins.Where(a => a.id == id).FirstOrDefault();
            try
            {
               
            
                admin.NameSurname = model.nameSurname;

                UpdateModel(admin);
                db.SubmitChanges();

                var newAdmin = db.Admins.Where(a => a.id ==model.id).FirstOrDefault();
                Session["admin"] = newAdmin;

                ViewBag.message = "Bilgileriniz Gncellendi";

                return RedirectToAction("Settings");
            }
            catch (Exception e)
            {

               return Content(GetErrorMessage(e));
            }
        }


        [OutputCache(NoStore =true, Duration =0,VaryByParam ="None")]
        public ActionResult changePassword()
        {
            changePasswordModel pass = new changePasswordModel();

            return View(pass);
        }
        [OutputCache(NoStore = true, Duration = 0, VaryByParam = "None")]
        [HttpPost]
        public ActionResult changePassword(changePasswordModel newPass)
        {
            if (newPass.oldPassword!=null)
            {
                newPass.oldPassword = FormsAuthentication.HashPasswordForStoringInConfigFile(newPass.oldPassword, "md5");
            }
            
            int id = (Int32)Session["id"];
            var admin = db.Admins.Where(a => a.id == id).FirstOrDefault();

            if (newPass.newPassword!=newPass.confirmPassword)
            {
                ViewBag.message = "Şifreler Uyuşmuyor";
                return View();
            }

            if (ModelState.IsValid && newPass.oldPassword==admin.Password )
            {
               
                admin.Password= FormsAuthentication.HashPasswordForStoringInConfigFile(newPass.newPassword, "md5");
                UpdateModel(admin);
                db.SubmitChanges();
                ViewBag.message = "Şifreniz Başarıyla Değiştirildi";
                return View();
            }
            else
            {
                if (newPass.oldPassword != admin.Password)
                {
                    ViewBag.message = "Eski Şifreniz Yanlış";
                }
                return View();
            }
         
        }
       

        public ActionResult createAdmin()
        {

            adminModel model = new adminModel();

            return View(model);
        }
        [HttpPost]
        public ActionResult createAdmin(adminModel model)
        {
            try
            {
                model.password = FormsAuthentication.HashPasswordForStoringInConfigFile(model.password, "md5");

                Admin admin = new Admin();
                admin.NameSurname = model.nameSurname;
                admin.Email = model.eMail;
              
                admin.Password = model.password;

                db.Admins.InsertOnSubmit(admin);
                db.SubmitChanges();

                return RedirectToAction("Settings");
            }
            catch (Exception e)
            {

                return Content(GetErrorMessage(e));
            }
        }
        [OutputCache(NoStore = true, Duration = 0, VaryByParam = "None")]
        public ActionResult deleteAccount(int id)
        {
            try
            {
                var model = db.Admins.Where(a => a.id == id).FirstOrDefault();
                db.Admins.DeleteOnSubmit(model);
                db.SubmitChanges();

                return RedirectToAction("Login");
            }
            catch (Exception e)
            {

                return Content(GetErrorMessage(e));
            }
        }

        [AdminSessionControl]
        public PartialViewResult getCategory()
        {


            var model = db.SubCategories.ToList();

            return PartialView("~/Areas/Admin/Views/Shared/_myPartial.cshtml", model);

        }

        public ActionResult getSubCategories(int id)
        {
            SubCategory model = db.SubCategories.Where(a => a.id == id).FirstOrDefault();

            var picture = db.Pictures.Where(a => a.SubCategoryid == id).ToList();

            subCategoryAdminModel cat = new subCategoryAdminModel();

            cat.id = id;
            cat.subCategoryName = model.SubcategoryName;
            cat.categoryName = model.Category.CategoryName;
            cat.createdDate = model.UpdatedDate;
            cat.picture = picture;
            cat.description = model.Description;

            return View(cat);
        }
        [AdminSessionControl]
        public ActionResult educatorImage(int id)
        {
           

                var f = this.Request.Files["upload"];

                if (f.ContentLength != 0)
                {

                    string photo = Guid.NewGuid().ToString() + ".jpg";
                    string savedFile = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, Server.MapPath("/Areas/site/Content/img/"));
                    savedFile = Path.Combine(savedFile, photo);

                    string path = "/images/1150x320/" + photo;
                    f.SaveAs(savedFile);

                    FileResize.ImageResize(1150, 320, "/Areas/site/Content/img/" + photo, "/images/1150x320/" + photo);

                    var model = db.Pictures.Where(a => a.SubCategoryid == 15).FirstOrDefault();
                    Picture im = new Picture();
                    im.Position = 1;
                    im.SubCategoryid = 15;
                    im.Path = path;
                    im.Newsid = null;

                    db.Pictures.DeleteOnSubmit(model);

                    db.Pictures.InsertOnSubmit(im);

                    db.SubmitChanges();
                }

                else
                {
                    TempData["uyarı"] = "Lütfen Bir Resim Seçiniz.";
                }
            return RedirectToAction("educatorList",new { id=15});

        }

        [AdminSessionControl]
        [ValidateInput(false)]
        public ActionResult dataList(int id)
        {
           
                try
                {


                    var picture = db.Pictures.Where(a => a.SubCategoryid == id).ToList();

                    string path11 = "", path22 = "";
                    foreach (var item in picture)
                    {
                        if (item.Position == 1)
                        {
                            path11 = item.Path;
                        }
                        if (item.Position == 0)
                        {
                            path22 = item.Path;
                        }
                    }



                    string path1 = "", path2 = "";
                    var f1 = this.Request.Files["image1"];
                    if (f1.ContentLength != 0)
                    {
                        string photo = Guid.NewGuid().ToString() + ".jpg";
                        string savedFile1 = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, Server.MapPath("/Areas/site/Content/img/"));
                        savedFile1 = Path.Combine(savedFile1, photo);
                        //path1 = savedFile1.Substring(savedFile1.Length - 64);
                        path1 = "/images/1150x320/" + photo;
                        f1.SaveAs(savedFile1);

                        FileResize.ImageResize(1150, 320, "/Areas/site/Content/img/" + photo, "/images/1150x320/" + photo);

                    }
                    if (f1.ContentLength == 0)
                    {
                        path1 = path11;
                    }


                    var f2 = this.Request.Files["image2"];
                    if (f2.ContentLength != 0)
                    {
                        string photo2 = Guid.NewGuid().ToString() + ".jpg";
                        string savedFile2 = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, Server.MapPath("/Areas/site/Content/img/"));
                        savedFile2 = Path.Combine(savedFile2, photo2);
                        //path2 = savedFile2.Substring(savedFile2.Length - 64);
                        path2 = "/images/530x320/" + photo2;
                        f2.SaveAs(savedFile2);
                        FileResize.ImageResize(530, 320, "/Areas/site/Content/img/" + photo2, "/images/530x320/" + photo2);
                    }
                    if (f2.ContentLength == 0)
                    {
                        path2 = path22;
                    }


                    foreach (var item in picture)
                    {
                        if (item.Position == 1)
                        {


                            var p = db.Pictures.Where(a => a.id == item.id).FirstOrDefault();


                            db.Pictures.DeleteOnSubmit(p);

                            Picture pctr = new Picture();

                            pctr.Position = item.Position;
                            pctr.Path = path1;
                            pctr.SubCategoryid = item.SubCategoryid;
                            pctr.Newsid = null;

                            db.Pictures.InsertOnSubmit(pctr);
                            db.SubmitChanges();

                        }

                        if (item.Position == 0)
                        {


                            var p = db.Pictures.Where(a => a.id == item.id).FirstOrDefault();
                            db.Pictures.DeleteOnSubmit(p);

                            Picture pctr = new Picture();

                            pctr.Position = item.Position;
                            pctr.Path = path2;
                            pctr.SubCategoryid = item.SubCategoryid;
                            pctr.Newsid = null;

                            db.Pictures.InsertOnSubmit(pctr);
                            db.SubmitChanges();
                        }

                    }



                    var temp = db.SubCategories.Where(a => a.id == id).FirstOrDefault();


                    UpdateModel(temp);
                    db.SubmitChanges();

                }
                catch (Exception e)
                {

                    return Content(GetErrorMessage(e));
                }


                return RedirectToAction("getSubCategories", new { id = id });
            
        }

        [AdminSessionControl]
        public ActionResult educatorList(int id)
        {

            var model = db.Educators.OrderByDescending(a=>a.id).ToList();

            Picture picture = new Picture();
            picture = db.Pictures.Where(a => a.SubCategoryid == id).FirstOrDefault();


            ViewBag.picture = picture.Path;

            ViewBag.id = id;

            return View(model);
        }
        [AdminSessionControl]
        public ActionResult createEducator()
        {
            educatorModel edu = new educatorModel();
            return View(edu);
        }

        [AdminSessionControl]
        [HttpPost]
        public ActionResult createEducator(educatorModel model)
        {
            try
            {

                Educator edu = new Educator();

                var f1 = this.Request.Files["image1"];
                string photo = Guid.NewGuid().ToString() + ".jpg";
                string savedFile1 = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, Server.MapPath("/Areas/site/Content/img/"));
                savedFile1 = Path.Combine(savedFile1, photo);
                var path1 = savedFile1.Substring(savedFile1.Length - 64);


                f1.SaveAs(savedFile1);

                edu.NameSurname = model.nameSurname;
                edu.Company = model.company;
                edu.About = model.about;
                edu.ProfilPicture = path1;
                db.Educators.InsertOnSubmit(edu);
                db.SubmitChanges();
                return RedirectToAction("educatorList", new { id = 15 });
            }
            catch (Exception e)
            {

                return Content(GetErrorMessage(e));
            }
        }
        [OutputCache(NoStore = true, Duration = 0, VaryByParam = "None")]
        [AdminSessionControl]
        public ActionResult editEducator(int id)
        {
            try
            {


                Educator edu = new Educator();
                edu = db.Educators.Where(a => a.id == id).FirstOrDefault();

                educatorModel model = new educatorModel();

                model.id = edu.id;
                model.nameSurname = edu.NameSurname;
                model.company = edu.Company;
                model.about = edu.About;
                model.picturePath = edu.ProfilPicture;


                return View(model);

            }
            catch (Exception e)
            {

                return Content(GetErrorMessage(e));
            }
        }
        [OutputCache(NoStore = true, Duration = 0, VaryByParam = "None")]
        [AdminSessionControl]
        [HttpPost]
        public ActionResult editEducator(educatorModel model)
        {
            try
            {
                var f1 = this.Request.Files["image1"];
                string path1 = "";
                if (f1.ContentLength != 0)
                {
                    string photo = Guid.NewGuid().ToString() + ".jpg";
                    string savedFile1 = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, Server.MapPath("/Areas/site/Content/img/"));
                    savedFile1 = Path.Combine(savedFile1, photo);
                    path1 = savedFile1.Substring(savedFile1.Length - 64);
                    f1.SaveAs(savedFile1);


                    FileResize.ImageResize(1150, 320, "/Areas/site/Content/img/" + photo, "/images/1150x320/" + photo);

                }

                if (f1.ContentLength == 0)
                {
                    var picture = db.Educators.Where(a => a.id == model.id).FirstOrDefault();

                    path1 = picture.ProfilPicture;
                }


                var edu = db.Educators.Where(a => a.id == model.id).FirstOrDefault();
                edu.ProfilPicture = path1;
                
                UpdateModel(edu);
                db.SubmitChanges();

                return RedirectToAction("educatorList", new { id = 15 });
            }
            catch (Exception e)
            {

                return Content(GetErrorMessage(e));
            }
        }
        [OutputCache(NoStore = true, Duration = 0, VaryByParam = "None")]
        [AdminSessionControl]
        public ActionResult deleteEducator(int id)
        {
            try
            {

                var model = db.Educators.Where(a => a.id == id).FirstOrDefault();
                db.Educators.DeleteOnSubmit(model);
                db.SubmitChanges();
                return RedirectToAction("educatorList", new { id = 15 });
            }
            catch (Exception e)
            {

                return Content(GetErrorMessage(e));
            }
        }
        [AdminSessionControl]
        public ActionResult newsAdmin()
        {
            var model = new List<New>();

            model = db.News.OrderByDescending(a=>a.id).ToList();

            return View(model);
        }
        [OutputCache(NoStore =true, Duration =0,VaryByParam ="None")]
        [AdminSessionControl]
        public ActionResult createNews()
        {
            newsModel news = new newsModel();

            return View(news);
        }
        [AdminSessionControl]
        [HttpPost]
        public ActionResult createNews(newsModel model)
        {
            try
            {


                New news = new New();
                news.ShortTitle = model.shortTitle;
                news.Title = model.title;
                news.ContentNews = model.contentNews;

                db.News.InsertOnSubmit(news);
                db.SubmitChanges();
               
                return RedirectToAction("newsAdmin");

            }
            catch (Exception e)
            {

                return Content(GetErrorMessage(e));
            }
        }
        [AdminSessionControl]
        public ActionResult uploadPicture(int id)
        {
            var items = db.Pictures.Where(a => a.Newsid == id).ToList();
            ViewData["id"] = id;
            return View(items);
        }
        [AdminSessionControl]
        public ActionResult upload(int? id)
        {

           
                try
                {
                    StringBuilder sb = new StringBuilder();

                    string photo = Guid.NewGuid().ToString() + ".jpg";

                    var f = this.Request.Files["upload"];
                    string savedFile = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, Server.MapPath("/Areas/site/Content/img/"));
                    savedFile = Path.Combine(savedFile, photo);
                    var path1 = savedFile.Substring(savedFile.Length - 64);
                    f.SaveAs(savedFile);


                      FileResize.ImageResize(100, 100, "/Areas/site/Content/img/" + photo, "/images/100x100/" + photo);
                      FileResize.ImageResize(400, 216, "/Areas/site/Content/img/" + photo, "/images/400x216/" + photo);

                    Picture img = new Picture();

                    img.Path = photo;
                    img.Newsid = id;

                    db.Pictures.InsertOnSubmit(img);
                    db.SubmitChanges();


                    sb.Append("<li id=" + img.id + "><img alt='' src='/images/100x100/" + photo + "'/><div class='actions'><a  class='btn btn-orange btn-small' rel='facebox' href='/images/400x216/" + photo + "'> Göster </a><a href='javascript:;' name=" + img.id + " class='btn btn-grey btn-small delete'>Sil</a></div></li>");
                }
                catch (Exception)
                {

                    TempData["uyarı"] = "Lütfen Bir Resim Seçiniz.";
                }

                return RedirectToAction("uploadPicture", new { id = id });
            
           

        }
        [OutputCache(NoStore = true, Duration = 0, VaryByParam = "None")]
        [AdminSessionControl]
        [HttpPost]
        public ActionResult deletePicture(int id)
        {
            try
            {
                Picture img = db.Pictures.SingleOrDefault(a => a.id == id);
                db.Pictures.DeleteOnSubmit(img);

                var directories = from f in new DirectoryInfo(Server.MapPath("/images/")).GetDirectories() select f;

                foreach (var item in directories)
                {
                    FileInfo ff = new FileInfo(Server.MapPath("/images/" + item.Name + "/" + img.Path));

                    if (ff.Exists)
                        ff.Delete();
                }

                db.SubmitChanges();
            }
            catch (Exception ex)
            {
                return Content(GetErrorMessage(ex));
            }


            return Json(id);
        }
        [OutputCache(NoStore = true, Duration = 0, VaryByParam = "None")]
        [AdminSessionControl]
        public ActionResult editNews(int id)
        {
            var model = db.News.Where(a => a.id == id).FirstOrDefault();
            newsModel news = new newsModel();
            news.id = model.id;
            news.shortTitle = model.ShortTitle;
            news.title = model.Title;
            news.contentNews = model.ContentNews;


            return View(news);
        }
        [OutputCache(NoStore = true, Duration = 0, VaryByParam = "None")]
        [AdminSessionControl]
        [HttpPost]
        public ActionResult editNews(newsModel model)
        {
            try
            {
                var m = db.News.Where(a => a.id == model.id).FirstOrDefault();
                UpdateModel(m);
                db.SubmitChanges();
                return RedirectToAction("newsAdmin");
            }
            catch (Exception e)
            {

                return Content(GetErrorMessage(e));
            }
        }
        [OutputCache(NoStore = true, Duration = 0, VaryByParam = "None")]
        [AdminSessionControl]
        public ActionResult deleteNews(int id)
        {
            try
            {
                var model = db.News.Where(a => a.id == id).FirstOrDefault();
                db.News.DeleteOnSubmit(model);

                var picture = db.Pictures.Where(b => b.Newsid == id).ToList();
                db.Pictures.DeleteAllOnSubmit(picture);
                db.SubmitChanges();
                return RedirectToAction("newsAdmin");
            }
            catch (Exception e)
            {

                return Content(GetErrorMessage(e));
            }
        }
        [OutputCache(NoStore = true, Duration = 0, VaryByParam = "None")]
        [AdminSessionControl]
        public ActionResult Academy()
        {
            var model = db.Categories.Where(a => a.id == 6).FirstOrDefault();

            return View(model);
        }
        [OutputCache(NoStore = true, Duration = 0, VaryByParam = "None")]
        [AdminSessionControl]
        [HttpPost]
        public ActionResult Academy(string url)
        {
            try
            {
                var model = db.Categories.Where(a => a.id == 6).FirstOrDefault();

                model.Url = url;
                UpdateModel(model);
                db.SubmitChanges();
                return View();
            }
            catch (Exception e)
            {

                return Content(GetErrorMessage(e));
            }
        }
        [OutputCache(NoStore = true, Duration = 0, VaryByParam = "None")]
        [AdminSessionControl]
        public ActionResult socialMedya()
        {
            var model = db.SocialMedyas.ToList();
            return View(model);
        }
        [OutputCache(NoStore = true, Duration = 0, VaryByParam = "None")]
        [AdminSessionControl]
        [HttpPost]
        public ActionResult socialMedya(int id, string link)
        {
            try
            {
                var temp = db.SocialMedyas.Where(a => a.id == id).FirstOrDefault();
                temp.Url = link;
                UpdateModel(temp);
                db.SubmitChanges();

                var model = db.SocialMedyas.ToList();
                return RedirectToAction("socialMedya");
            }
            catch (Exception e)
            {

                return Content(GetErrorMessage(e));
            }
        }
        [AdminSessionControl]
        public PartialViewResult getMessage()
        {
            var messageCount = db.UserMessages.Where(a => a.IsRead == false).Count();
            ViewBag.okunmamısMessage = messageCount;

            var model = db.UserMessages.Where(a => a.IsRead == false).OrderByDescending(u => u.id).Take(4);


            return PartialView("~/Areas/Admin/Views/Shared/_messagePartial.cshtml", model);
        }
        [AdminSessionControl]
        public ActionResult seeAll()
        {
            var model = db.UserMessages.Where(a => a.IsRead == false).OrderByDescending(U=>U.id).ToList();


            ViewBag.cevapsız = db.UserMessages.Where(a => a.IsReply == false).OrderByDescending(b=>b.id).ToList();

            ViewBag.cevaplanmıs = db.UserMessages.Where(a => a.IsReply == true).OrderByDescending(b => b.id).ToList();


            return View(model);
        }
        [OutputCache(NoStore = true, Duration = 0, VaryByParam = "None")]
        [AdminSessionControl]
        public ActionResult readMessage(int id)
        {
            try
            {
                var model = db.UserMessages.Where(a => a.id == id).FirstOrDefault();

                var models = db.Admins.Where(a => a.id == model.Adminid).FirstOrDefault();
                ViewBag.admin = (Admin)models;
                model.IsRead = true;
                UpdateModel(model);
                db.SubmitChanges();
                return View(model);
            }
            catch (Exception e)
            {

                return Content(GetErrorMessage(e));
            }
        }
        [OutputCache(NoStore = true, Duration = 0, VaryByParam = "None")]
        [AdminSessionControl]
        [HttpPost]
        public ActionResult deleteMessage(int id)
        {
            try
            {
                var model = db.UserMessages.Where(a => a.id == id).FirstOrDefault();

                db.UserMessages.DeleteOnSubmit(model);
                db.SubmitChanges();
                return RedirectToAction("seeAll");
            }
            catch (Exception e)
            {

                return Content(GetErrorMessage(e));
            }
        }
        [OutputCache(NoStore = true, Duration = 0, VaryByParam = "None")]
        [AdminSessionControl]
        [HttpPost]
        public ActionResult answerMessage(string answer, int id)
        {
            try
            {
                var model = db.UserMessages.Where(a => a.id == id).FirstOrDefault();
                model.IsReply = true;

                model.Adminid = (Int32)Session["id"];
                model.AnswerMessage = answer;

                UpdateModel(model);
                db.SubmitChanges();

                var admiName = Session["adminName"];
                var adminEmail = Session["adminEmail"];
                var adminPassword = (string)Session["password"];

                if (ModelState.IsValid)
                {
                    var body = "<p>Email From: {0} ({1})</p><p>Message:</p><p>{2}</p>";
                    var message = new MailMessage();
                    message.To.Add(new MailAddress((string)model.Email));
                    message.From = new MailAddress((string)adminEmail);
                    message.Subject = "Kıraça Holding A.Ş.";
                    message.Body = string.Format(body, (string)admiName, (string)adminEmail, answer);
                    message.IsBodyHtml = true;

                    using (var smtp = new SmtpClient())
                    {
                        var credential = new NetworkCredential
                        {
                            UserName = "kiraca@kiraca.com.tr",  
                            Password = ""  
                        };
                        smtp.Credentials = credential;
                        smtp.Host = "mail.kiraca.com.tr";
                        smtp.Port = 587;
                        smtp.EnableSsl = true;
                        smtp.Send(message);

                        return RedirectToAction("readMessage", new { id = id });
                    }
                }

                return RedirectToAction("readMessage", model.id);
            }
            catch (Exception e)
            {

                return Content(GetErrorMessage(e));
            }

        }
    }
}