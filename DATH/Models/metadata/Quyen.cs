using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace DATH.Models
{
    [MetadataTypeAttribute(typeof(QuyenMetadata))]
    public partial class Quyen
    {
        internal sealed class QuyenMetadata
        {
            [Display(Name = "Tên Quyền")]
            public string TenQuyen { get; set; }
            [Display(Name = "Mã quyền")]
            public string MoTa { get; set; }
            [Display(Name = "Xóa Tạm")]
            public System.Nullable<bool> XoaTam { get; set; }
        }
    }
}