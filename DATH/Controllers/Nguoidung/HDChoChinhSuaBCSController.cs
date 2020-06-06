using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DATH.Models;
using System.IO;

namespace DATH.Controllers.Nguoidung
{
    public class HDChoChinhSuaBCSController : Controller
    {
        ActionDataContext db = new ActionDataContext();
        // GET: HDChoChinhSuaBCS
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
                    if (nd != null && a.Quyen.MoTa == "xdshdchs")
                    {
                        return View(db.HoatDongs.ToList().OrderBy(n => n.IdHoatDong).Where(n => (n.TrangThai == "ccs") && (n.IdInfo == nd.IdInfo) && (n.XoaTam==false)).ToList());
                    }
                }
                return RedirectToAction("Loi", "Login");
            }
        }

        [HttpGet]
        public ActionResult ChinhSua(int id)
        {
            if (Session["Taikhoan"] == null || Session["Taikhoan"].ToString() == "")
            {
                return RedirectToAction("Index", "Login");
            }
            else
            {
                NguoiDung nd = (NguoiDung)Session["Taikhoan"];
                if (nd != null)
                {
                    HoatDong sv = db.HoatDongs.SingleOrDefault(n => (n.IdHoatDong == id) && (n.IdInfo == nd.IdInfo));
                    if (sv == null)
                    {
                        Response.StatusCode = 404;
                        return null;
                    }
                    return View(sv);
                }
                return RedirectToAction("Index", "TrangChu");
            }
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult ChinhSua(HoatDong sv, HttpPostedFileBase fileUpload)
        {
            if (Session["Taikhoan"] == null || Session["Taikhoan"].ToString() == "")
            {
                return RedirectToAction("Index", "Login");
            }
            else
            {
                NguoiDung nd = (NguoiDung)Session["Taikhoan"];
                
                if (fileUpload == null && nd != null)
                {
                    HoatDong d = db.HoatDongs.SingleOrDefault(n => (n.IdHoatDong == sv.IdHoatDong) && (n.IdInfo == nd.IdInfo));
                    d.TenHoatDong = sv.TenHoatDong;
                    d.MoTaChinh = sv.MoTaChinh;
                    d.ThoiGianBD = sv.ThoiGianBD;
                    d.ThoiGianKT = sv.ThoiGianKT;
                    d.GioiHanNguoiThamGia = sv.GioiHanNguoiThamGia;
                    d.Diadiem = sv.Diadiem;
                    UpdateModel(d);
                    db.SubmitChanges();
                    return RedirectToAction("Index", "HDChoChinhSuaBCS");
                }
                else if (fileUpload != null && nd != null)
                {
                    HoatDong d = db.HoatDongs.SingleOrDefault(n => (n.IdHoatDong == sv.IdHoatDong) && (n.IdInfo == nd.IdInfo));
                    var fileName = Path.GetFileName(fileUpload.FileName);
                    var path = Path.Combine(Server.MapPath("~/images"), fileName);
                    ViewBag.chucvu = nd.ChucVu.MoTa;
                    if (System.IO.File.Exists(path))
                        ViewBag.ThongBao = "Hình ảnh đã tồn tại";
                    else
                    {
                        fileUpload.SaveAs(path);
                    }
                    d.HinhAnh = fileName;
                    UpdateModel(d);
                    db.SubmitChanges();
                    return RedirectToAction("Index", "HDChoChinhSuaBCS");
                }
                return RedirectToAction("Index", "TrangChu");
            }
        }
    }
}