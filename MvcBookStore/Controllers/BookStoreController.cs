using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MvcBookStore.Models;

namespace MvcBookStore.Controllers
{
    public class BookStoreController : Controller
    {
        QLBANSACHEntities db = new QLBANSACHEntities();

        private List<SACH> LaySachMoi(int soluong)
        {
            //Sắp xếp sách theo ngày cập nhật giảm dần, lấy dúng số lượng sách cần 
            //Chuyển qua dạng danh sách kết quả đạt được
            return db.SACHes.OrderByDescending(sach => sach.Ngaycapnhat).Take(soluong).ToList();
        }


        public ActionResult LayChuDe()
        {
            var dsChuDe = db.CHUDEs.ToList();
            return PartialView(dsChuDe);
        }

        public ActionResult LayNXB()
        {
            var dsNXB = db.NHAXUATBANs.ToList();
            return PartialView(dsNXB);
        }
        // GET: BookStore
        public ActionResult Index()
        {
            //Giả sử cần lấy 5 quyển sách mới cập nhật
            var dsSachMoi = LaySachMoi(6);
            return View(dsSachMoi);
        }

        public ActionResult SPTheoChuDe(int id)
        {
            //Lấy các sách theo mã chủ đề được chọn
            var dsSachtheoChuDe = db.SACHes.Where(sach => sach.MaCD == id).ToList();

            //Trả về View để render các sách trên
            //Tái sử dụng view index ở trên, truyền vào danh sách)
            return View("Index", dsSachtheoChuDe);
        }

        public ActionResult SPTheoNXB(int id)
        {
            //Lấy các sách theo mã chủ đề được chọn
            var dsSachtheoNXB = db.SACHes.Where(sach => sach.MaNXB == id).ToList();

            //Trả về View để render các sách trên
            //Tái sử dụng view index ở trên, truyền vào danh sách)
            return View("Index", dsSachtheoNXB);
        }

        public ActionResult Details(int id)
        {
            //lấy sách có mã tương ứng:
            var sach = db.SACHes.FirstOrDefault(s => s.Masach == id);
            return View(sach);
        }
    }
}