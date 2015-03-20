using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using dvhd.Models;
using Newtonsoft.Json;
namespace dvhd.Controllers
{
    public class HomeController : Controller
    {
        private dvhdEntities db = new dvhdEntities();
        public ActionResult Index()
        {
            ViewBag.Message = "Modify this template to jump-start your ASP.NET MVC application.";

            return View();
        }
        public ActionResult Login()
        {
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
        
        public string getLoai(string keyword)
        {
            var p = (from q in db.HoSoes where q.loaidongvat.Contains(keyword) orderby q.loaidongvat select q.loaidongvat).Distinct().Take(10);
            return JsonConvert.SerializeObject(p.ToList());
        }

        public string getLoaiDetails(string keyword)
        {
            var p = (from q in db.HoSoes where q.loaidongvat.Contains(keyword) orderby q.loaidongvat select new { q.loaidongvat, q.thoigianvipham, q.tinhvipham, q.hoten, q.cmthochieu, q.hanhvivipham });
            return JsonConvert.SerializeObject(p.ToList());
        }


        public string getTinh(string keyword)
        {
            var p = (from q in db.HoSoes where q.tinhvipham.Contains(keyword) orderby q.tinhvipham select q.tinhvipham).Distinct().Take(10);
            return JsonConvert.SerializeObject(p.ToList());
        }

        public string getTinhDetails(string keyword)
        {
            var p = (from q in db.HoSoes where q.tinhvipham.Contains(keyword) orderby q.tinhvipham select new { q.loaidongvat, q.thoigianvipham, q.tinhvipham, q.hoten, q.cmthochieu, q.hanhvivipham });
            return JsonConvert.SerializeObject(p.ToList());
        }

        public string getCMT(string keyword)
        {
            var p = (from q in db.HoSoes where q.cmthochieu.Contains(keyword) orderby q.cmthochieu select q.cmthochieu).Distinct().Take(10);
            return JsonConvert.SerializeObject(p.ToList());
        }

        public string getCMTDetails(string keyword)
        {
            var p = (from q in db.HoSoes where q.cmthochieu.Contains(keyword) orderby q.cmthochieu select new { q.loaidongvat, q.thoigianvipham, q.tinhvipham, q.hoten, q.cmthochieu, q.hanhvivipham });
            return JsonConvert.SerializeObject(p.ToList());
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
        public string getLoaiDVHD(string keyword)
        {
            var p = (from q in db.DsDvhds where q.ten.Contains(keyword) select q.ten).Take(10);
            return JsonConvert.SerializeObject(p.ToList());
        }
    }
}
