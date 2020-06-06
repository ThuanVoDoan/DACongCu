using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DATH.Models;

namespace DATH.Controllers.Quantri
{
    public class QuanlyLopController : Controller
    {
        ActionDataContext db = new ActionDataContext();
        // GET: QuanlyLop
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
                    if (nd != null && a.Quyen.MoTa == "xdsl")
                    {
                        return View(db.Lops.OrderBy(n => n.IdLop).Where(n => n.XoaTam == false).ToList());
                    }
                }
                return RedirectToAction("Loi", "Login");
            }
        }

        [HttpGet]
        public ActionResult ThemMoi()
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
                    if (nd != null && a.Quyen.MoTa == "tl")
                    {
                        return View();
                    }
                }
                return RedirectToAction("Loi", "Login");
            }
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult ThemMoi(Lop ds)
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
                    if (nd != null && a.Quyen.MoTa == "tl")
                    {
                        if (ModelState.IsValid)
                        {
                            ds.XoaTam = false;
                            db.Lops.InsertOnSubmit(ds);
                            db.SubmitChanges();
                        }
                    }
                }
                return RedirectToAction("Index", "QuanlyLop");
            }
        }

        public JsonResult GetLop(int id)
        {
            var position = db.Lops.Where(x => x.IdLop == id).FirstOrDefault();
            if (position == null) return null;
            return new JsonResult()
            {
                Data = new { NAME = position.TenLop },
                JsonRequestBehavior = JsonRequestBehavior.DenyGet
            };
        }

        [HttpPost]
        public string EditLop(Lop position)
        {
            NguoiDung nd = (NguoiDung)Session["Taikhoan"];
            List<Rel_CV_Q> re = db.Rel_CV_Qs.OrderBy(n => n.IdChucVu).Where(n => n.IdChucVu == nd.IdChucVu).ToList();
            foreach (var a in re)
            {
                if (nd != null && a.Quyen.MoTa == "sl")
                {
                    Lop p = db.Lops.SingleOrDefault(n => n.IdLop == position.IdLop);
                    //p.TenLop = position.TenLop;

                    try
                    {
                        UpdateModel(p);
                        db.SubmitChanges();
                    }
                    catch (Exception ex)
                    {
                        return ex.Message;
                    }
                    return "ok";
                }
            }
            return "Bạn không có quyền cập nhật";
        }

        [HttpPost]
        public string AddLop(Lop position)
        {
            NguoiDung nd = (NguoiDung)Session["Taikhoan"];
            List<Rel_CV_Q> re = db.Rel_CV_Qs.OrderBy(n => n.IdChucVu).Where(n => n.IdChucVu == nd.IdChucVu).ToList();
            foreach (var a in re)
            {
                if (nd != null && a.Quyen.MoTa == "tl")
                {
                    try
                    {
                        position.XoaTam = false;
                        db.Lops.InsertOnSubmit(position);
                        db.SubmitChanges();
                    }
                    catch (Exception ex)
                    {
                        return ex.Message;
                    }
                    return "ok";
                }
            }
            return "Bạn không có quyền thêm";
        }

        [HttpPost]
        public string DeleteLop(Lop position)
        {
            NguoiDung nd = (NguoiDung)Session["Taikhoan"];
            List<Rel_CV_Q> re = db.Rel_CV_Qs.OrderBy(n => n.IdChucVu).Where(n => n.IdChucVu == nd.IdChucVu).ToList();
            foreach (var a in re)
            {
                if (nd != null && a.Quyen.MoTa == "xl")
                {
                    Lop p = db.Lops.SingleOrDefault(n => n.IdLop == position.IdLop);
                    p.XoaTam = true;

                    try
                    {
                        UpdateModel(p);
                        db.SubmitChanges();
                    }
                    catch (Exception ex)
                    {
                        return ex.Message;
                    }
                    return "ok";
                }
            }
            return "Bạn không có quyền xóa";
        }
    }
}