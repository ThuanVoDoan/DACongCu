using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace DATH.Models
{
    [MetadataTypeAttribute(typeof(DonViToChucMetadata))]
    public partial class DonViToChuc
    {
        internal sealed class DonViToChucMetadata
        {
            [Display(Name = "Tên Đơn Vị")]
            public string TenDonVi { get; set; }
            [Display(Name = "Mã Đơn Vị")]
            public string MaDonVi { get; set; }
            [Display(Name = "Xóa Tạm")]
            public System.Nullable<bool> XoaTam { get; set; }
        }
    }
}