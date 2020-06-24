using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DATH.Models;
using System.Security.Cryptography;
using System.Text;
using Excel = Microsoft.Office.Interop.Excel;

namespace DATH.Controllers.Quantri
{
    public class QuanlyQuantriController : Controller
    {
        ActionDataContext db = new ActionDataContext();
        // GET: QuanlyQuantri
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
                    if (nd != null && a.Quyen.MoTa == "xdsqtv")
                    {
                        return View(db.NguoiDungs.OrderBy(n => n.IdInfo).Where(n=>((n.ChucVu.MoTa == "K") && (n.XoaTam == false)) || ((n.ChucVu.MoTa == "DK") && (n.XoaTam == false))).ToList());
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
                    if (nd != null && a.Quyen.MoTa == "tqtv")
                    {
                        ViewBag.IdChucVu = new SelectList(db.ChucVus.ToList().OrderBy(n => n.IdChucVu), "IdChucVu", "TenChucVu");
                        return View();
                    }
                }
                return RedirectToAction("Loi", "Login");
            }
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult ThemMoi(NguoiDung ds)
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
                    if (nd != null && a.Quyen.MoTa == "tqtv")
                    {
                        if (ModelState.IsValid)
                        {
                            ds.XoaTam = false;
                            db.NguoiDungs.InsertOnSubmit(ds);
                            db.SubmitChanges();
                        }
                    }
                }
                return RedirectToAction("Index", "QuanlyQuantri");
            }
        }

        public JsonResult GetQuantri(int id)
        {
            var position = db.NguoiDungs.Where(x => x.IdInfo == id).FirstOrDefault();
            if (position == null) return null;
            ViewBag.IdChucVu = db.ChucVus.Where(n => (n.MoTa =="K") || (n.MoTa == "DK")).Select(n =>n.MoTa);
            //ViewBag.IdChucVu = new SelectList(db.ChucVus.ToList().OrderBy(n => n.IdChucVu), "IdChucVu", "MoTa");
            return new JsonResult()
            {
                Data = new { CV = ViewBag.IdChucVu, MASO = position.SoId, NAME = position.Ho_Ten, SDT = position.SDT, CHUCVU = position.IdChucVu, MATKHAU = position.MatKhau },
                JsonRequestBehavior = JsonRequestBehavior.DenyGet
            };
        }

        [HttpPost]
        public string EditQuantri(NguoiDung position)
        {
            NguoiDung nd = (NguoiDung)Session["Taikhoan"];
            List<Rel_CV_Q> re = db.Rel_CV_Qs.OrderBy(n => n.IdChucVu).Where(n => n.IdChucVu == nd.IdChucVu).ToList();
            foreach (var a in re)
            {
                if (nd != null && a.Quyen.MoTa == "sqtv")
                {

                    NguoiDung p = db.NguoiDungs.SingleOrDefault(n => n.IdInfo == position.IdInfo);

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
        public string AddQuantri(NguoiDung position)
        {
            NguoiDung nd = (NguoiDung)Session["Taikhoan"];
            List<Rel_CV_Q> re = db.Rel_CV_Qs.OrderBy(n => n.IdChucVu).Where(n => n.IdChucVu == nd.IdChucVu).ToList();
            foreach (var a in re)
            {
                if (nd != null && a.Quyen.MoTa == "tqtv")
                {
                    string pass = "8888";
                    MD5CryptoServiceProvider md5 = new MD5CryptoServiceProvider();

                    byte[] bHash = md5.ComputeHash(Encoding.UTF8.GetBytes(pass));

                    StringBuilder sbHash = new StringBuilder();

                    foreach (byte b in bHash)
                    {

                        sbHash.Append(String.Format("{0:x2}", b));

                    }
                    pass = sbHash.ToString();
                    
                    try
                    {
                        position.MatKhau = pass;
                        position.XoaTam = false;
                        db.NguoiDungs.InsertOnSubmit(position);
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
        public string DeleteQuantri(NguoiDung position)
        {
            NguoiDung nd = (NguoiDung)Session["Taikhoan"];
            List<Rel_CV_Q> re = db.Rel_CV_Qs.OrderBy(n => n.IdChucVu).Where(n => n.IdChucVu == nd.IdChucVu).ToList();
            foreach (var a in re)
            {
                if (nd != null && a.Quyen.MoTa == "xqtv")
                {
                    NguoiDung p = db.NguoiDungs.SingleOrDefault(n => n.IdInfo == position.IdInfo);
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

        [HttpPost]
        public string ResetMk(NguoiDung position)
        {
            NguoiDung nd = (NguoiDung)Session["Taikhoan"];
            List<Rel_CV_Q> re = db.Rel_CV_Qs.OrderBy(n => n.IdChucVu).Where(n => n.IdChucVu == nd.IdChucVu).ToList();
            foreach (var a in re)
            {
                if (nd != null && a.Quyen.MoTa == "remk")
                {
                    NguoiDung p = db.NguoiDungs.SingleOrDefault(n => n.IdInfo == position.IdInfo);

                    try
                    {

                        string pass = "8888";
                        MD5CryptoServiceProvider md5 = new MD5CryptoServiceProvider();

                        byte[] bHash = md5.ComputeHash(Encoding.UTF8.GetBytes(pass));

                        StringBuilder sbHash = new StringBuilder();

                        foreach (byte b in bHash)
                        {

                            sbHash.Append(String.Format("{0:x2}", b));

                        }
                        pass = sbHash.ToString();

                        p.MatKhau = pass;
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