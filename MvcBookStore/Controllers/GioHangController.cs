﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MvcBookStore.Models;

namespace MvcBookStore.Controllers
{
    public class GioHangController : Controller
    {
        QLBANSACHEntities db = new QLBANSACHEntities();
         public List<MatHangMua> LayGioHang()
        {
            List<MatHangMua> giohang = Session["GioHang"] as List<MatHangMua>;
            //Nếu giỏ hàng chưa tồn tại thì tạo mới và đưa vào Session
            if (giohang == null)
            {
                giohang = new List<MatHangMua>();
                Session["GioHang"] = giohang;
            }
            return giohang;
        }

        public ActionResult ThemSPVaoGio(int maSP)
        {
            //Lấy giỏ hàng hiện tại
            List<MatHangMua> giohang = LayGioHang();

            //Kiểm tra xem có tồn tại mặt hàng này trong giỏ hay chưa
            //nếu có thì tăng số lượng lên 1, ngược lại thêm vào giỏ
            MatHangMua sanPham = giohang.FirstOrDefault(s => s.MaSach == maSP);
            if(sanPham == null)
            {
                sanPham = new MatHangMua(maSP);
                giohang.Add(sanPham);
            }
            else
            {
                sanPham.SoLuong++; 
            }
            return RedirectToAction("Details", "BookStore", new { id = maSP });
        }

        private int TinhTongSL()
        {
            int tongSl = 0;
            List<MatHangMua> giohang = LayGioHang();
            if(giohang != null)
            {
                tongSl = giohang.Sum(sp => sp.SoLuong);
            }
            return tongSl;
        }

        private double TinhTongTien()
        {
            double TongTien = 0;
            List<MatHangMua> giohang = LayGioHang();
            if(giohang != null)
            {
                TongTien = giohang.Sum(sp => sp.ThanhTien());
            }
            return TongTien;
        }

        public ActionResult HienThiGioHang()
        {
            List<MatHangMua> giohang = LayGioHang();
            //nếu giỏ hàng trống thì trả về trang ban đầu
            if(giohang == null || giohang.Count == 0)
            {
                return RedirectToAction("Index", "BookStore");
            }
            ViewBag.TongSL = TinhTongSL();
            ViewBag.TongTien = TinhTongTien();
            return View(giohang);
        }

        public ActionResult GioHangPartial()
        {
            ViewBag.TongSL = TinhTongSL();
            ViewBag.TongTien = TinhTongTien();
            return PartialView();
        }

        public ActionResult XoaMatHang(int MaSP)
        {
            List<MatHangMua> giohang = LayGioHang();

            //Lấy sản phẩm trong giỏ hàng
            var sanpham = giohang.FirstOrDefault(s => s.MaSach == MaSP);
            if(sanpham != null)
            {
                giohang.RemoveAll(s => s.MaSach == MaSP);
                return RedirectToAction("HienThiGioHang");
            }
            if(giohang.Count == 0)
            {
                return RedirectToAction("Index", "BookStore");
            }
            return RedirectToAction("HienThiGioHang");
        }

        public ActionResult CapNhatMatHang(int MaSP, int SoLuong)
        {
            List<MatHangMua> giohang = LayGioHang();
            //Lấy sản phẩm trong giỏ hàng
            var sanpham = giohang.FirstOrDefault(s => s.MaSach == MaSP);
            if (sanpham != null)
            {
                //Cập nhật lại số lượng tương ứng
                //Lưu ý số lượng phải lớn hơn hoặc bằng 1
                sanpham.SoLuong = SoLuong;
                
             }
            return RedirectToAction("HienThiGioHang");
            
        }

        public ActionResult DatHang()
        {
            if(Session["TaiKhoan"] == null)
            {
                return RedirectToAction("DangNhap", "NguoiDung");
            }
            List<MatHangMua> giohang = LayGioHang();
            if (giohang == null || giohang.Count == 0)
            {
                return RedirectToAction("Index", "BookStore");
            }
            ViewBag.TongSL = TinhTongSL();
            ViewBag.TongTien = TinhTongTien();
            return View(giohang);
        }

        public ActionResult DongYDatHang()
        {
            KHACHHANG khach = Session["TaiKhoan"] as KHACHHANG;
            List<MatHangMua> giohang = LayGioHang();

            DONDATHANG donHang = new DONDATHANG();
            donHang.MaKh = khach.MaKH;
            donHang.NgayDH = DateTime.Now;
            donHang.Trigia = (decimal)TinhTongTien();
            donHang.Dagiao = false;
            donHang.Tennguoinhan = khach.HotenKH;
            donHang.Diachinhan = khach.DiachiKH;
            donHang.Dienthoainhan = khach.DienthoaiKH;
            donHang.HTGiaohang = false;
            donHang.HTThanhtoan = false;

            db.DONDATHANGs.Add(donHang);
            db.SaveChanges();

            //Them chi tiet gio hang
            foreach(var sanpham in giohang)
            {
                CTDATHANG cTDATHANG = new CTDATHANG();
                cTDATHANG.SoDH = donHang.SoDH;
                cTDATHANG.MaSach = sanpham.MaSach;
                cTDATHANG.Soluong = sanpham.SoLuong;
                cTDATHANG.Dongia = (decimal)sanpham.DonGia;
                db.CTDATHANGs.Add(cTDATHANG);
                db.SaveChanges();
            }

            //Xoa gio hang
            Session["GioHang"] = null;
            return RedirectToAction("HoanThanhDonHang");
        }

        public ActionResult HoanThanhDonHang()
        {
            return View();
        }
    }
}