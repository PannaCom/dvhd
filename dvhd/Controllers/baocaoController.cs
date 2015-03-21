using iTextSharp.text;
using iTextSharp.text.html.simpleparser;
using iTextSharp.text.pdf;
using System;
using System.Collections.Generic;
using System.IO;
using System.Web.Mvc;

namespace dvhd.Controllers
{
    public class baocaoController : Controller
    {
        //
        // GET: /baocao/
        public ActionResult Index()
        {
            return View();
        }

        //
        // GET: /BaoCaoTheoLoai/
        public ActionResult BaoCaoTheoLoai()
        {
            if (Config.getCookie("logged") == "") return RedirectToAction("Login", "Home");
            if (!Config.checkPermission(Config.getCookie("logged"), "BC1")) return RedirectToAction("Permission", "Home");
            return View();
        }

        //
        // GET: /BaoCaoTheoTinh/
        public ActionResult BaoCaoTheoTinh()
        {
            return View();
        }

        //
        // GET: /BaoCaoTheoTinh/
        public ActionResult BaoCaoTheoDoiTuong()
        {
            return View();
        }

        //create report
        [HttpPost]
        [ValidateInput(false)]
        public string createReport(string html, int type)
        {
            string path = HttpContext.Server.MapPath("../Images/Report"+ "\\");
            Document document = new Document();
            try
            {
                var reportName = "";
                switch (type)
                {
                    case 1:
                        reportName = "BaoCaoTheoLoai"+Guid.NewGuid().ToString()+".pdf";
                        break;
                    case 2:
                        reportName = "BaoCaoTheoTinh" + Guid.NewGuid().ToString() + ".pdf";
                        break;
                    case 3:
                        reportName = "BaoCaoTheoDoiTuong" + Guid.NewGuid().ToString() + ".pdf";
                        break;
                    default:
                        reportName = "BaoCao" + Guid.NewGuid().ToString() + ".pdf";
                        break;
                }

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
