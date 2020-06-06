using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DATH.Models;
using System.IO;

namespace DATH.Controllers.Nguoidung
{
    public class TaoHdBCSController : Controller
    {
        ActionDataContext db = new ActionDataContext();

        [HttpGet]
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
                    if (nd != null && a.Quyen.MoTa == "thd")
                    {
                        ViewBag.IdDonViToChuc = new SelectList(db.DonViToChucs.ToList().OrderBy(n => n.IdDonViToChuc), "IdDonViToChuc", "TenDonVi");
                        return View();
                    }
                }
                return RedirectToAction("Loi", "Login");
            }
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Index(HoatDong ds, HttpPostedFileBase fileUpload)
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
                    if (nd != null && a.Quyen.MoTa == "thd")
                    {
                        string message = "";
                        var fileName = Path.GetFileName(fileUpload.FileName);
                        var path = Path.Combine(Server.MapPath("~/images/"), fileName);
                        if (System.IO.File.Exists(path))
                        {
                            ViewBag.ThongBao = "Hình ảnh đã tồn tại";
                        }
                        else
                        {
                            fileUpload.SaveAs(path);
                        }
                        if(ds.ThoiGianBD < DateTime.Now || ds.ThoiGianKT < DateTime.Now || ds.ThoiGianBD > ds.ThoiGianKT)
                        {
                            ViewBag.message = "Ngày tháng không hợp lệ";
                            return View();
                        }
                        ds.HinhAnh = fileName;
                        ds.XoaTam = false;
                        ds.TrangThai = "cd";
                        ds.IdInfo = nd.IdInfo;
                        ds.SLDaDangKi = 0;
                        ds.ThoiGianT = DateTime.Now;
                        ds.IdDonViToChuc = 3;
                        db.HoatDongs.InsertOnSubmit(ds);
                        db.SubmitChanges();
                    }
                }
                return RedirectToAction("Index", "HDDaTaoBCS");
            }
        }
    }
}