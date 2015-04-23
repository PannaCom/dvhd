using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using dvhd.Models;
namespace dvhd.Controllers
{
    public class AdminController : Controller
    {
        //
        // GET: /Admin/
        private dvhdEntities db = new dvhdEntities();
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult BackUp()
        {
            if (Config.getCookie("logged") == "") return RedirectToAction("Login", "Admin");
            
            return View();
        }
        public string backupdb()
        {
            string file = Guid.NewGuid().ToString();
            //string dbname = "dvhd";
            try
            {
                
                var dbPath = @"C:\Wlc\wlc"+file+".bak.rar";

                using (var data = new dvhdEntities())
                {
                    var cmd = String.Format("BACKUP DATABASE {0} TO DISK='{1}' WITH FORMAT, MEDIANAME='Wlc', MEDIADESCRIPTION='Media set for {0} database';"
                        , "Wlc", dbPath);
                    db.Database.ExecuteSqlCommand(cmd);
                }
            }
            catch (Exception ex)
            {
                return "0";
            }
            return file;
        }
    }
}
