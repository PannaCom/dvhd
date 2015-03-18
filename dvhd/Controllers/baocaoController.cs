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
            Document document = new Document();
            try
            {
                var reportName = "";
                switch (type)
                {
                    case 1:
                        reportName = "d:\\BaoCaoTheoLoai.pdf";
                        break;
                    case 2:
                        reportName = "d:\\BaoCaoTheoTinh.pdf";
                        break;
                    case 3:
                        reportName = "d:\\BaoCaoTheoDoiTuong.pdf";
                        break;
                    default:
                        reportName = "d:\\BaoCao.pdf";
                        break;
                }

                PdfWriter.GetInstance(document, new FileStream(reportName, FileMode.Create));
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
