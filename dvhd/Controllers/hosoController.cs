using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using dvhd.Models;
using PagedList;
using System.IO;
using iTextSharp.text;
using iTextSharp.text.pdf;
using iTextSharp.text.html.simpleparser;

namespace dvhd.Controllers
{
    public class hosoController : Controller
    {
        private dvhdEntities db = new dvhdEntities();

        //
        // GET: /hoso/

        public ActionResult Index(int? page)
        {
            var p = (from q in db.HoSoes select q).OrderByDescending(o => o.id).Take(1000);
            int pageSize = Config.PageSize;
            int pageNumber = (page ?? 1);
            ViewBag.page = page;
            return View(p.ToPagedList(pageNumber, pageSize));
        }

        //
        // GET: /hoso/Details/5

        public ActionResult Details(int id = 0)
        {
            HoSo hoso = db.HoSoes.Find(id);
            if (hoso == null)
            {
                return HttpNotFound();
            }
            return View(hoso);
        }

        //
        // GET: /hoso/Create

        public ActionResult Create()
        {
            return View();
        }

        //
        // POST: /hoso/Create

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(HoSo hoso)
        {
            if (ModelState.IsValid)
            {
                db.HoSoes.Add(hoso);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(hoso);
        }

        //
        // GET: /hoso/Edit/5

        public ActionResult Edit(int id = 0)
        {
            HoSo hoso = db.HoSoes.Find(id);
            if (hoso == null)
            {
                return HttpNotFound();
            }
            return View(hoso);
        }

        //
        // POST: /hoso/Edit/5

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(HoSo hoso)
        {
            if (ModelState.IsValid)
            {
                db.Entry(hoso).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(hoso);
        }

        //
        // GET: /hoso/Delete/5

        public ActionResult Delete(int id = 0)
        {
            HoSo hoso = db.HoSoes.Find(id);
            if (hoso == null)
            {
                return HttpNotFound();
            }
            return View(hoso);
        }

        //
        // POST: /hoso/Delete/5

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            HoSo hoso = db.HoSoes.Find(id);
            db.HoSoes.Remove(hoso);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }

        //create report
        [HttpPost]
        [ValidateInput(false)]
        public string createReport(string html) {
            Document document = new Document();
            try
            {
                PdfWriter.GetInstance(document, new FileStream("c:\\report.pdf", FileMode.Create));
                document.Open();                
                List<IElement> htmlarraylist = HTMLWorker.ParseToList(new StringReader(html), null);
                for (int k = 0; k < htmlarraylist.Count; k++)
                {
                    document.Add((IElement)htmlarraylist[k]);
                }

                document.Close();
                return "ok";
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }
    }
}