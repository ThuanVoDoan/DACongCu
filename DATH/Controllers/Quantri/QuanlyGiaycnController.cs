using DATH.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DATH.Controllers.Quantri
{
    public class QuanlyGiaycnController : Controller
    {
        ActionDataContext db = new ActionDataContext();
        public ActionResult Index()
        {
            if (Session["Taikhoan"] == null || Session["Taikhoan"].ToString() == "")
            {
                return RedirectToAction("Index", "Login");
            }
            else
            {
                NguoiDung nd = (NguoiDung)Session["Taikhoan"];
                List<Rel_CV_Q> re = db.Rel_CV_Qs.OrderBy(n => n.IdChucVu).Where(n => n.IdChucVu == nd.IdChucVu).ToList();
                foreach (var a in re)
                {
                    if (nd != null)
                    {
                        return View(db.DangKiThamGiaHDs.OrderBy(n => n.IdDangKiHD).Where(n => (n.XoaTam == false) && (n.TrangThai == true)).ToList());
                    }
                }
                return RedirectToAction("Loi", "Login");
            }
        }
    }
}