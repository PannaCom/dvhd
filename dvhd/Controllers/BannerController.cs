using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using dvhd.Models;

namespace dvhd.Controllers
{
    public class BannerController : Controller
    {
        private dvhdEntities db = new dvhdEntities();

        //
        // GET: /Banner/

        public ActionResult Index()
        {
            return View(db.banners.ToList());
        }

        //
        // GET: /Banner/Details/5

        public ActionResult Details(int id = 0)
        {
            banner banner = db.banners.Find(id);
            if (banner == null)
            {
                return HttpNotFound();
            }
            return View(banner);
        }
        [HttpPost]
        [AcceptVerbs(HttpVerbs.Post)]
        public string UploadImageProcess(HttpPostedFileBase file, string filename)
        {
            string physicalPath = HttpContext.Server.MapPath("../" + Config.SlideImagePath + "\\");
            string nameFile = String.Format("{0}.jpg", filename);
            int countFile = Request.Files.Count;
            string fullPath = physicalPath + System.IO.Path.GetFileName(nameFile);
            for (int i = 0; i < countFile; i++)
            {
                if (System.IO.File.Exists(fullPath))
                {
                    System.IO.File.Delete(fullPath);
                }
                Request.Files[i].SaveAs(fullPath);
                break;
            }
            //string ok = resizeImage(Config.imgWidthBigSlide, Config.imgHeightBigSlide, fullPath, Config.SlideImagePath + "/" + nameFile);
            return Config.SlideImagePath + "/" + nameFile;
        }
        //
        // GET: /Banner/Create

        public ActionResult Create()
        {
            ViewBag.filename = Guid.NewGuid().ToString();
            return View();
        }

        //
        // POST: /Banner/Create

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(banner banner)
        {
            if (ModelState.IsValid)
            {
                db.banners.Add(banner);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(banner);
        }

        //
        // GET: /Banner/Edit/5

        public ActionResult Edit(int id = 0)
        {
            banner banner = db.banners.Find(id);
            if (banner == null)
            {
                return HttpNotFound();
            }
            return View(banner);
        }

        //
        // POST: /Banner/Edit/5

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(banner banner)
        {
            if (ModelState.IsValid)
            {
                db.Entry(banner).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(banner);
        }

        //
        // GET: /Banner/Delete/5

        public ActionResult Delete(int id = 0)
        {
            banner banner = db.banners.Find(id);
            if (banner == null)
            {
                return HttpNotFound();
            }
            return View(banner);
        }

        //
        // POST: /Banner/Delete/5

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            banner banner = db.banners.Find(id);
            db.banners.Remove(banner);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }
}