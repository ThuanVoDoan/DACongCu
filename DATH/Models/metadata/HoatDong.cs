using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.ComponentModel;

namespace DATH.Models
{
    [MetadataTypeAttribute(typeof(HoatDongMetadata))]
    public partial class HoatDong
    {
        internal sealed class HoatDongMetadata
        {
            [Display(Name = "Tên Hoạt Động")]
            public string TenHoatDong { get; set; }
            [Display(Name = "Mô Tả Chính")]
            public string MoTaChinh { get; set; }

            [Display(Name = "Giới Hạn Tham Gia")]
            public System.Nullable<int> GioiHanNguoiThamGia { get; set; }
            [Display(Name = "Đã Đăng Kí")]
            public System.Nullable<int> SLDaDangKi { get; set; }
            [Display(Name = "Nội Dung Chỉnh Sửa")]
            public string NoiDungChinhSua { get; set; }
            [Display(Name = "Xóa Tạm")]
            public System.Nullable<bool> XoaTam { get; set; }
            [Display(Name = "Đơn Vị Tổ Chức")]
            public System.Nullable<int> IdDonViToChuc { get; set; }
            [Display(Name = "Hình Ảnh")]
            public string HinhAnh { get; set; }
            [Display(Name = "Trạng Thái")]
            public string TrangThai { get; set; }
            [Display(Name = "Người Tạo HD")]
            public System.Nullable<int> IdInfo { get; set; }

            [DataType(DataType.DateTime)]
            [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:HH:mm dd-MM-yyyy}")]
            public System.Nullable<System.DateTime> ThoiGianKT { get; set; }
            [DataType(DataType.DateTime)]
            [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:HH:mm dd-MM-yyyy}")]
            public System.Nullable<System.DateTime> ThoiGianBD { get; set; }
            [DataType(DataType.DateTime)]
            [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:HH:mm dd-MM-yyyy}")]
            public System.Nullable<System.DateTime> ThoiGianT { get; set; }
        }
    }
}