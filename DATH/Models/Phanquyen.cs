using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DATH.Models
{
    public class Phanquyen
    {
        public int IdQuyen { get; set; }
        public string TenQuyen { get; set; }
        public int IdChucVu { get; set; }
        public string TenChucVu { get; set; }
        public System.Nullable<bool> XoaTam { get; set; }
    }
}