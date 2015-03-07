/* NDMemberController.cs
 * 
 * Nicole Dahlquist - Section 3
 * 
 * Created: Nov 4, 2014
 *  + Import SailingSQL.mdf, generate context and models
 *  + Generate NDMemberController and add link for it in menu
 *  + Alter layout page
 *  + Add App_GlobalResources folder and create NDTranslations.resx file
 *  + Copy NDTranslations.resx into Finnish and Dutch language files
 * 
 * Revision History:
 * Nov 6, 2014
 *  + Imported Finnish/Dutch translation files
 *  + Began entering name/value pairs for translation
 * Nov 7, 2014
 * + Create protected override void Initialize(System.Web.Routing.RequestContext requestContex)
 * + Initialize request context and set the language using SetLanguage in NDLanguageController
 * + Add ModelState errors to create and Edit Views
 * + Translate error message
 * + Add success message if updated or deleted, and fail message for delete failure
 * Nov 11, 2014
 * + Add translations for fields, hyperlinks, and error messages
 * Nov 13, 2014
 * + Add comments
 * 
 * 
 */
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using NDSailing.Models;
using NDSailing.App_GlobalResources;

namespace NDSailing.Controllers
{
    /// <summary>
    /// This Class provides everything necessary to work with member information
    /// </summary>
    public class NDMemberController : Controller
    {
        private sailSQLContext db = new sailSQLContext();

        // override method for Initialize method
        protected override void Initialize(System.Web.Routing.RequestContext requestContext)
        {
            base.Initialize(requestContext);
            NDLanguageController.SetLanguage(Request.Cookies);
        }

        // NDMember Index View displays member info
        public ActionResult Index()
        {
            var members = db.members.Include(m => m.province);
            return View(members.ToList());
        }

        // displays details on selected member
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            member member = db.members.Find(id);
            if (member == null)
            {
                return HttpNotFound();
            }
            return View(member);
        }

        // allows creation of new member
        public ActionResult Create()
        {
            ViewBag.provinceCode = new SelectList(db.provinces, "provinceCode", "name");
            return View();
        }

        // Post-back method for member creation
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "memberId,fullName,firstName,lastName,spouseFirstName,spouseLastName,street,city,provinceCode,postalCode,homePhone,email,yearJoined,comment,taskExempt,useCanadaPost")] member member)
        {
            try
            {
                ValidateModel(member);
                
                db.members.Add(member);
                db.SaveChanges();
                TempData["message"] = String.Format(NDTranslations.creationSuccessful);
                return RedirectToAction("Index", "NDMember");
            }
            catch(Exception ex)
            {
                ModelState.AddModelError("", String.Format(NDTranslations.insertException) + ex.GetBaseException().Message);
                Create();
                ViewBag.provinceCode = new SelectList(db.provinces, "provinceCode", "name", member.provinceCode);
                return View(member);
            }
         }

        // Allows edit of the selected member's information
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            member member = db.members.Find(id);
            if (member == null)
            {
                return HttpNotFound();
            }
            ViewBag.provinceCode = new SelectList(db.provinces, "provinceCode", "name", member.provinceCode);
            return View(member);
        }

        // Post-back method for member info edit
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "memberId,fullName,firstName,lastName,spouseFirstName,spouseLastName,street,city,provinceCode,postalCode,homePhone,email,yearJoined,comment,taskExempt,useCanadaPost")] member member)
        {
            try
            {
                ValidateModel(member);

                db.Entry(member).State = EntityState.Modified;
                db.SaveChanges();
                TempData["message"] = String.Format(NDTranslations.editSuccess);
                return RedirectToAction("Index", "NDMember");
            }
            catch(Exception ex)
            {
                ModelState.AddModelError("", String.Format(NDTranslations.dbWritingError) + ex.GetBaseException().Message);
                ViewBag.provinceCode = new SelectList(db.provinces, "provinceCode", "name", member.provinceCode);
                Edit(member.memberId);
                return View(member);
            }
        }

        // allows the deletion of selected member
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            member member = db.members.Find(id);
            if (member == null)
            {
                return HttpNotFound();
            }
            return View(member);
        }

        // Post-back method for member deletion
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            try
            {
                member member = db.members.Find(id);
                db.members.Remove(member);
                TempData["message"] = String.Format(NDTranslations.deleteSuccess);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            catch(Exception ex)
            {
                member member = db.members.Find(id);
                ModelState.AddModelError("", String.Format(NDTranslations.deleteError) + ex.GetBaseException().Message);
                TempData["message"] = String.Format(NDTranslations.deleteFail);
                Edit(member.memberId);
                return View(member);
            }
            
        }
        // garbage collection
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
