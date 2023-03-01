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
            return db.SACHes.OrderByDescending(sach => sach.NgayCapNhat).Take(soluong).ToList();
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
    }
}