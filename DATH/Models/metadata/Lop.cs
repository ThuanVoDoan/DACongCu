using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace DATH.Models
{
    [MetadataTypeAttribute(typeof(LopMetadata))]
    public partial class Lop
    {
        internal sealed class LopMetadata
        {
            [Display(Name = "Tên Lớp")]
            public string TenLop { get; set; }
            [Display(Name = "Xóa Tạm")]
            public System.Nullable<bool> XoaTam { get; set; }
        }
    }
}