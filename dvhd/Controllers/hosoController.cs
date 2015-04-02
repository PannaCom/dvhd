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
using System.Drawing;
using System.Drawing.Drawing2D;

namespace dvhd.Controllers
{
    public class hosoController : Controller
    {
        private dvhdEntities db = new dvhdEntities();

        //
        // GET: /hoso/

        public ActionResult Index(int? page, string keyword, string fromdate, string todate)
        {
            DateTime fdate = DateTime.Now.AddDays(-730);
            DateTime tdate = DateTime.Now;
            if (fromdate != "" && fromdate != null)
            {
                fdate = Config.convertToDateTimeFromString(fromdate);
            }
            if (todate != "" && todate != null)
            {
                tdate = Config.convertToDateTimeFromString(todate);
            }
            ViewBag.fdate = fdate;
            ViewBag.fromdate = fromdate;
            ViewBag.tdate = tdate;
            ViewBag.todate = todate;
            if (keyword == null) keyword = "";
            ViewBag.keyword = keyword;
            if (Config.getCookie("logged") == "") return RedirectToAction("Login", "Home");
            if (!Config.checkPermission(Config.getCookie("logged"), "HS0")) return RedirectToAction("Permission", "Home");
            var p = (from q in db.HoSoes where (q.hoten.Contains(keyword) || q.cmthochieu.Contains(keyword)) && q.thoigianvipham>=fdate && q.thoigianvipham<=tdate select q).OrderByDescending(o => o.id).Take(1000);
            int pageSize = Config.PageSize;
            int pageNumber = (page ?? 1);
            ViewBag.page = page;
            return View(p.ToPagedList(pageNumber, pageSize));
        }
        [HttpPost]
        [AcceptVerbs(HttpVerbs.Post)]
        public string UploadImageProcess(HttpPostedFileBase file, string filename)
        {
            string physicalPath = HttpContext.Server.MapPath("../" + Config.HoSoImagePath + "\\");
            string nameFile = String.Format("{0}.jpg", Guid.NewGuid().ToString());
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
            string ok = resizeImage(Config.imgWidthNews, Config.imgHeightNews, fullPath, Config.HoSoImagePath + "/" + nameFile);
            return Config.HoSoImagePath + "/" + nameFile;
        }
        [HttpPost]
        [AcceptVerbs(HttpVerbs.Post)]
        public string UploadImageProcessLoaidongvat(HttpPostedFileBase file, string filename)
        {
            string physicalPath = HttpContext.Server.MapPath("../" + Config.HoSoImagePath + "\\");
            string nameFile = String.Format("{0}.jpg", Guid.NewGuid().ToString());
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
            //string ok = resizeImage(Config.imgWidthNews, Config.imgHeightNews, fullPath, Config.HoSoImagePath + "/" + nameFile);
            return Config.HoSoImagePath + "/" + nameFile;
        }
        public string resizeImage(int maxWidth, int maxHeight, string fullPath, string path)
        {

            var image = System.Drawing.Image.FromFile(fullPath);
            var ratioX = (double)maxWidth / image.Width;
            var ratioY = (double)maxHeight / image.Height;
            var ratio = Math.Min(ratioX, ratioY);
            var newWidth = (int)(image.Width * ratioX);
            var newHeight = (int)(image.Height * ratioY);
            var newImage = new Bitmap(newWidth, newHeight);
            Graphics thumbGraph = Graphics.FromImage(newImage);

            thumbGraph.CompositingQuality = CompositingQuality.HighQuality;
            thumbGraph.SmoothingMode = SmoothingMode.HighQuality;
            //thumbGraph.InterpolationMode = InterpolationMode.HighQualityBicubic;

            thumbGraph.DrawImage(image, 0, 0, newWidth, newHeight);
            image.Dispose();

            string fileRelativePath = path;// "newsizeimages/" + maxWidth + Path.GetFileName(path);
            newImage.Save(HttpContext.Server.MapPath(fileRelativePath), newImage.RawFormat);
            return fileRelativePath;
        }
        //
        // GET: /hoso/Details/5

        public ActionResult Details(int id = 0)
        {
            if (Config.getCookie("logged") == "") return RedirectToAction("Login", "Home");
            if (!Config.checkPermission(Config.getCookie("logged"), "HS2")) return RedirectToAction("Permission", "Home");
            HoSo hoso = db.HoSoes.Find(id);
            if (hoso == null)
            {
                return HttpNotFound();
            }
            return View(hoso);
        }

        //
        // GET: /hoso/Create

        public ActionResult Create(string hosocode)
        {
            if (Config.getCookie("logged") == "") return RedirectToAction("Login", "Home");
            if (!Config.checkPermission(Config.getCookie("logged"), "HS1")) return RedirectToAction("Permission", "Home");
            if (hosocode == null)
            {
                ViewBag.hosocode = Guid.NewGuid().ToString();
            }
            else { ViewBag.hosocode = hosocode; }
            return View();
        }
        public ActionResult AddMore(string hosocode)
        {
            ViewBag.hosocode = hosocode;
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
                if (hoso.xulytangvat == null) hoso.xulytangvat = "";
                if (hoso.nguyennhankhongxuly == null) hoso.nguyennhankhongxuly = "";
                db.HoSoes.Add(hoso);
                db.SaveChanges();
                return RedirectToAction("AddMore", new { hosocode = hoso.hosocode });
                //return RedirectToAction("Index");
            }

            return View(hoso);
        }

        //
        // GET: /hoso/Edit/5

        public ActionResult Edit(int id = 0)
        {
            if (Config.getCookie("logged") == "") return RedirectToAction("Login", "Home");
            if (!Config.checkPermission(Config.getCookie("logged"), "HS3")) return RedirectToAction("Permission", "Home");
            HoSo hoso = db.HoSoes.Find(id);
            ViewBag.hosocode = hoso.hosocode;
            if (hoso.hosocode == null) ViewBag.hosocode = Guid.NewGuid().ToString();
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
                if (hoso.xulytangvat == null) hoso.xulytangvat = "";
                if (hoso.nguyennhankhongxuly == null) hoso.nguyennhankhongxuly = "";
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
            if (Config.getCookie("logged") == "") return RedirectToAction("Login", "Home");
            if (!Config.checkPermission(Config.getCookie("logged"), "HS4")) return RedirectToAction("Permission", "Home");
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
                string path = HttpContext.Server.MapPath("../Images/Report" + "\\");
                string reportName = "HoSoDayDu" + Guid.NewGuid().ToString() + ".pdf";
                PdfWriter.GetInstance(document, new FileStream(path + reportName, FileMode.Create));
                document.Open();                
                List<IElement> htmlarraylist = HTMLWorker.ParseToList(new StringReader(html), null);
                for (int k = 0; k < htmlarraylist.Count; k++)
                {
                    document.Add((IElement)htmlarraylist[k]);
                }

                document.Close();
                return reportName;
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }
    }
}