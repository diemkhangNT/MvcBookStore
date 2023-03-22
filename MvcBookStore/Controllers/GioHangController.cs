using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MvcBookStore.Models;

namespace MvcBookStore.Controllers
{
    public class GioHangController : Controller
    {
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
    }
}