//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace MvcBookStore.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class CTDATHANG
    {
        public int SODH { get; set; }
        public int MaSach { get; set; }
        public Nullable<int> Soluong { get; set; }
        public Nullable<decimal> DonGia { get; set; }
        public Nullable<decimal> ThanhTien { get; set; }
    
        public virtual DONDATHANG DONDATHANG { get; set; }
        public virtual SACH SACH { get; set; }
    }
}