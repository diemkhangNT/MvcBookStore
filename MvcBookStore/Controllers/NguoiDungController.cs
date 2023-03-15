using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MvcBookStore.Models;

namespace MvcBookStore.Controllers
{
    public class NguoiDungController : Controller
    {
        QLBANSACHEntities database = new QLBANSACHEntities();

        // GET: NguoiDung
        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public ActionResult DangKy()
        {
            return View();
        }

        [HttpPost]
        public ActionResult DangKy(KHACHHANG kh)
        {
            if (ModelState.IsValid)
            {
                if (string.IsNullOrEmpty(kh.HoTenKH)){
                    ModelState.AddModelError(string.Empty, "Họ tên không được để trống!");
                }
                if (string.IsNullOrEmpty(kh.TenDN))
                {
                    ModelState.AddModelError(string.Empty, "Tên đăng nhập không được để trống!");
                }
                if (string.IsNullOrEmpty(kh.Matkhau))
                {
                    ModelState.AddModelError(string.Empty, "Mật khẩu không được để trống!");
                }
                if (string.IsNullOrEmpty(kh.Email))
                {
                    ModelState.AddModelError(string.Empty, "Email không được để trống!");
                }
                if (string.IsNullOrEmpty(kh.DienThoaiKH))
                {
                    ModelState.AddModelError(string.Empty, "Điện thoại không được để trống!");
                }

                var khachhang = database.KHACHHANGs.FirstOrDefault(k => k.TenDN == kh.TenDN);
                if(khachhang != null)
                {
                    ModelState.AddModelError(string.Empty, "Đã có người đăng ký tên này!");
                }
                if (ModelState.IsValid)
                {
                    database.KHACHHANGs.Add(kh);
                    database.SaveChanges();
                }
                else
                {
                    return View();
                }
            }
            return RedirectToAction("DangNhap");
        }


        [HttpGet]
        public ActionResult DangNhap()
        {
            return View();
        }

        [HttpPost]
        public ActionResult DangNhap(KHACHHANG kh)
        {
            if (ModelState.IsValid)
            {
                if (string.IsNullOrEmpty(kh.TenDN))
                {
                    ModelState.AddModelError(string.Empty, "Tên đăng nhập không được để trống!");
                }
                if (string.IsNullOrEmpty(kh.Matkhau))
                {
                    ModelState.AddModelError(string.Empty, "Mật khẩu không được để trống!");
                }

                if (ModelState.IsValid)
                {
                    var khach = database.KHACHHANGs.FirstOrDefault(k => k.TenDN == kh.TenDN && k.Matkhau == kh.Matkhau);
                    if(khach != null)
                    {
                        ViewBag.ThongBao = "Chúc mừng đăng nhập thành công!";
                        Session["TaiKhoan"] = khach;
                        return RedirectToAction("Index", "BookStore");
                    }
                    else
                    {
                        ViewBag.ThongBao = "Tên đăng nhập hoặc mật khẩu không đúng!";
                    }
                }
            }
            return View();
        }
    }
}