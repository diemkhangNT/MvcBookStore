using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MvcBookStore.Models;
using PagedList;
using System.IO;
using System.Net;
using System.Data.Entity;

namespace MvcBookStore.Controllers
{
    public class AdminController : Controller
    {
        // Use DbContext to manage database
        QLBANSACHEntities database = new QLBANSACHEntities();
        // GET: Admin
        public ActionResult Index()
        {
            if (Session["Admin"] == null)
                return RedirectToAction("Login");
            var sach = database.SACHes.ToList();
            return View(sach);
        }
        [HttpGet]
        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Login(ADMIN admin)
        {
            if (ModelState.IsValid)
            {
                if (string.IsNullOrEmpty(admin.UserAdmin))
                    ModelState.AddModelError(string.Empty, "User name không được để trống");
                if (string.IsNullOrEmpty(admin.PassAdmin))
                    ModelState.AddModelError(string.Empty, "Password không được để trống");
                    //Kiểm tra có admin này hay chưa
                    var adminDB = database.ADMINs.FirstOrDefault(ad => ad.UserAdmin == admin.UserAdmin && ad.PassAdmin == admin.PassAdmin);
                if (adminDB == null)
                    ModelState.AddModelError(string.Empty, "Tên đăng nhập hoặc mật khẩu không đúng");
                else
                {
                    Session["Admin"] = adminDB;
                    ViewBag.ThongBao = "Đăng nhập admin thành công";
                    return RedirectToAction("Index", "Admin");
                }
            }
            return View();
        }

        public ActionResult DangXuat()
        {
            return RedirectToAction("Login", "Admin");
        }

        public ActionResult Sach(int? page)
        {
            var dsSach = database.SACHes.ToList();
            //Tạo biến cho biết số sách mỗi trang
            int pageSize = 7;
            //Tạo biến số trang
            int pageNum = (page ?? 1);
            return View(dsSach.OrderBy(sach => sach.Masach).ToPagedList(pageNum, pageSize));
        }

        //Tạo mới sách
        public ActionResult ThemSach()
        {
            ViewBag.MaCD = new SelectList(database.CHUDEs.ToList(), "MaCD", "TenChuDe");
            ViewBag.MaNXB = new SelectList(database.NHAXUATBANs.ToList(), "MaNXB", "TenNXB");
            return View();
        }

        [HttpPost]
        public ActionResult ThemSach(SACH sACH, HttpPostedFileBase Hinhminhhoa)
        {
            ViewBag.MaCD = new SelectList(database.CHUDEs.ToList(), "MaCD", "TenChuDe");
            ViewBag.MaNXB = new SelectList(database.NHAXUATBANs.ToList(), "MaNXB", "TenNXB");
            if (Hinhminhhoa == null)
            {
                ViewBag.ThongBao = "Vui lòng chọn ảnh";
                return View();
            }
            else
            {
                if (ModelState.IsValid)
                {
                    //Lấy tên file hình được up lên
                    var fileName = Path.GetFileName(Hinhminhhoa.FileName);
                    //Tạo đường dẫn tới file 
                    var path = Path.Combine(Server.MapPath("~/Images"), fileName);
                    //Kiểm tra hình đã tồn tại trong hệ thống chưa
                    if (System.IO.File.Exists(path))
                    {
                        ViewBag.ThongBao = "Hình đã tồn tại";
                    }
                    else
                    {
                        Hinhminhhoa.SaveAs(path); //Lưu vào hệ thống
                    }
                    //Lưu tên sách vào trường Hinhminhhoa
                    sACH.Hinhminhhoa = fileName;
                    //Lưu vào CSDL
                    database.SACHes.Add(sACH);
                    database.SaveChanges();
                }
            }
            
            return RedirectToAction("Sach");

        }

        public ActionResult ChiTietSach(int id)
        {
            var sach = database.SACHes.FirstOrDefault(s => s.Masach == id);
            if(sach == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            return View(sach);
        }

        // GET: SACHes/Edit/5
        public ActionResult SuaSach(int? id)
        {
            
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SACH sACH = database.SACHes.Find(id);
            ViewBag.MaCD = new SelectList(database.CHUDEs, "MaCD", "TenChuDe", sACH.MaCD);
            ViewBag.MaNXB = new SelectList(database.NHAXUATBANs, "MaNXB", "TenNXB", sACH.MaNXB);
            if (sACH == null)
            {
                return HttpNotFound();
            }
            return View(sACH);
        }

        // POST: SACHes/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult SuaSach([Bind(Include = "Masach,Tensach,DonVitinh,Dongia,Mota,Hinhminhhoa,MaCD,MaNXB,Ngaycapnhat,Soluongban,Solanxem")] SACH sACH)
        {
            ViewBag.MaCD = new SelectList(database.CHUDEs, "MaCD", "TenChuDe", sACH.MaCD);
            ViewBag.MaNXB = new SelectList(database.NHAXUATBANs, "MaNXB", "TenNXB", sACH.MaNXB);
            if (ModelState.IsValid)
            {
                database.Entry(sACH).State = EntityState.Modified;
                database.SaveChanges();
                return RedirectToAction("Sach");
            }
            return View(sACH);
        }

        // GET: SACHes/Delete/5
        public ActionResult XoaSach(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SACH sACH = database.SACHes.Find(id);
            if (sACH == null)
            {
                return HttpNotFound();
            }
            return View(sACH);
        }

        // POST: SACHes/Delete/5
        [HttpPost, ActionName("XoaSach")]
        [ValidateAntiForgeryToken]
        public ActionResult XoaSachConfirmed(int id)
        {
            SACH sACH = database.SACHes.Find(id);
            database.SACHes.Remove(sACH);
            database.SaveChanges();
            return RedirectToAction("Sach");
        }
    }
}