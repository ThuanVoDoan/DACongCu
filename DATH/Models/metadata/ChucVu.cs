using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace DATH.Models
{
    [MetadataTypeAttribute(typeof(ChucVuMetadata))]
    public partial class ChucVu
    {
        internal sealed class ChucVuMetadata
        {
            [Display(Name = "Tên Chức Vụ")]
            public string TenChucVu { get; set; }
            [Display(Name = "Mã Chức Vụ")]
            public string MoTa { get; set; }
            [Display(Name = "Xóa Tạm")]
            public System.Nullable<bool> XoaTam { get; set; }
        }
    }
}