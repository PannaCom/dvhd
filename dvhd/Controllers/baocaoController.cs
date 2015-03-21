using iTextSharp.text;
using iTextSharp.text.html.simpleparser;
using iTextSharp.text.pdf;
using System;
using System.Collections.Generic;
using System.IO;
using System.Web.Mvc;
using dvhd.Models;
using System.Linq;
namespace dvhd.Controllers
{
    public class baocaoController : Controller
    {
        private dvhdEntities db = new dvhdEntities();
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
        // GET: /BaoCaoTongHop/
        public ActionResult BaoCaoTongHop()
        {
            return View();
        }
        public string baocaotonghopreport(string tinhvipham) {
            try
            {
                //int tongsovu, sovuhanhchinh, sovuhinhsu,;
                var p = (from q in db.HoSoes where q.tinhvipham.Contains(tinhvipham) orderby q.tinhvipham, q.quanvipham select new { tinhvipham = q.tinhvipham, quanvipham = q.quanvipham }).Distinct().Take(1000).ToList();
                var content = "<tr><th>Tỉnh thành</th><th>Quận huyện</th><th>Tổng số vụ</th><th>Số vụ vi phạm hành chính</th><th>Số vụ vi phạm hình sự</th><th>Số vụ xử lý hành chính</th><th>Số vụ xử lý hình sự</th><th>Số vụ không xử lý</th><tr>";
                for (int i = 0; i < p.Count; i++) {
                    string qvp = p[i].quanvipham;
                    string hinhthucvipham0 = Config.hinhthucvipham[0];
                    string hinhthucvipham1 = Config.hinhthucvipham[1];
                    string ketquaxuly1 = Config.ketquaxuly[1];
                    string ketquaxuly2 = Config.ketquaxuly[2];
                    string ketquaxuly3 = Config.ketquaxuly[3];
                    int? tongsovu = (int?)db.HoSoes.Count(o => o.quanvipham.Contains(qvp));//.Count(o => o.quanvipham.Contains(p[i].quanvipham));//.Count();//(from c in db.HoSoes where c.quanvipham.Contains(p[i].quanvipham) select c).ToList().Count();
                    int sovuhanhchinh = (int)db.HoSoes.Count(o => o.quanvipham.Contains(qvp)  && o.hinhthucvipham.Contains(hinhthucvipham0));//(from c in db.HoSoes where c.quanvipham.Contains(p[i].quanvipham) && c.hinhthucvipham.Contains(Config.hinhthucvipham[0]) select c).ToList().Count();
                    int sovuhinhsu = (int)db.HoSoes.Count(o => o.quanvipham.Contains(qvp) && o.hinhthucvipham.Contains(hinhthucvipham1));// (from c in db.HoSoes where c.quanvipham.Contains(p[i].quanvipham) && c.hinhthucvipham.Contains(Config.hinhthucvipham[1]) select c).ToList().Count();
                    int xulyhanhchinh = (int)db.HoSoes.Count(o => o.quanvipham.Contains(qvp) && o.ketquaxuly.Contains(ketquaxuly1));// (from c in db.HoSoes where c.quanvipham.Contains(p[i].quanvipham) && c.ketquaxuly.Contains(Config.ketquaxuly[1]) select c).ToList().Count();
                    int xulyhinhsu = (int)db.HoSoes.Count(o => o.quanvipham.Contains(qvp) && o.ketquaxuly.Contains(ketquaxuly2)); ;//(from c in db.HoSoes where c.quanvipham.Contains(p[i].quanvipham) && c.ketquaxuly.Contains(Config.ketquaxuly[2]) select c).ToList().Count();
                    int khongxuly = (int)db.HoSoes.Count(o => o.quanvipham.Contains(qvp) && o.ketquaxuly.Contains(ketquaxuly3));//(from c in db.HoSoes where c.quanvipham.Contains(p[i].quanvipham) && c.ketquaxuly.Contains(Config.ketquaxuly[3]) select c).ToList().Count();
                    content += "<tr><td>" + p[i].tinhvipham + "</td><td>" + p[i].quanvipham + "</td><td>" + tongsovu + "</td><td>" + sovuhanhchinh + "</td><td>" + sovuhinhsu + "</td><td>" + xulyhanhchinh + "</td><td>" + xulyhinhsu + "</td><td>" + khongxuly + "</td>";
                }
                return content;
            }
            catch (Exception ex) {
                return "0";
            }
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
