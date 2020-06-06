using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace DATH.Models
{
    public class ThongBao
    {
        public int IdDangKiHD { get; set; }
        public System.Nullable<int> IdInfo { get; set; }
        public System.Nullable<int> IdHoatDong { get; set; }

        public string TenHoatDong { get; set; }
        public int ngayconlai { get; set; }
        public System.Nullable<bool> XoaTam { get; set; }
        [DataType(DataType.DateTime)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:HH:mm dd-MM-yyyy}")]
        public System.Nullable<System.DateTime> ThoiGianBD { get; set; }
        [DataType(DataType.DateTime)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:HH:mm dd-MM-yyyy}")]
        public System.Nullable<System.DateTime> ThoiGianKT { get; set; }
        [DataType(DataType.DateTime)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:HH:mm dd-MM-yyyy}")]
        public System.Nullable<System.DateTime> ThoiGianT { get; set; }
        public System.Nullable<bool> TrangThai { get; set; }
        public string TrangThaihd { get; set; }
        public System.Nullable<bool> XoaTamhd { get; set; }
        public string SoId { get; set; }
        public string Ho_Ten { get; set; }
        public string TenLop { get; set; }
        public string TenDonVi { get; set; }
        public string Background { get; set; }
        public string HinhAnh { get; set; }

    }
}