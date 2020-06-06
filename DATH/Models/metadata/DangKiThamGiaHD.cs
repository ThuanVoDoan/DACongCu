using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace DATH.Models
{
    [MetadataTypeAttribute(typeof(DangKiThamGiaHDMetadata))]
    public partial class DangKiThamGiaHD
    {
        internal sealed class DangKiThamGiaHDMetadata
        {
            [Display(Name = "ID Sinh Viên")]
            public System.Nullable<int> IdInfo { get; set; }
            [Display(Name = "ID Hoạt Động")]
            public System.Nullable<int> IdHoatDong { get; set; }
            [Display(Name = "Tình Trạng")]
            public System.Nullable<bool> TrangThai { get; set; }
            
        }
    }
}