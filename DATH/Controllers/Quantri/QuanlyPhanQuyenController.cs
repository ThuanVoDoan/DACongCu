using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DATH.Models;

namespace DATH.Controllers.Quantri
{
    public class QuanlyPhanQuyenController : Controller
    {
        ActionDataContext db = new ActionDataContext(); 
        // GET: QuanlyPhanQuyen
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
                    if (nd != null && a.Quyen.MoTa == "xdsqcv")
                    {
                        return View(db.Rel_CV_Qs.OrderBy(n => n.IdChucVu).Where(n => n.XoaTam == false).ToList());
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
                    if (nd != null && a.Quyen.MoTa == "tqcv")
                    {
                        ViewBag.IdQuyen = new SelectList(db.Quyens.ToList().OrderBy(n => n.IdQuyen), "IdQuyen", "TenQuyen");
                        ViewBag.IdChucVu = new SelectList(db.ChucVus.ToList().OrderBy(n => n.IdChucVu), "IdChucVu", "TenChucVu");
                        return View();
                    }
                }
                return RedirectToAction("Loi", "Login");
            }
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult ThemMoi(Rel_CV_Q ds)
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
                    if (nd != null && a.Quyen.MoTa == "tqcv")
                    {
                        if (ModelState.IsValid)
                        {
                            ds.XoaTam = false;
                            db.Rel_CV_Qs.InsertOnSubmit(ds);
                            db.SubmitChanges();
                        }
                    }
                }
                return RedirectToAction("Index", "QuanlyPhanQuyen");
            }
        }

        public JsonResult GetPhanquyen(int id)
        {
            var position = db.Rel_CV_Qs.Where(x => x.IdCV_Q == id).FirstOrDefault();
            if (position == null) return null;
            ViewBag.IdQuyen = new SelectList(db.Quyens.ToList().OrderBy(n => n.IdQuyen), "IdQuyen", "TenQuyen");
            ViewBag.IdChucVu = new SelectList(db.ChucVus.ToList().OrderBy(n => n.IdChucVu), "IdChucVu", "TenChucVu");
            return new JsonResult()
            {
                Data = new { NAME = position.ChucVu.TenChucVu, DESCRIPTION = position.Quyen.TenQuyen },
                JsonRequestBehavior = JsonRequestBehavior.DenyGet
            };
        }

        [HttpPost]
        public string AddPhanquyen(Rel_CV_Q position)
        {
            NguoiDung nd = (NguoiDung)Session["Taikhoan"];
            List<Rel_CV_Q> re = db.Rel_CV_Qs.OrderBy(n => n.IdChucVu).Where(n => n.IdChucVu == nd.IdChucVu).ToList();
            foreach (var a in re)
            {
                if (nd != null && a.Quyen.MoTa == "tqcv")
                {
                    try
                    {
                        position.XoaTam = false;
                        db.Rel_CV_Qs.InsertOnSubmit(position);
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
        public string DeletePhanquyen(Rel_CV_Q position)
        {
            NguoiDung nd = (NguoiDung)Session["Taikhoan"];
            List<Rel_CV_Q> re = db.Rel_CV_Qs.OrderBy(n => n.IdChucVu).Where(n => n.IdChucVu == nd.IdChucVu).ToList();
            foreach (var a in re)
            {
                if (nd != null && a.Quyen.MoTa == "xqcv")
                {
                    Rel_CV_Q p = db.Rel_CV_Qs.SingleOrDefault(n => n.IdCV_Q == position.IdCV_Q);
                    //p.XoaTam = true;

                    try
                    {
                        //UpdateModel(p);
                        db.Rel_CV_Qs.DeleteOnSubmit(p);
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