using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using dvhd.Models;
using PagedList;
using System.Security.Cryptography;
namespace dvhd.Controllers
{
    public class usersController : Controller
    {
        private dvhdEntities db = new dvhdEntities();

        //
        // GET: /users/

        public ActionResult Index(int? page)
        {
            if (Config.getCookie("logged") == "") return RedirectToAction("Login", "Home");
            if (!Config.checkPermission(Config.getCookie("logged"), "US0")) return RedirectToAction("Permission", "Home");

            var p = (from q in db.users select q).OrderByDescending(o => o.id).Take(1000);
            int pageSize = Config.PageSize;
            int pageNumber = (page ?? 1);
            ViewBag.page = page;
            return View(p.ToPagedList(pageNumber, pageSize));
        }

        //
        // GET: /users/Details/5

        public ActionResult Details(int id = 0)
        {
            user user = db.users.Find(id);
            if (user == null)
            {
                return HttpNotFound();
            }
            return View(user);
        }

        //
        // GET: /users/Create

        public ActionResult Create()
        {
            if (Config.getCookie("logged") == "") return RedirectToAction("Login", "Home");
            if (!Config.checkPermission(Config.getCookie("logged"), "US1")) return RedirectToAction("Permission", "Home");
            return View();
        }

        //
        // POST: /users/Create

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(user user)
        {
            if (ModelState.IsValid)
            {
                string pass = user.pass;
                MD5 md5Hash = MD5.Create();
                string hash = Config.GetMd5Hash(md5Hash, pass);
                user.pass = hash;
                db.users.Add(user);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(user);
        }
        public string updatePermission(int id, string permission)
        {
            try
            {
                db.Database.ExecuteSqlCommand("update users set permission=N'" + permission + "' where id=" + id);
                return "1";
            }
            catch (Exception ex) {
                return "0";
            }
        }
        [HttpPost]
        public string Login(string name, string pass)
        {
            try
            {
                MD5 md5Hash = MD5.Create();
                pass = Config.GetMd5Hash(md5Hash, pass);
                var p = (from q in db.users where q.name.Contains(name) && q.pass.Contains(pass) select q).FirstOrDefault().permission;
                if (p != null && p != "")
                {
                    //Ghi ra cookie
                    Config.setCookie("logged", p);
                    return "1";
                }
                else
                {
                    return "0";
                }
            }
            catch (Exception ex) {
                return "0";
            }
            //return "0";
        }
        //
        // GET: /users/Edit/5

        public ActionResult Edit(int id = 0)
        {
            if (Config.getCookie("logged") == "") return RedirectToAction("Login", "Home");
            if (!Config.checkPermission(Config.getCookie("logged"), "US2")) return RedirectToAction("Permission", "Home");
            user user = db.users.Find(id);
            if (user == null)
            {
                return HttpNotFound();
            }
            ViewBag.id = id;
            return View(user);
        }

        //
        // POST: /users/Edit/5

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(user user)
        {
            if (ModelState.IsValid)
            {
                string pass = user.pass;
                MD5 md5Hash = MD5.Create();
                string hash = Config.GetMd5Hash(md5Hash, pass);
                user.pass = hash;
                db.Entry(user).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(user);
        }

        //
        // GET: /users/Delete/5

        public ActionResult Delete(int id = 0)
        {
            if (Config.getCookie("logged") == "") return RedirectToAction("Login", "Home");
            if (!Config.checkPermission(Config.getCookie("logged"), "US3")) return RedirectToAction("Permission", "Home");
            user user = db.users.Find(id);
            if (user == null)
            {
                return HttpNotFound();
            }
            return View(user);
        }

        //
        // POST: /users/Delete/5

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            user user = db.users.Find(id);
            db.users.Remove(user);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }
}