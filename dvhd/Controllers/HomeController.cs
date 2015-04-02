using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using dvhd.Models;
using Newtonsoft.Json;
using PagedList;
namespace dvhd.Controllers
{
    public class HomeController : Controller
    {
        private static dvhdEntities db = new dvhdEntities();
        public ActionResult Index()
        {
            if (Config.getCookie("logged") == "") return RedirectToAction("Login", "Home");
            ViewBag.Message = "Modify this template to jump-start your ASP.NET MVC application.";

            return View();
        }
        public ActionResult Login()
        {
            return View();
        }
        public ActionResult Logout()
        {
            if (Request.Cookies["logged"] != null)
            {
                Response.Cookies["logged"].Expires = DateTime.Now.AddDays(-1);
            }
            Session.Abandon();
            return View();
        }
        public ActionResult Permission() {
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your app description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        #region DuyVT
        
        public string getLoaiDetails(string keyword,string fromdate,string todate)
        {
            DateTime fdate = DateTime.Now.AddDays(-1000);
            DateTime tdate = DateTime.Now.AddDays(1000);
            if (fromdate != "")
            {
                fdate = Config.convertToDateTimeFromString(fromdate);
            }
            if (todate != "")
            {
                tdate = Config.convertToDateTimeFromString(todate);
            } 
            var p = (from q in db.HoSoes
                     where q.loaidongvat.Contains(keyword) && q.thoigianvipham>=fdate && q.thoigianvipham<=tdate
                     orderby q.loaidongvat
                     select new
                     {
                         q.id,
                         q.loaidongvat,
                         q.tinhtrangbaoton,
                         q.donvitinh,
                         q.soluongchitiet,
                         q.trigiatangvat,
                         q.tendonvibatgiu,
                         q.phuongthucvanchuyen,
                         q.tuyenduongvanchuyen,
                         q.thoigianvipham
                     }).OrderBy(o=>o.loaidongvat).ThenBy(o=>o.thoigianvipham);
            return JsonConvert.SerializeObject(p.ToList());
        }
        
        public string getQuanHuyenDetails(string keyword,string fromdate,string todate)
        {
            DateTime fdate = DateTime.Now.AddDays(-1000);
            DateTime tdate = DateTime.Now.AddDays(1000);
            if (fromdate != "")
            {
                fdate = Config.convertToDateTimeFromString(fromdate);
            }
            if (todate != "")
            {
                tdate = Config.convertToDateTimeFromString(todate);
            } 
            if (keyword.Contains("/"))
            {
                var quanTinh = keyword.Trim().Split('/');
                var quan = quanTinh[0].Trim();
                var tinh = quanTinh[1].Trim();
                var p1 = (from q in db.HoSoes
                          where q.quanvipham.Contains(quan) && q.tinhvipham.Contains(tinh) && q.thoigianvipham >= fdate && q.thoigianvipham <= tdate
                          orderby q.tinhvipham, q.quanvipham
                          select new
                          {
                              diaban = q.quanvipham + "/" + q.tinhvipham,
                              q.id,
                              q.hoten,
                              q.choohientai,
                              q.hanhvivipham,
                              q.loaidongvat,
                              q.soluongchitiet,
                              q.tendonvibatgiu,
                              q.thoigianvipham
                          }).OrderBy(o=>o.diaban).ThenBy(o=>o.thoigianvipham);
                return JsonConvert.SerializeObject(p1.ToList());
            }
            var p2 = (from q in db.HoSoes
                      where (q.quanvipham.Contains(keyword) || q.tinhvipham.Contains(keyword)) && q.thoigianvipham >= fdate && q.thoigianvipham <= tdate
                      orderby q.tinhvipham, q.quanvipham
                      select new
                      {
                          diaban = q.quanvipham + "/" + q.tinhvipham,
                          q.id,
                          q.hoten,
                          q.choohientai,
                          q.hanhvivipham,
                          q.loaidongvat,
                          q.soluongchitiet,
                          q.tendonvibatgiu,
                          q.thoigianvipham
                      }).OrderBy(o => o.diaban).ThenBy(o => o.thoigianvipham);
            return JsonConvert.SerializeObject(p2.ToList());
        }

        public string getCMTDetails(string keyword, string fromdate, string todate)
        {
            DateTime fdate = DateTime.Now.AddDays(-1000);
            DateTime tdate = DateTime.Now.AddDays(1000);
            if (fromdate != "")
            {
                fdate = Config.convertToDateTimeFromString(fromdate);                
            }
            if (todate != "")
            {
                tdate = Config.convertToDateTimeFromString(todate);
            } 
            int rs;
            if ((keyword!="" && keyword!=null) && ((int.TryParse(keyword.Substring(0, 1), out rs) || (keyword.Length > 1 && Char.IsLetter(keyword, 0) && int.TryParse(keyword.Substring(1, 1), out rs)))))
            {                
                var p1 = (from q in db.HoSoes
                          where q.cmthochieu.Contains(keyword) && q.thoigianvipham >= fdate && q.thoigianvipham <= tdate
                         orderby q.cmthochieu
                         select new
                         {
                             q.hoten,
                             q.id,
                             q.cmthochieu,
                             q.choohientai,
                             q.hotencha,
                             q.hotenme,
                             q.tienantiensu,
                             q.hanhvivipham,
                             q.loaidongvat,
                             q.soluongchitiet,
                             q.thoigianvipham
                         }).OrderBy(o=>o.hoten).ThenBy(o=>o.thoigianvipham);
                return JsonConvert.SerializeObject(p1.ToList());
            }

            var p2 = (from q in db.HoSoes
                      where q.hoten.Contains(keyword) && q.thoigianvipham >= fdate && q.thoigianvipham <= tdate
                     orderby q.hoten
                     select new
                     {
                         q.hoten,
                         q.id,
                         q.cmthochieu,
                         q.choohientai,
                         q.hotencha,
                         q.hotenme,
                         q.tienantiensu,
                         q.hanhvivipham,
                         q.loaidongvat,
                         q.soluongchitiet,
                         q.thoigianvipham
                     }).OrderBy(o => o.hoten).ThenBy(o => o.thoigianvipham); ;
            return JsonConvert.SerializeObject(p2.ToList());
        }

        public string getHTVPDetails(string keyword, string fromdate, string todate)
        {
            DateTime fdate = DateTime.Now.AddDays(-1000);
            DateTime tdate = DateTime.Now.AddDays(1000);
            if (fromdate != "")
            {
                fdate = Config.convertToDateTimeFromString(fromdate);
            }
            if (todate != "")
            {
                tdate = Config.convertToDateTimeFromString(todate);
            } 
            var p = (from q in db.HoSoes
                     where q.hinhthucvipham.Contains(keyword) && q.thoigianvipham >= fdate && q.thoigianvipham <= tdate
                     orderby q.loaidongvat
                     select new
                     {
                         q.id,
                         q.hinhthucvipham,
                         q.hanhvivipham,
                         q.loaidongvat,
                         diaban = q.quanvipham + "/" + q.tinhvipham,
                         q.motavipham,
                         q.soluongchitiet,
                         q.tendonvibatgiu,
                         q.phuongthucvanchuyen,
                         q.tuyenduongvanchuyen,
                         q.thoigianvipham
                     }).OrderBy(o=>o.hinhthucvipham).ThenBy(o=>o.thoigianvipham);
            return JsonConvert.SerializeObject(p.ToList());
        }

        public string getHanhViVPDetails(string keyword, string fromdate, string todate)
        {
            DateTime fdate = DateTime.Now.AddDays(-1000);
            DateTime tdate = DateTime.Now.AddDays(1000);
            if (fromdate != "")
            {
                fdate = Config.convertToDateTimeFromString(fromdate);
            }
            if (todate != "")
            {
                tdate = Config.convertToDateTimeFromString(todate);
            } 
            var p = (from q in db.HoSoes
                     where q.hanhvivipham.Contains(keyword) && q.thoigianvipham >= fdate && q.thoigianvipham <= tdate
                     orderby q.loaidongvat
                     select new
                     {
                         q.id,
                         q.hanhvivipham,
                         q.hinhthucvipham,
                         q.loaidongvat,
                         diaban = q.quanvipham + "/" + q.tinhvipham,
                         q.motavipham,
                         q.soluongchitiet,
                         q.tendonvibatgiu,
                         q.phuongthucvanchuyen,
                         q.tuyenduongvanchuyen,
                         q.thoigianvipham
                     }).OrderBy(o=>o.hanhvivipham).ThenBy(o=>o.thoigianvipham);
            return JsonConvert.SerializeObject(p.ToList());
        }

        public string getHanhViVP(string keyword)
        {
            var p = (from q in db.HoSoes where q.hanhvivipham.Contains(keyword) orderby q.hanhvivipham select q.hanhvivipham).Distinct().Take(10);
            return JsonConvert.SerializeObject(p.ToList());
        }        

        public static string getLoaiContentPDF(string keyword)
        {
            var records = (from q in db.HoSoes where q.loaidongvat.Contains(keyword) orderby q.loaidongvat select new { q.loaidongvat, q.thoigianvipham, q.tinhvipham,q.quanvipham, q.hoten, q.cmthochieu, q.hanhvivipham }).ToList();
            var content = "";
            foreach (var q in records) {
                content += q.loaidongvat + " " + " " + q.thoigianvipham + " " + q.tinhvipham + " " + q.quanvipham + " " + q.hoten + " " + q.cmthochieu + " " + q.hanhvivipham + "\n";
            }
            return content;
        }        

        //public string getTinhDetails(string keyword)
        //{
        //    var p = (from q in db.HoSoes where q.tinhvipham.Contains(keyword) orderby q.tinhvipham select new { q.loaidongvat, q.thoigianvipham, q.tinhvipham, q.quanvipham, q.hoten, q.cmthochieu, q.hanhvivipham });
        //    return JsonConvert.SerializeObject(p.ToList());
        //}
        
        public string searchQuanHuyen(string keyword)
        {
            var p = (from q in db.TinhThanhs where q.quanhuyen.Contains(keyword) || q.tinhthanhpho.Contains(keyword) orderby q.tinhthanhpho, q.quanhuyen select new { q.quanhuyen, q.tinhthanhpho }).Distinct().Take(10).ToList();
            var listRs = new List<string>();
            foreach (var item in p) {
                listRs.Add(item.quanhuyen + "/" + item.tinhthanhpho);
            }
            return JsonConvert.SerializeObject(listRs);
        }        

        public static string getQuanHuyenContentPDF(string keyword)
        {
            var content = "";
            if (keyword.Contains("/"))
            {
                var quanTinh = keyword.Trim().Split('/');
                var quan = quanTinh[0].Trim();
                var tinh = quanTinh[1].Trim();
                var p1 = (from q in db.HoSoes where q.quanvipham.Contains(quan) && q.tinhvipham.Contains(tinh) orderby q.quanvipham select new { q.loaidongvat, q.thoigianvipham, q.tinhvipham, q.quanvipham, q.hoten, q.cmthochieu, q.hanhvivipham }).ToList();
                foreach (var q in p1)
                {
                    content += q.loaidongvat + " " + " " + q.thoigianvipham + " " + q.tinhvipham + " " + q.quanvipham + " " + q.hoten + " " + q.cmthochieu + " " + q.hanhvivipham + "\n";
                }
                return content;
            }
            var p2 = (from q in db.HoSoes where q.quanvipham.Contains(keyword) || q.tinhvipham.Contains(keyword) orderby q.quanvipham select new { q.loaidongvat, q.thoigianvipham, q.tinhvipham, q.quanvipham, q.hoten, q.cmthochieu, q.hanhvivipham });
            foreach (var q in p2)
            {
                content += q.loaidongvat + " " + " " + q.thoigianvipham + " " + q.tinhvipham + " " + q.quanvipham + " " + q.hoten + " " + q.cmthochieu + " " + q.hanhvivipham + "\n";
            }
            return content;
        }

        public string getCMT(string keyword)
        {
            int rs;
            if (int.TryParse(keyword.Substring(0, 1), out rs) || (keyword.Length > 1 && Char.IsLetter(keyword, 0) && int.TryParse(keyword.Substring(1, 1), out rs)))
            {
                var p1 = (from q in db.HoSoes where q.cmthochieu.Contains(keyword) orderby q.cmthochieu select q.cmthochieu ).Distinct().Take(10).ToList();
                return JsonConvert.SerializeObject(p1);
            }
            var p2 = (from q in db.HoSoes where q.hoten.Contains(keyword) orderby q.hoten select q.hoten).Distinct().Take(10).ToList();            
            return JsonConvert.SerializeObject(p2);
        }

        public static string getDoiTuongContentPdf(string keyword)
        {
            var content = "";
            var records = (from q in db.HoSoes where q.cmthochieu.Contains(keyword) orderby q.cmthochieu select new { q.loaidongvat, q.thoigianvipham, q.tinhvipham, q.quanvipham, q.hoten, q.cmthochieu, q.hanhvivipham });
            foreach (var q in records)
            {
                content += q.loaidongvat + " " + " " + q.thoigianvipham + " " + q.tinhvipham + " " + q.quanvipham + " " + q.hoten + " " + q.cmthochieu + " " + q.hanhvivipham + "\n";
            }
            return content;
        }

        #endregion

        public string getQuanHuyen(string keyword) {
            var p = (from q in db.TinhThanhs where q.quanhuyen.Contains(keyword) orderby q.quanhuyen select q.quanhuyen).Distinct().Take(10);
            return JsonConvert.SerializeObject(p.ToList());
        }
        public string getTinhThanh(string keyword)
        {
            var p = (from q in db.TinhThanhs where q.tinhthanhpho.Contains(keyword) orderby q.tinhthanhpho select q.tinhthanhpho).Distinct().Take(10);
            return JsonConvert.SerializeObject(p.ToList());
        }
        public string getAllTinhThanh(string keyword)
        {
            var p = (from q in db.TinhThanhs where q.tinhthanhpho.Contains(keyword) orderby q.tinhthanhpho select q.tinhthanhpho).Distinct();
            return JsonConvert.SerializeObject(p.ToList());
        }
        public string getLoaiDVHD(string keyword)
        {
            var p = (from q in db.DsDvhds where q.ten.Contains(keyword) select q.ten).Take(10);
            return JsonConvert.SerializeObject(p.ToList());
        }
        public ActionResult Search(int? page, string keyword)
        {
            ViewBag.keyword = keyword;
            var p = (from q in db.HoSoes where q.hoten.Contains(keyword) || q.cmthochieu.Contains(keyword) || q.hokhauthuongtru.Contains(keyword) || q.noicap.Contains(keyword) || q.nghenghiep.Contains(keyword) || q.hinhthucvipham.Contains(keyword) || q.hanhvivipham.Contains(keyword) || q.quanvipham.Contains(keyword) || q.tinhvipham.Contains(keyword) || q.tuyenduongvanchuyen.Contains(keyword) || q.loaidongvat.Contains(keyword) || q.soluongchitiet.Contains(keyword) || q.donvibatgiu.Contains(keyword) || q.ketquaxuly.Contains(keyword) || q.ketquaxulychitiet.Contains(keyword) || q.tendonvixuly.Contains(keyword) || q.hotencanboxuly.Contains(keyword) || q.ketquaxuly.Contains(keyword) || q.ketquaxulychitiet.Contains(keyword) select q).OrderByDescending(o => o.id).Take(1000);
            int pageSize = Config.PageSize;
            int pageNumber = (page ?? 1);
            ViewBag.page = page;
            return View(p.ToPagedList(pageNumber, pageSize));
        }
    }
}
