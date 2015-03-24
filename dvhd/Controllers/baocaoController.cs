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
//using Excel = Microsoft.Office.Interop.Excel;

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

        public string exportExel(string type = "Hành Chính")
        {
            //if (type != null && type != string.Empty)
            //{
            //    var p = (from q in db.HoSoes
            //             where q.ketquaxuly.Contains(type)
            //             orderby q.ngayxuly
            //             select q).ToList();

            //    Excel.Application xlApp ;
            //    Excel.Workbook xlWorkBook ;
            //    Excel.Worksheet xlWorkSheet ;
            //    object misValue = System.Reflection.Missing.Value;

            //    xlApp = new Excel.Application();
            //    xlWorkBook = xlApp.Workbooks.Add(misValue);
            //    xlWorkSheet = (Excel.Worksheet)xlWorkBook.Worksheets.get_Item(1);
            
            //    for (var i = 0; i < p.Count; i++)
            //    {
            //        var row=p[i];                
            //        var j=0;                
            //        xlWorkSheet.Cells[i + 1, ++j] = i+1;
            //        xlWorkSheet.Cells[i + 1, ++j] = row.ketquaxuly;
            //        xlWorkSheet.Cells[i + 1, ++j] = row.ketquaxulychitiet;
            //        xlWorkSheet.Cells[i + 1, ++j] = row.donvixuly;
            //        xlWorkSheet.Cells[i + 1, ++j] = row.tendonvixuly;
            //        xlWorkSheet.Cells[i + 1, ++j] = row.hoten;
            //        xlWorkSheet.Cells[i + 1, ++j] = row.motachitietxuly;
            //        xlWorkSheet.Cells[i + 1, ++j] = row.motavipham;
            //        xlWorkSheet.Cells[i + 1, ++j] = row.hanhvivipham;
            //        xlWorkSheet.Cells[i + 1, ++j] = row.ngayxuly.HasValue == true ? row.ngayxuly.Value.ToString("dd/MM/yyyy", System.Globalization.CultureInfo.InvariantCulture) : "";
            //        xlWorkSheet.Cells[i + 1, ++j] = row.hotencanboxuly;
            //    }

            //    xlWorkBook.SaveAs("csharp.net-informations.xls", Excel.XlFileFormat.xlWorkbookNormal, misValue, misValue, misValue, misValue, Excel.XlSaveAsAccessMode.xlExclusive, misValue, misValue, misValue, misValue, misValue);
            //    xlWorkBook.Close(true, misValue, misValue);
            //    xlApp.Quit();

            //    releaseObject(xlWorkSheet);
            //    releaseObject(xlWorkBook);
            //    releaseObject(xlApp);
            //}
            return "OK";
        }
             
        [ValidateInput(false)]
        public void ExportExcelBaoCaoTheoLoai(string keyword)
        {  
            System.Web.HttpContext.Current.Response.ContentType = "application/force-download";
            System.Web.HttpContext.Current.Response.AddHeader("content-disposition", "attachment; filename=Print.xls");
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
            System.Web.HttpContext.Current.Response.Write("Báo cáo theo loài "+keyword);
            //Ghi ra nội dung báo cáo ở đây
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
            //Ghi ra nội dung chi tiết;
            string htmlContent = "<table ><tr><th style=\"width:auto;\">Stt</th><th style=\"width:auto;\">Tên Loài</th>";
            htmlContent += "<th style=\"width:auto;\">Tình Trạng Bảo Tồn</th><th style=\"width:auto;\">Đơn Vị Tính</th>";
            htmlContent += "<th style=\"width:auto;\">Số Lượng Chi Tiết</th><th style=\"width:auto;\">Trị Giá Tang Vật</th>";
            htmlContent += "<th style=\"width:auto;\">Tên Đơn Vị Bắt Giữ</th><th style=\"width:auto;\">Phương Thức Vận Chuyển</th>";
            htmlContent += "<th style=\"width:auto;\">Tuyến Đường Vận Chuyển</th><th style=\"width:auto;\">Ngày Vi Phạm</th></tr>";
                for(int i=0;i<p.Count;i++){
                    htmlContent += "<tr><td >" + (i + 1) + "</td><td style=\"width:auto;\">" + p[i].loaidongvat + "</td><td style=\"width:auto;\">" + p[i].tinhtrangbaoton + "</td><td style=\"width:auto;\">";
                    htmlContent += p[i].donvitinh + "</td><td style=\"width:auto;\">" + p[i].soluongchitiet + "</td><td style=\"width:auto;\">" + p[i].trigiatangvat + "</td><td style=\"width:auto;\">";
                    htmlContent += p[i].tendonvibatgiu + "</td><td style=\"width:auto;\">" + p[i].phuongthucvanchuyen + "</td><td style=\"width:auto;\">";
                    htmlContent += p[i].tuyenduongvanchuyen + "</td><td style=\"width:auto;\">" + p[i].thoigianvipham + "</td></tr>";
                }
            htmlContent +="</table>";
            System.Web.HttpContext.Current.Response.Write(htmlContent);
            System.Web.HttpContext.Current.Response.Flush();
           
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
        public string createReport(string keyword, int type)
        {            
            try
            {
                var reportName = "";
                var content = "";
                switch (type)
                {
                    case 1:
                        reportName = "BaoCaoTheoLoai"+Guid.NewGuid().ToString()+".pdf";
                        content = HomeController.getLoaiContentPDF(keyword);
                        break;
                    case 2:
                        reportName = "BaoCaoTheoTinh" + Guid.NewGuid().ToString() + ".pdf";
                        content = HomeController.getQuanHuyenContentPDF(keyword);
                        break;
                    case 3:
                        reportName = "BaoCaoTheoDoiTuong" + Guid.NewGuid().ToString() + ".pdf";
                        content = HomeController.getDoiTuongContentPdf(keyword);
                        break;
                    default:
                        reportName = "BaoCao" + Guid.NewGuid().ToString() + ".pdf";
                        content = HomeController.getLoaiContentPDF(keyword);
                        break;
                }

                string path = HttpContext.Server.MapPath("../Images/Report" + "\\");
                iTextSharp.text.pdf.BaseFont Vn_Helvetica = iTextSharp.text.pdf.BaseFont.CreateFont(@"C:\Windows\Fonts\arial.ttf", "Identity-H", iTextSharp.text.pdf.BaseFont.EMBEDDED);
                iTextSharp.text.Font fontNormal = new iTextSharp.text.Font(Vn_Helvetica, 12, iTextSharp.text.Font.NORMAL);
                
                Document doc = new Document();
                PdfWriter writer = PdfWriter.GetInstance(doc, new FileStream(path + reportName, FileMode.Create, FileAccess.Write));
                doc.Open();
                doc.Add(new Paragraph(content, fontNormal));
                doc.Close();

                //Document document = new Document();
                //PdfWriter.GetInstance(document, new FileStream(path + reportName, FileMode.Create));
                //document.Open();
                //List<IElement> htmlarraylist = HTMLWorker.ParseToList(new StringReader(html), null);
                //for (int k = 0; k < htmlarraylist.Count; k++)
                //{
                //    document.Add((IElement)htmlarraylist[k]);
                //}
                //document.Close();
                return reportName;
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }
    }
}
