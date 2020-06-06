using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace DATH.Models
{
    [MetadataTypeAttribute(typeof(QuyenTheoChucVuMetadata))]
    public partial class QuyenTheoChucVu
    {
        internal sealed class QuyenTheoChucVuMetadata
        {
            [Display(Name = "Tên Chức Vụ")]
            public System.Nullable<int> IdChucVu { get; set; }
            [Display(Name = "Tên Quyền")]
            public System.Nullable<int> IdQuyen { get; set; }
            [Display(Name = "Xóa Tạm")]
            public System.Nullable<bool> XoaTam { get; set; }
        }
    }
}