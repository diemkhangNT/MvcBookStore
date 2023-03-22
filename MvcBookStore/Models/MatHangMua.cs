using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MvcBookStore.Models
{
    public class MatHangMua
    {
        QLBANSACHEntities db = new QLBANSACHEntities();
        public int MaSach { get; set; }
        public string TenSach { get; set; }
        public string AnhBia { get; set; }
        public double DonGia { get; set; }
        public int SoLuong { get; set; }
        public double ThanhTien()
        {
            return SoLuong * DonGia;
        }

        public MatHangMua(int MaSach)
        {
            this.MaSach = MaSach;
            //Tìm sách trong csdl có mã id cần và gán cho mặt hàng được mua
            var sach = db.SACHes.Single(s => s.MaSach == this.MaSach);
            this.TenSach = sach.TenSach;
            this.AnhBia = sach.HinhMinhHoa;
            this.DonGia = double.Parse(sach.DonGia.ToString());
            this.SoLuong = 1; 
        }

    }
}