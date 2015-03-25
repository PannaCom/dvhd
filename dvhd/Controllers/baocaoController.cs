﻿using iTextSharp.text;
using iTextSharp.text.html.simpleparser;
using iTextSharp.text.pdf;
using System;
using System.Collections.Generic;
using System.IO;
using System.Web.Mvc;
using dvhd.Models;
using System.Linq;
using System.Globalization;

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
            if (Config.getCookie("logged") == "") return RedirectToAction("Login", "Home");
            if (!Config.checkPermission(Config.getCookie("logged"), "BC2")) return RedirectToAction("Permission", "Home");
            return View();
        }

        //
        // GET: /BaoCaoTheoTinh/
        public ActionResult BaoCaoTheoDoiTuong()
        {
            if (Config.getCookie("logged") == "") return RedirectToAction("Login", "Home");
            if (!Config.checkPermission(Config.getCookie("logged"), "BC3")) return RedirectToAction("Permission", "Home");
            return View();
        }
        
        // GET: /BaoCaoTongHop/
        public ActionResult BaoCaoTongHop()
        {
            if (Config.getCookie("logged") == "") return RedirectToAction("Login", "Home");
            if (!Config.checkPermission(Config.getCookie("logged"), "BC9")) return RedirectToAction("Permission", "Home");
            return View();
        }

        // GET: /BaoCaoHinhThuc/
        public ActionResult BaoCaoHinhThuc(string type)
        {
            if (Config.getCookie("logged") == "") return RedirectToAction("Login", "Home");
            if (!Config.checkPermission(Config.getCookie("logged"), "BC4")) return RedirectToAction("Permission", "Home");
            if (type != null && type != string.Empty)
            {
                var p = (from q in db.HoSoes
                         where q.hinhthucvipham.Contains(type)
                         orderby q.thoigianvipham
                         select q).ToList();
                return View(p);
            }
            return View();
        }

        // GET: /BaoCaoHanhVi/
        public ActionResult BaoCaoHanhVi()
        {
            if (Config.getCookie("logged") == "") return RedirectToAction("Login", "Home");
            if (!Config.checkPermission(Config.getCookie("logged"), "BC5")) return RedirectToAction("Permission", "Home");
            return View();
        }

        public ActionResult xuly(string type)
        {
            if (Config.getCookie("logged") == "") return RedirectToAction("Login", "Home");
            if (type.Equals(Config.ketquaxuly[1]) && !Config.checkPermission(Config.getCookie("logged"), "BC6")) return RedirectToAction("Permission", "Home");
            if (type.Equals(Config.ketquaxuly[2]) && !Config.checkPermission(Config.getCookie("logged"), "BC7")) return RedirectToAction("Permission", "Home");
            if (type.Equals(Config.ketquaxuly[3]) && !Config.checkPermission(Config.getCookie("logged"), "BC8")) return RedirectToAction("Permission", "Home");
            if (type != null && type != string.Empty)
            {
                var p = (from q in db.HoSoes
                         where q.ketquaxuly.Contains(type)
                         orderby q.ngayxuly
                         select q).ToList();                
                return View(p);
            }
            return View();
        }
                     
        [ValidateInput(false)]
        public void ExportExcel(string keyword, int type)
        {  
            System.Web.HttpContext.Current.Response.ContentType = "application/force-download";            
            System.Web.HttpContext.Current.Response.Write("<html xmlns:x=\"urn:schemas-microsoft-com:office:excel\">");
            System.Web.HttpContext.Current.Response.Write("<head>");
            System.Web.HttpContext.Current.Response.Write("<META http-equiv=\"Content-Type\" content=\"text/html; charset=utf-8\">");
            System.Web.HttpContext.Current.Response.Write("<!--[if gte mso 9]><xml>");
            System.Web.HttpContext.Current.Response.Write("<x:ExcelWorkbook>");
            System.Web.HttpContext.Current.Response.Write("<x:ExcelWorksheets>");
            System.Web.HttpContext.Current.Response.Write("<x:ExcelWorksheet>");
            System.Web.HttpContext.Current.Response.Write("<x:Name>Report Data</x:Name>");
            System.Web.HttpContext.Current.Response.Write("<x:WorksheetOptions>");
            System.Web.HttpContext.Current.Response.Write("<x:Print>");
            System.Web.HttpContext.Current.Response.Write("<x:ValidPrinterInfo/>");
            System.Web.HttpContext.Current.Response.Write("</x:Print>");
            System.Web.HttpContext.Current.Response.Write("</x:WorksheetOptions>");
            System.Web.HttpContext.Current.Response.Write("</x:ExcelWorksheet>");
            System.Web.HttpContext.Current.Response.Write("</x:ExcelWorksheets>");
            System.Web.HttpContext.Current.Response.Write("</x:ExcelWorkbook>");
            System.Web.HttpContext.Current.Response.Write("</xml>");
            System.Web.HttpContext.Current.Response.Write("<![endif]--> ");            
            System.Web.HttpContext.Current.Response.Write("</head>");

            string htmlContent = "";
            var filename = "";
            switch (type)
            {
                case 1:
                    filename = "Báo Cáo Theo Loài " + keyword + " " + DateTime.Now.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture);                    
                    htmlContent = getLoaiData(keyword);
                    break;
                case 2: 
                    filename = "Báo Cáo Theo Địa Bàn " + keyword + " " + DateTime.Now.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture);                    
                    htmlContent = getDiaBanData(keyword);
                    break;
                case 3: 
                    filename = "Báo Cáo Theo Đối Tượng " + keyword + " " + DateTime.Now.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture);                    
                    htmlContent = getDoiTuongData(keyword);
                    break;
                case 4: 
                    filename = "Báo Cáo Theo Hình Thức Vi Phạm " + keyword + " " + DateTime.Now.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture);                    
                    htmlContent = getHinhThucData(keyword);
                    break;
                case 5: 
                    filename = "Báo Cáo Theo Hành Vi Vi Phạm " + keyword + " " + DateTime.Now.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture);
                    htmlContent = getHanhViData(keyword);
                    break;
                case 6: 
                    filename = "Báo Cáo Theo Kết Quả Xử Lý " + keyword + " " + DateTime.Now.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture);
                    htmlContent = getXuLyData(keyword);                    
                    break;                
            }

            System.Web.HttpContext.Current.Response.AddHeader("content-disposition", "attachment; filename=" + filename + ".xls");
            System.Web.HttpContext.Current.Response.Write("<b>" + filename + "</b>");
            System.Web.HttpContext.Current.Response.Write(htmlContent);
            System.Web.HttpContext.Current.Response.Flush();
           
        }

        public string getXuLyData(string keyword)
        {
            var p = (from q in db.HoSoes
                     where q.ketquaxuly.Contains(keyword)
                     orderby q.ngayxuly
                     select q).ToList();                
            var htmlContent = "<table><tr><th style=\"width:auto;\">Stt</th><th style=\"width:auto;\">Kết Quả Xử Lý</th><th style=\"width:auto;\">Kết Quả Xử Lý Chi Tiết</th>"
                + "<th style=\"width:auto;\">Tên Đơn Vị Xử Lý</th><th style=\"width:auto;\">Bị Can</th><th style=\"width:auto;\">Mô Tả Chi Tiết Xử Lý</th>"
                + "<th style=\"width:auto;\">Mô Tả Chi Tiết Vi Phạm</th><th style=\"width:auto;\">Hành Vi Vi Phạm</th>"
                + "<th style=\"width:auto;\">Ngày Xử Lý</th><th style=\"width:auto;\">Cán Bộ Xử Lý</th><th style=\"width:auto;\">Cấp Bậc</th></tr>";
            for (int i = 0; i < p.Count; i++)
            {
                htmlContent += "<tr><td >" + (i + 1) + "</td><td style=\"width:auto;\">" + p[i].ketquaxuly + "</td><td style=\"width:auto;\">"
                    + p[i].ketquaxulychitiet + "</td><td style=\"width:auto;\">";
                htmlContent += p[i].tendonvixuly + "</td><td style=\"width:auto;\">" + p[i].hoten + "</td><td style=\"width:auto;\">"
                    + p[i].motachitietxuly + "</td><td style=\"width:auto;\">";
                htmlContent += p[i].motavipham + "</td><td style=\"width:auto;\">" + p[i].hanhvivipham + "</td><td style=\"width:auto;\">";
                htmlContent += (p[i].ngayxuly.HasValue == true ? p[i].ngayxuly.Value.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture) : string.Empty) 
                    + "</td><td style=\"width:auto;\">" + p[i].hotencanboxuly + "</td><td style=\"width:auto;\">" + p[i].capbac + "</td></tr>";                
            }
            htmlContent += "</table>";
            return htmlContent;
        }

        public string getHanhViData(string keyword)
        {
            var p = (from q in db.HoSoes
                     where q.hanhvivipham.Contains(keyword)
                     orderby q.loaidongvat
                     select new
                     {
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
                     }).ToList();
            var htmlContent = "<table><tr><th style=\"width:auto;\">Stt</th><th style=\"width:auto;\">Hành Vi Vi Phạm</th><th style=\"width:auto;\">Hình Thức Vi Phạm</th>"
                + "<th style=\"width:auto;\">Loại Vi Phạm</th><th style=\"width:auto;\">Địa Điểm Vi Phạm</th>"
                + "<th style=\"width:auto;\">Mô Tả Chi Tiết</th><th style=\"width:auto;\">Số Lượng Chi Tiết</th><th style=\"width:auto;\">Tên Đơn Vị Bắt Giữ</th>"
                + "<th style=\"width:auto;\">Phương Thức Vận Chuyển</th><th style=\"width:auto;\">Tuyến Đường Vận Chuyển</th><th style=\"width:auto;\">Ngày Vi Phạm</th></tr>";
            for (int i = 0; i < p.Count; i++)
            {
                htmlContent += "<tr><td >" + (i + 1) + "</td><td style=\"width:auto;\">" + p[i].hanhvivipham + "</td><td style=\"width:auto;\">"
                    + p[i].hinhthucvipham + "</td><td style=\"width:auto;\">";
                htmlContent += p[i].loaidongvat + "</td><td style=\"width:auto;\">" + p[i].diaban + "</td><td style=\"width:auto;\">"
                    + p[i].motavipham + "</td><td style=\"width:auto;\">";
                htmlContent += p[i].soluongchitiet + "</td><td style=\"width:auto;\">" + p[i].tendonvibatgiu + "</td><td style=\"width:auto;\">";
                htmlContent += p[i].phuongthucvanchuyen + "</td><td style=\"width:auto;\">" + p[i].tuyenduongvanchuyen + "</td><td style=\"width:auto;\">";
                htmlContent += (p[i].thoigianvipham.HasValue == true ? p[i].thoigianvipham.Value.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture) : string.Empty)
                    + "</td></tr>";
            }
            htmlContent += "</table>";
            return htmlContent;
        }

        public string getHinhThucData(string keyword) {
            var p = (from q in db.HoSoes
                     where q.hinhthucvipham.Contains(keyword)
                     orderby q.loaidongvat
                     select new
                     {
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
                     }).ToList();
            var htmlContent = "<table><tr><th style=\"width:auto;\">Stt</th><th style=\"width:auto;\">Hình Thức Vi Phạm</th><th style=\"width:auto;\">Hành Vi Vi Phạm</th>"
                + "<th style=\"width:auto;\">Loại Vi Phạm</th><th style=\"width:auto;\">Địa Điểm Vi Phạm</th>"
                + "<th style=\"width:auto;\">Mô Tả Chi Tiết</th><th style=\"width:auto;\">Số Lượng Chi Tiết</th><th style=\"width:auto;\">Tên Đơn Vị Bắt Giữ</th>"
                + "<th style=\"width:auto;\">Phương Thức Vận Chuyển</th><th style=\"width:auto;\">Tuyến Đường Vận Chuyển</th><th style=\"width:auto;\">Ngày Vi Phạm</th></tr>";
            for (int i = 0; i < p.Count; i++)
            {
                htmlContent += "<tr><td >" + (i + 1) + "</td><td style=\"width:auto;\">" + p[i].hinhthucvipham + "</td><td style=\"width:auto;\">"
                    + p[i].hanhvivipham + "</td><td style=\"width:auto;\">";
                htmlContent += p[i].loaidongvat + "</td><td style=\"width:auto;\">" + p[i].diaban + "</td><td style=\"width:auto;\">"
                    + p[i].motavipham + "</td><td style=\"width:auto;\">";
                htmlContent += p[i].soluongchitiet + "</td><td style=\"width:auto;\">" + p[i].tendonvibatgiu + "</td><td style=\"width:auto;\">";
                htmlContent += p[i].phuongthucvanchuyen + "</td><td style=\"width:auto;\">" + p[i].tuyenduongvanchuyen + "</td><td style=\"width:auto;\">";
                htmlContent += (p[i].thoigianvipham.HasValue == true ? p[i].thoigianvipham.Value.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture) : string.Empty)
                    + "</td></tr>";
            }
            htmlContent += "</table>";
            return htmlContent;
        }

        public string getDoiTuongData(string keyword)
        {
            var htmlContent = "<table><tr><th style=\"width:auto;\">Stt</th><th style=\"width:auto;\">Đối Tượng Vi Phạm</th><th style=\"width:auto;\">Số CMT/Hộ Chiếu</th>"
                    + "<th style=\"width:auto;\">Địa Chỉ Thường Trú</th><th style=\"width:auto;\">Thông Tin Thân Nhân</th>"
                    + "<th style=\"width:auto;\">Tiền Án Tiền Sự</th><th style=\"width:auto;\">Hành Vi Vi Phạm</th>"
                    + "<th style=\"width:auto;\">Tên Loài</th><th style=\"width:auto;\">Số Lượng Chi Tiết</th><th style=\"width:auto;\">Ngày Vi Phạm</th></tr>";
            int rs;
            if (int.TryParse(keyword.Substring(0, 1), out rs) || (keyword.Length > 1 && Char.IsLetter(keyword, 0) && int.TryParse(keyword.Substring(1, 1), out rs)))
            {
                var p = (from q in db.HoSoes
                          where q.cmthochieu.Contains(keyword)
                          orderby q.cmthochieu
                          select new
                          {
                              q.hoten,
                              q.cmthochieu,
                              q.choohientai,
                              q.hotencha,
                              q.hotenme,
                              q.tienantiensu,
                              q.hanhvivipham,
                              q.loaidongvat,
                              q.soluongchitiet,
                              q.thoigianvipham
                          }).ToList();
                for (int i = 0; i < p.Count; i++)
                {
                    htmlContent += "<tr><td >" + (i + 1) + "</td><td style=\"width:auto;\">" + p[i].hoten + "</td><td style=\"width:auto;\">"
                        + p[i].cmthochieu + "</td><td style=\"width:auto;\">";
                    htmlContent += p[i].choohientai + "</td><td style=\"width:auto;\">" + p[i].hotencha + "</td><td style=\"width:auto;\">"
                        + p[i].hotenme + "</td><td style=\"width:auto;\">";
                    htmlContent += p[i].tienantiensu + "</td><td style=\"width:auto;\">" + p[i].hanhvivipham + "</td><td style=\"width:auto;\">";
                    htmlContent += p[i].loaidongvat + "</td><td style=\"width:auto;\">" + p[i].soluongchitiet + "</td><td style=\"width:auto;\">";
                    htmlContent += (p[i].thoigianvipham.HasValue == true ? p[i].thoigianvipham.Value.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture) : string.Empty) 
                        + "</td></tr>";
                }
                htmlContent += "</table>";
                return htmlContent;
            }

            var p2 = (from q in db.HoSoes
                      where q.hoten.Contains(keyword)
                      orderby q.hoten
                      select new
                      {
                          q.hoten,
                          q.cmthochieu,
                          q.choohientai,
                          q.hotencha,
                          q.hotenme,
                          q.tienantiensu,
                          q.hanhvivipham,
                          q.loaidongvat,
                          q.soluongchitiet,
                          q.thoigianvipham
                      }).ToList();
            for (int i = 0; i < p2.Count; i++)
            {
                htmlContent += "<tr><td >" + (i + 1) + "</td><td style=\"width:auto;\">" + p2[i].hoten + "</td><td style=\"width:auto;\">"
                    + p2[i].cmthochieu + "</td><td style=\"width:auto;\">";
                htmlContent += p2[i].choohientai + "</td><td style=\"width:auto;\">" + p2[i].hotencha + "</td><td style=\"width:auto;\">"
                    + p2[i].hotenme + "</td><td style=\"width:auto;\">";
                htmlContent += p2[i].tienantiensu + "</td><td style=\"width:auto;\">" + p2[i].hanhvivipham + "</td><td style=\"width:auto;\">";
                htmlContent += p2[i].loaidongvat + "</td><td style=\"width:auto;\">" + p2[i].soluongchitiet + "</td><td style=\"width:auto;\">";
                htmlContent += (p2[i].thoigianvipham.HasValue == true ? p2[i].thoigianvipham.Value.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture) : string.Empty)
                    + "</td></tr>";
            }
            htmlContent += "</table>";
            return htmlContent;
        }

        public string getDiaBanData(string keyword) {
            var htmlContent = "<table><tr><th style=\"width:auto;\">Stt</th><th style=\"width:auto;\">Tên Địa Bàn</th>"
                    + "<th style=\"width:auto;\">Đối Tượng Vi Phạm</th><th style=\"width:auto;\">Địa Chỉ Thường Trú</th>"
                    + "<th style=\"width:auto;\">Hành Vi Vi Phạm</th><th style=\"width:auto;\">Tên Loài</th>"
                    + "<th style=\"width:auto;\">Số Lượng Chi Tiết</th><th style=\"width:auto;\">Tên Đơn Vị Bắt Giữ</th>"
                    + "<th style=\"width:auto;\">Ngày Vi Phạm</th></tr>";
            if (keyword.Contains("/"))
            {
                var quanTinh = keyword.Trim().Split('/');
                var quan = quanTinh[0].Trim();
                var tinh = quanTinh[1].Trim();
                var p = (from q in db.HoSoes
                          where q.quanvipham.Contains(quan) && q.tinhvipham.Contains(tinh)
                          orderby q.tinhvipham, q.quanvipham
                          select new
                          {
                              diaban = q.quanvipham + "/" + q.tinhvipham,
                              q.hoten,
                              q.choohientai,
                              q.hanhvivipham,
                              q.loaidongvat,
                              q.soluongchitiet,
                              q.tendonvibatgiu,
                              q.thoigianvipham
                          }).ToList();                
                for (int i = 0; i < p.Count; i++)
                {
                    htmlContent += "<tr><td >" + (i + 1) + "</td><td style=\"width:auto;\">" + p[i].diaban + "</td><td style=\"width:auto;\">" 
                        + p[i].hoten + "</td><td style=\"width:auto;\">";
                    htmlContent += p[i].choohientai + "</td><td style=\"width:auto;\">" + p[i].hanhvivipham + "</td><td style=\"width:auto;\">"
                        + p[i].loaidongvat + "</td><td style=\"width:auto;\">";
                    htmlContent += p[i].soluongchitiet + "</td><td style=\"width:auto;\">" + p[i].tendonvibatgiu + "</td><td style=\"width:auto;\">";
                    htmlContent += (p[i].thoigianvipham.HasValue == true ? p[i].thoigianvipham.Value.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture) : string.Empty) + "</td></tr>";
                }
                htmlContent += "</table>";
                return htmlContent;
            }
            var p2 = (from q in db.HoSoes
                      where q.quanvipham.Contains(keyword) || q.tinhvipham.Contains(keyword)
                      orderby q.tinhvipham, q.quanvipham
                      select new
                      {
                          diaban = q.quanvipham + "/" + q.tinhvipham,
                          q.hoten,
                          q.choohientai,
                          q.hanhvivipham,
                          q.loaidongvat,
                          q.soluongchitiet,
                          q.tendonvibatgiu,
                          q.thoigianvipham
                      }).ToList();
            for (int i = 0; i < p2.Count; i++)
            {
                htmlContent += "<tr><td >" + (i + 1) + "</td><td style=\"width:auto;\">" + p2[i].diaban + "</td><td style=\"width:auto;\">"
                    + p2[i].hoten + "</td><td style=\"width:auto;\">";
                htmlContent += p2[i].choohientai + "</td><td style=\"width:auto;\">" + p2[i].hanhvivipham + "</td><td style=\"width:auto;\">"
                    + p2[i].loaidongvat + "</td><td style=\"width:auto;\">";
                htmlContent += p2[i].soluongchitiet + "</td><td style=\"width:auto;\">" + p2[i].tendonvibatgiu + "</td><td style=\"width:auto;\">";
                htmlContent += (p2[i].thoigianvipham.HasValue == true ? p2[i].thoigianvipham.Value.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture) : string.Empty) + "</td></tr>";
            }
            htmlContent += "</table>";
            return htmlContent;
        }

        public string getLoaiData(string keyword) {
            // lấy dữ liệu từ db
            var p = (from q in db.HoSoes
                     where q.loaidongvat.Contains(keyword)
                     orderby q.loaidongvat
                     select new
                     {
                         q.loaidongvat,
                         q.tinhtrangbaoton,
                         q.donvitinh,
                         q.soluongchitiet,
                         q.trigiatangvat,
                         q.tendonvibatgiu,
                         q.phuongthucvanchuyen,
                         q.tuyenduongvanchuyen,
                         q.thoigianvipham
                     }).ToList();
            // ghi dữ liệu
            var htmlContent = "<table><tr><th style=\"width:auto;\">Stt</th><th style=\"width:auto;\">Tên Loài</th>";
            htmlContent += "<th style=\"width:auto;\">Tình Trạng Bảo Tồn</th><th style=\"width:auto;\">Đơn Vị Tính</th>";
            htmlContent += "<th style=\"width:auto;\">Số Lượng Chi Tiết</th><th style=\"width:auto;\">Trị Giá Tang Vật</th>";
            htmlContent += "<th style=\"width:auto;\">Tên Đơn Vị Bắt Giữ</th><th style=\"width:auto;\">Phương Thức Vận Chuyển</th>";
            htmlContent += "<th style=\"width:auto;\">Tuyến Đường Vận Chuyển</th><th style=\"width:auto;\">Ngày Vi Phạm</th></tr>";
            for (int i = 0; i < p.Count; i++)
            {
                htmlContent += "<tr><td >" + (i + 1) + "</td><td style=\"width:auto;\">" + p[i].loaidongvat + "</td><td style=\"width:auto;\">" + p[i].tinhtrangbaoton + "</td><td style=\"width:auto;\">";
                htmlContent += p[i].donvitinh + "</td><td style=\"width:auto;\">" + p[i].soluongchitiet + "</td><td style=\"width:auto;\">" + p[i].trigiatangvat + "</td><td style=\"width:auto;\">";
                htmlContent += p[i].tendonvibatgiu + "</td><td style=\"width:auto;\">" + p[i].phuongthucvanchuyen + "</td><td style=\"width:auto;\">";
                htmlContent += p[i].tuyenduongvanchuyen + "</td><td style=\"width:auto;\">"
                    + (p[i].thoigianvipham.HasValue == true ? p[i].thoigianvipham.Value.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture) : string.Empty) + "</td></tr>";
            }
            htmlContent += "</table>";
            return htmlContent;
        }

        public void Downloaded(string path,string filename)
        {
            string strFilePath = path + filename;

            if (!System.IO.File.Exists(strFilePath))
            {
                System.IO.File.Create(strFilePath).Close();
            }
            StreamWriter sw = System.IO.File.CreateText(strFilePath);
            sw.WriteLine("");
            sw.WriteLine(sw.NewLine);
            sw.Flush();
            sw.Close();
        }

        private string releaseObject(object obj)
        {
            try
            {
                System.Runtime.InteropServices.Marshal.ReleaseComObject(obj);
                obj = null;
            }
            catch (Exception ex)
            {
                obj = null;
                return "Exception Occured while releasing object " + ex.ToString();
            }
            finally
            {
                GC.Collect();
            }
            return "OK";
        }

        public string baocaotonghopreport(string tinhvipham) {
            try
            {
                //int tongsovu, sovuhanhchinh, sovuhinhsu,;
                int stongsovu = 0, ssovuhanhchinh = 0, ssovuhinhsu = 0, sxulyhanhchinh = 0, sxulyhinhsu = 0, skhongxuly = 0;
                var p = (from q in db.HoSoes where q.tinhvipham.Contains(tinhvipham) select new { tinhvipham = q.tinhvipham, quanvipham = q.quanvipham }).OrderBy(o=>o.tinhvipham).ThenBy(o=>o.quanvipham).Distinct().Take(1000).ToList();
                var content = "<tr><th>Tỉnh thành</th><th>Quận huyện</th><th>Tổng số vụ</th><th>Số vụ vi phạm hành chính</th><th>Số vụ vi phạm hình sự</th><th>Số vụ xử lý hành chính</th><th>Số vụ xử lý hình sự</th><th>Số vụ không xử lý</th><tr>";
                for (int i = 0; i < p.Count; i++) {
                    string qvp = p[i].quanvipham;
                    string hinhthucvipham0 = Config.hinhthucvipham[0];
                    string hinhthucvipham1 = Config.hinhthucvipham[1];
                    string ketquaxuly1 = Config.ketquaxuly[1];
                    string ketquaxuly2 = Config.ketquaxuly[2];
                    string ketquaxuly3 = Config.ketquaxuly[3];
                    int? tongsovu = (int?)db.HoSoes.Count(o => o.quanvipham.Contains(qvp));//.Count(o => o.quanvipham.Contains(p[i].quanvipham));//.Count();//(from c in db.HoSoes where c.quanvipham.Contains(p[i].quanvipham) select c).ToList().Count();
                    stongsovu += (int)tongsovu;
                    int? sovuhanhchinh = (int?)db.HoSoes.Count(o => o.quanvipham.Contains(qvp)  && o.hinhthucvipham.Contains(hinhthucvipham0));//(from c in db.HoSoes where c.quanvipham.Contains(p[i].quanvipham) && c.hinhthucvipham.Contains(Config.hinhthucvipham[0]) select c).ToList().Count();
                    ssovuhanhchinh += (int)sovuhanhchinh;
                    int? sovuhinhsu = (int?)db.HoSoes.Count(o => o.quanvipham.Contains(qvp) && o.hinhthucvipham.Contains(hinhthucvipham1));// (from c in db.HoSoes where c.quanvipham.Contains(p[i].quanvipham) && c.hinhthucvipham.Contains(Config.hinhthucvipham[1]) select c).ToList().Count();
                    ssovuhinhsu += (int)sovuhinhsu;
                    int? xulyhanhchinh = (int?)db.HoSoes.Count(o => o.quanvipham.Contains(qvp) && o.ketquaxuly.Contains(ketquaxuly1));// (from c in db.HoSoes where c.quanvipham.Contains(p[i].quanvipham) && c.ketquaxuly.Contains(Config.ketquaxuly[1]) select c).ToList().Count();
                    sxulyhanhchinh += (int)xulyhanhchinh;
                    int? xulyhinhsu = (int?)db.HoSoes.Count(o => o.quanvipham.Contains(qvp) && o.ketquaxuly.Contains(ketquaxuly2)); ;//(from c in db.HoSoes where c.quanvipham.Contains(p[i].quanvipham) && c.ketquaxuly.Contains(Config.ketquaxuly[2]) select c).ToList().Count();
                    sxulyhinhsu += (int)xulyhinhsu;
                    int? khongxuly = (int?)db.HoSoes.Count(o => o.quanvipham.Contains(qvp) && o.ketquaxuly.Contains(ketquaxuly3));//(from c in db.HoSoes where c.quanvipham.Contains(p[i].quanvipham) && c.ketquaxuly.Contains(Config.ketquaxuly[3]) select c).ToList().Count();
                    skhongxuly += (int)khongxuly;
                    content += "<tr><td>" + p[i].tinhvipham + "</td><td>" + p[i].quanvipham + "</td><td>" + tongsovu + "</td><td>" + sovuhanhchinh + "</td><td>" + sovuhinhsu + "</td><td>" + xulyhanhchinh + "</td><td>" + xulyhinhsu + "</td><td>" + khongxuly + "</td>";
                }
                content += "<tr><td colspan=2 align=right>Tổng:</td><td>" + stongsovu + "</td><td>" + ssovuhanhchinh + "</td><td>" + ssovuhinhsu + "</td><td>" + sxulyhanhchinh + "</td><td>" + sxulyhinhsu + "</td><td>" + skhongxuly + "</td></tr>";
                return content;
            }
            catch (Exception ex) {
                return "0";
            }
        }        
    }
}
