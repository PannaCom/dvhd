using System;
using System.Collections.Generic;
using System.Linq;
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
            //ViewBag.Message = "Modify this template to jump-start your ASP.NET MVC application.";
            //string list = "";
            //try
            //{
            //    var p1 = (from q in db.banners select q).OrderBy(o => o.no).ThenByDescending(o => o.id).ThenBy(o => o.images).Take(5);
            //    var p = p1.ToList();
            //    for (int i = 0; i < p.Count; i++)
            //    {
            //        if (i <= 0)
            //        {
            //            list += "\"" + p[i].images + "\"";
            //        }
            //        else
            //        {
            //            list += ",\"" + p[i].images + "\"";
            //        }
            //    }

            //}
            //catch (Exception ex)
            //{

            //}
            //ViewBag.slide = list;
            //Tin tuc
            try
            {
                var p2 = (from q in db.News select q).OrderByDescending(o => o.id).Take(4);
                var prs2 = p2.ToList();
                string news = "";
                string link = "";
                for (int j = 0; j < prs2.Count; j++)
                {
                    link = "/News/Details/" + prs2[j].id;
                    ///hotel/" + Config.unicodeToNoMark(prs[j].name) + "-" + ViewBag.fromdate + "-" + ViewBag.todate + "-" + prs[j].id + "
                    news += "<div class=\"col-sm-3 single\" style=\"height:auto;\">";
                    news += " <div ><a href=\"" + link + "\"><img src=\"" + Config.domain+prs2[j].image + "\" alt=\"" + prs2[j].title + "\" style=\"width:250px;height:200px;\" class=\"img-responsive\" /></a>";
                    news += "  <div class=\"mask\">";
                    news += "   <div class=\"main\">";
                    news += "      <a href=\"" + link + "\"><b>" + prs2[j].title + "</b></a>";
                    news += "      <p>" + prs2[j].des + "</p>";
                    news += "    </div>";
                    news += " </div>";
                    news += " </div>";
                    news += " </div>";

                }
                ViewBag.news = news;
            }
            catch (Exception ex2)
            {
            }
            return View();
        }
        public string getListBanner() {
            string list = "";
            try
            {
                var p1 = (from q in db.banners where q.no >= 0 && q.type == 0 select new { images=q.images,no=q.no,id=q.id }).OrderBy(o => o.no).ThenByDescending(o => o.id).ThenBy(o => o.images);
                //var p = p1.ToList();
                //for (int i = 0; i < p.Count; i++)
                //{
                //    if (i <= 0)
                //    {
                //        list += "\"" + p[i].images + "\"";
                //    }
                //    else
                //    {
                //        list += ",\"" + p[i].images + "\"";
                //    }
                //}
                return JsonConvert.SerializeObject(p1.ToList());
            }
            catch (Exception ex)
            {
                return "";
            }
        }
        public ActionResult Login()
        {
            //string list = "";
            //try
            //{
            //    var p1 = (from q in db.banners select q).OrderBy(o => o.no).ThenByDescending(o => o.id).ThenBy(o => o.images).Take(5);
            //    var p = p1.ToList();

            //    for (int i = 0; i < p.Count; i++)
            //    {
            //        if (i <= 0)
            //        {
            //            list += "\"" + p[i].images + "\"";
            //        }
            //        else
            //        {
            //            list += ",\"" + p[i].images + "\"";
            //        }
            //    }

            //}
            //catch (Exception ex)
            //{

            //}
            //ViewBag.slide = list;
            
            return View();
        }
        public ActionResult Logout()
        {
            if (Request.Cookies["logged"] != null)
            {
                Response.Cookies["logged"].Expires = DateTime.Now.AddDays(-1);
            }
            Session.Abandon();
            return RedirectToAction("Login", "Home");
            //return View();
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

        public string getDVBG(string keyword)
        {
            var p = (from q in db.HoSoes where q.tendonvibatgiu.Contains(keyword) select q.tendonvibatgiu).Distinct().Take(10);
            return JsonConvert.SerializeObject(p.ToList());
        }

        public string getDVXL(string keyword)
        {
            var p = (from q in db.HoSoes where q.tendonvixuly.Contains(keyword) select q.tendonvixuly).Distinct().Take(10);
            return JsonConvert.SerializeObject(p.ToList());
        }

        
        public string getDVBGDetails(string keyword, string fromdate, string todate)
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
                     where q.tendonvibatgiu.Contains(keyword) && q.thoigianvipham >= fdate && q.thoigianvipham <= tdate
                     select new
                     {
                         q.id,
                         q.tendonvibatgiu,
                         q.diachilienhe,
                         q.hotencanboxuly,
                         q.capbac,
                         q.hoten,
                         q.loaidongvat,
                         q.soluongchitiet,
                         q.thoigianvipham
                     }).OrderBy(o => o.tendonvibatgiu).ThenBy(o => o.thoigianvipham);
            return JsonConvert.SerializeObject(p.ToList());
        }

        public string getDVXLDetails(string keyword, string fromdate, string todate)
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
                     where q.tendonvixuly.Contains(keyword) && q.thoigianvipham >= fdate && q.thoigianvipham <= tdate
                     select new
                     {
                         q.id,
                         q.tendonvixuly,
                         q.hoten,
                         q.hanhvivipham,
                         diadiem = q.quanvipham + "/" + q.tinhvipham,
                         q.loaidongvat,
                         q.soluongchitiet,
                         q.thoigianvipham
                     }).OrderBy(o => o.tendonvixuly).ThenBy(o => o.thoigianvipham);
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
            var p = (from q in db.HoSoes where q.hoten.Contains(keyword) || q.cmthochieu.Contains(keyword) || q.hokhauthuongtru.Contains(keyword) 
                         || q.noicap.Contains(keyword) || q.nghenghiep.Contains(keyword) || q.hinhthucvipham.Contains(keyword) || q.hanhvivipham.Contains(keyword) 
                         || q.quanvipham.Contains(keyword) || q.tinhvipham.Contains(keyword) || q.tuyenduongvanchuyen.Contains(keyword) || q.loaidongvat.Contains(keyword) 
                         || q.soluongchitiet.Contains(keyword) || q.donvibatgiu.Contains(keyword) || q.ketquaxuly.Contains(keyword) || q.ketquaxulychitiet.Contains(keyword) 
                         || q.tendonvixuly.Contains(keyword) || q.hotencanboxuly.Contains(keyword) select q).OrderByDescending(o => o.id).Take(1000);
            int pageSize = Config.PageSize;
            int pageNumber = (page ?? 1);
            ViewBag.page = page;
            return View(p.ToPagedList(pageNumber, pageSize));
        }
        public ActionResult Help() {
            return View();
        }
    }
}
