/* NDLanguageController.cs
 * 
 * Nicole Dahlquist - Section 3
 * 
 * Created: Nov 4, 2014
 *  + Create NDLanguage using empty template
 *  + Create ChangeLanguage() and ChangeLanguage.cshtml
 *  + Create dropdowns for language selection
 *  + Store selected language in a cookie
 *  + Set to return to view user started on
 * 
 * Revision History:
 * Nov 6, 2014
 *  + Make dropdown default to selected language
 *  + Create SetLanguage(), use cookie to set language, default English
 *  + Make SetLanguage accept System.Web.HttpCookieCollection cookie
 * Nov 13, 2014
 * + Add comments
 * 
 */
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace NDSailing.Controllers
{
    /// <summary>
    /// this class provides everything necessary to select a new language
    /// </summary>
    public class NDLanguageController : Controller
    {
        SelectList Languages;
        
        // Index view for NDLanguageController.  Not called from Member's page
        public ActionResult Index()
        {
            return View();
        }
        
        // This method provides the view with dropdowns for language selection
        public ActionResult ChangeLanguage()
        {
            string choice = "";
            Response.Cookies.Add(new HttpCookie("returnURL", Request.UrlReferrer.PathAndQuery));
            
            // if language is selected, default value is the selected language
            if (Request.Cookies["language"] != null)
            {
                choice = Request.Cookies["language"].Value;
                if (choice == "fi")
                {
                    SelectListItem en = new SelectListItem() { Text = "English", Value = "en" };
                    SelectListItem fi = new SelectListItem() { Text = "Suomi", Value = "fi" };
                    SelectListItem nl = new SelectListItem() { Text = "Nederlands", Value = "nl" };
                    Languages = new SelectList(new SelectListItem[] { en, fi, nl }, "Value", "Text", choice);
                }
                else if (choice == "nl")
                {
                    SelectListItem en = new SelectListItem() { Text = "English", Value = "en" };
                    SelectListItem fi = new SelectListItem() { Text = "Suomi", Value = "fi" };
                    SelectListItem nl = new SelectListItem() { Text = "Nederlands", Value = "nl"};

                    Languages = new SelectList(new SelectListItem[] { en, fi, nl }, "Value", "Text", choice);
                }
                else if (choice == "en")
                {
                    SelectListItem en = new SelectListItem() { Text = "English", Value = "en" };
                    SelectListItem fi = new SelectListItem() { Text = "Suomi", Value = "fi" };
                    SelectListItem nl = new SelectListItem() { Text = "Nederlands", Value = "nl" };
                    Languages = new SelectList(new SelectListItem[] { en, fi, nl }, "Value", "Text", choice);
                }
            }
            else
            {
                SelectListItem en = new SelectListItem() { Text = "English", Value = "en" };
                SelectListItem fi = new SelectListItem() { Text = "Suomi", Value = "fi" };
                SelectListItem nl = new SelectListItem() { Text = "Nederlands", Value = "nl" };
                Languages =  new SelectList(new SelectListItem[] { en, fi, nl }, "Value", "Text", choice);
            }

            ViewBag.Languages = Languages;
            
            return View();
        }
        
        // Displays the ChangeLanguage view after a language is selected
        [HttpPost]
        public void ChangeLanguage(string language)
        {
            Response.Cookies.Add(new HttpCookie("language", language));
            if (Request.Cookies["returnURL"] != null)
            {
                Response.Redirect(Request.Cookies["returnURL"].Value);
            }
            else
            {
                Response.Redirect("/");
            }
        }
        
        // overriding initialize method, display depends on requestContext
        protected override void Initialize(System.Web.Routing.RequestContext requestContext)
        {
            base.Initialize(requestContext);

            if(Request.Cookies["language"]!= null)
            {
                System.Threading.Thread.CurrentThread.CurrentUICulture =
                    new System.Globalization.CultureInfo(Request.Cookies["language"].Value);

                System.Threading.Thread.CurrentThread.CurrentCulture =
                    System.Globalization.CultureInfo.CreateSpecificCulture(Request.Cookies["language"].Value);
            }
            Response.Cookies.Add(new HttpCookie("returnURL", Request.RawUrl));
        }

        // overloaded SetLanguage method that accepts cookies
        public static void SetLanguage(System.Web.HttpCookieCollection cookie)
        {
            if (cookie["language"] != null)
            {
                System.Threading.Thread.CurrentThread.CurrentUICulture =
                    new System.Globalization.CultureInfo(cookie["language"].Value);

                System.Threading.Thread.CurrentThread.CurrentCulture =
                    System.Globalization.CultureInfo.CreateSpecificCulture(cookie["language"].Value);
            }
            else
            {
                System.Threading.Thread.CurrentThread.CurrentCulture =
                    new System.Globalization.CultureInfo("en");

                System.Threading.Thread.CurrentThread.CurrentCulture =
                    System.Globalization.CultureInfo.CreateSpecificCulture("en");
            }
        }

        // overloaded SetLanguage method that accepts session variables
        public void SetLanguage(System.Web.HttpSessionStateBase session)
        {
            if (session["language"] != null)
            {
                System.Threading.Thread.CurrentThread.CurrentUICulture =
                    new System.Globalization.CultureInfo(session["language"].ToString());

                System.Threading.Thread.CurrentThread.CurrentCulture =
                    System.Globalization.CultureInfo.CreateSpecificCulture(session["language"].ToString());
            }
            else
            {
                System.Threading.Thread.CurrentThread.CurrentCulture =
                    new System.Globalization.CultureInfo("en");

                System.Threading.Thread.CurrentThread.CurrentCulture =
                    System.Globalization.CultureInfo.CreateSpecificCulture("en");
            }
        }
    }
}