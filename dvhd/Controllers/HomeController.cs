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
