using DATH.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DATH.Controllers.Quantri
{
    public class QuanlyQuyen_PhanquyenController : Controller
    {
        ActionDataContext db = new ActionDataContext();
        // GET: QuanlyQuyen_Phanquyen

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
                    if (nd != null && a.Quyen.MoTa == "xdsq")
                    {
                        return View(db.Rel_CV_Qs.OrderBy(n => n.IdQuyen).GroupBy(n=>n.IdQuyen).ToList());
                    }
                }
                return RedirectToAction("Loi", "Login");
            }
        }
    }
}