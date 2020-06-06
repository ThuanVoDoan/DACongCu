using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace DATH.Models
{
    [MetadataTypeAttribute(typeof(NguoiDungMetadata))]
    public partial class NguoiDung
    {
        internal sealed class NguoiDungMetadata
        {
            [Display(Name = "Mã quản trị")]
            public string SoId { get; set; }
            [Display(Name = "Họ tên")]
            public string Ho_Ten { get; set; }
            [Display(Name = "Số điện thoại")]
            public string SDT { get; set; }
            [Display(Name = "Chức Vụ")]
            public System.Nullable<int> IdChucVu { get; set; }
        }
    }
}