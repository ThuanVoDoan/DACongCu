using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DATH.Models;

namespace DATH.Controllers.Quantri
{
    public class QuanlyDonvitochucController : Controller
    {
        ActionDataContext db = new ActionDataContext();
        // GET: QuanlyDonvitochuc
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
                    if (nd != null && a.Quyen.MoTa == "xdsdvtc")
                    {
                        return View(db.DonViToChucs.OrderBy(n => n.IdDonViToChuc).Where(n => n.XoaTam == false).ToList());
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
                    if (nd != null && a.Quyen.MoTa == "tdvtc")
                    {
                        return View();
                    }
                }
                return RedirectToAction("Loi", "Login");
            }
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult ThemMoi(DonViToChuc ds)
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
                    if (nd != null && a.Quyen.MoTa == "tdvtc")
                    {
                        if (ModelState.IsValid)
                        {
                            ds.XoaTam = false;
                            db.DonViToChucs.InsertOnSubmit(ds);
                            db.SubmitChanges();
                        }
                    }
                }
                return RedirectToAction("Index", "QuanlyDonvitochuc");
            }
        }

        public JsonResult GetDonvitochuc(int id)
        {
            var position = db.DonViToChucs.Where(x => x.IdDonViToChuc == id).FirstOrDefault();
            if (position == null) return null;
            return new JsonResult()
            {
                Data = new { NAME = position.TenDonVi, DESCRIPTION = position.MaDonVi },
                JsonRequestBehavior = JsonRequestBehavior.DenyGet
            };
        }

        [HttpPost]
        public string EditDonvitochuc(DonViToChuc position)
        {
            NguoiDung nd = (NguoiDung)Session["Taikhoan"];
            List<Rel_CV_Q> re = db.Rel_CV_Qs.OrderBy(n => n.IdChucVu).Where(n => n.IdChucVu == nd.IdChucVu).ToList();
            foreach (var a in re)
            {
                if (nd != null && a.Quyen.MoTa == "sdvtc")
                {
                    DonViToChuc p = db.DonViToChucs.SingleOrDefault(n => n.IdDonViToChuc == position.IdDonViToChuc);
                    p.MaDonVi = position.MaDonVi;
                    p.TenDonVi = position.TenDonVi;

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
        public string AddDonvitochuc(DonViToChuc position)
        {
            NguoiDung nd = (NguoiDung)Session["Taikhoan"];
            List<Rel_CV_Q> re = db.Rel_CV_Qs.OrderBy(n => n.IdChucVu).Where(n => n.IdChucVu == nd.IdChucVu).ToList();
            foreach (var a in re)
            {
                if (nd != null && a.Quyen.MoTa == "tdvtc")
                {
                    try
                    {
                        position.XoaTam = false;
                        db.DonViToChucs.InsertOnSubmit(position);
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
        public string DeleteDonvitochuc(DonViToChuc position)
        {
            NguoiDung nd = (NguoiDung)Session["Taikhoan"];
            List<Rel_CV_Q> re = db.Rel_CV_Qs.OrderBy(n => n.IdChucVu).Where(n => n.IdChucVu == nd.IdChucVu).ToList();
            foreach (var a in re)
            {
                if (nd != null && a.Quyen.MoTa == "xdvtc")
                {
                    DonViToChuc p = db.DonViToChucs.SingleOrDefault(n => n.IdDonViToChuc == position.IdDonViToChuc);
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