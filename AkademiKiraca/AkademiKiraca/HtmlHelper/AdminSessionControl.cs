using AkademiKiraca.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace AkademiKiraca.HtmlHelper
{
    public class AdminSessionControl:ActionFilterAttribute,IActionFilter
    {
      
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
           
            Admin admin = (Admin)HttpContext.Current.Session["admin"];

            if (admin == null)
            {
                HttpContext.Current.Response.Redirect("~/admin/Home/Login",false);
            }
            base.OnActionExecuting(filterContext);
        }
    }
}