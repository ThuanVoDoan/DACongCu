using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DATH.Models;

namespace DATH.Controllers.Quantri
{
    public class QuanlyChucvuController : Controller
    {
        ActionDataContext db = new ActionDataContext();
        // GET: QuanlyChucvu
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
                    if (nd != null && a.Quyen.MoTa == "xdscv")
                    {
                        return View(db.ChucVus.OrderBy(n => n.IdChucVu).Where(n => n.XoaTam == false).ToList());
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
                    if (nd != null && a.Quyen.MoTa == "tcv")
                    {
                        return View();
                    }
                }
                return RedirectToAction("Loi", "Login");
            }
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult ThemMoi(ChucVu ds)
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
                    if (nd != null && a.Quyen.MoTa == "tcv")
                    {
                        if (ModelState.IsValid)
                        {
                            ds.XoaTam = false;
                            db.ChucVus.InsertOnSubmit(ds);
                            db.SubmitChanges();
                        }
                    }
                }
                return RedirectToAction("Index", "QuanlyChucvu");
            }
        }

        public JsonResult GetChucvu(int id)
        {
            var position = db.ChucVus.Where(x => x.IdChucVu == id).FirstOrDefault();
            if (position == null) return null;
            return new JsonResult()
            {
                Data = new { NAME = position.TenChucVu, DESCRIPTION = position.MoTa },
                JsonRequestBehavior = JsonRequestBehavior.DenyGet
            };
        }

        [HttpPost]
        public string EditChucvu(ChucVu position)
        {
            NguoiDung nd = (NguoiDung)Session["Taikhoan"];
            List<Rel_CV_Q> re = db.Rel_CV_Qs.OrderBy(n => n.IdChucVu).Where(n => n.IdChucVu == nd.IdChucVu).ToList();
            foreach (var a in re)
            {
                if (nd != null && a.Quyen.MoTa == "scv")
                {
                    ChucVu p = db.ChucVus.SingleOrDefault(n => n.IdChucVu == position.IdChucVu);
                    p.MoTa = position.MoTa;
                    p.TenChucVu = position.TenChucVu;

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
        public string AddChucvu(ChucVu position)
        {
            NguoiDung nd = (NguoiDung)Session["Taikhoan"];
            List<Rel_CV_Q> re = db.Rel_CV_Qs.OrderBy(n => n.IdChucVu).Where(n => n.IdChucVu == nd.IdChucVu).ToList();
            foreach (var a in re)
            {
                if (nd != null && a.Quyen.MoTa == "tcv")
                {
                    try
                    {
                        position.XoaTam = false;
                        db.ChucVus.InsertOnSubmit(position);
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
        public string DeleteChucvu(ChucVu position)
        {
            NguoiDung nd = (NguoiDung)Session["Taikhoan"];
            List<Rel_CV_Q> re = db.Rel_CV_Qs.OrderBy(n => n.IdChucVu).Where(n => n.IdChucVu == nd.IdChucVu).ToList();
            foreach (var a in re)
            {
                if (nd != null && a.Quyen.MoTa == "xcv")
                {
                    ChucVu p = db.ChucVus.SingleOrDefault(n => n.IdChucVu == position.IdChucVu);
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