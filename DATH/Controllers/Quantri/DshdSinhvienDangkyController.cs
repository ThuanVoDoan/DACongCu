using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DATH.Models;

namespace DATH.Controllers.Quantri
{
    public class DshdSinhvienDangkyController : Controller
    {
        ActionDataContext db = new ActionDataContext();
        // GET: DshdSinhvienDangky
        public ActionResult Index(int id)
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
                    if (nd != null && a.Quyen.MoTa == "xdshdsv")
                    {
                        NguoiDung hd = db.NguoiDungs.SingleOrDefault(n => n.IdInfo == id);
                        List<DangKiThamGiaHD> lstdk = db.DangKiThamGiaHDs.OrderBy(n => n.IdDangKiHD).ToList();
                        ViewBag.hoten = hd.Ho_Ten;
                        return View(lstdk.Where(n => (n.IdInfo == hd.IdInfo) && (n.XoaTam == false)).ToList());
                    }
                }
                return RedirectToAction("Loi", "Login");
            }
        }

        public JsonResult Get(int id)
        {
            var position = db.DangKiThamGiaHDs.Where(x => x.IdDangKiHD == id).FirstOrDefault();
            if (position == null) return null;
            return new JsonResult()
            {
                Data = new {NAME = position.HoatDong.TenHoatDong, HD = position.IdHoatDong },
                JsonRequestBehavior = JsonRequestBehavior.DenyGet
            };
        }

        [HttpPost]
        public string Delete(DangKiThamGiaHD position, HoatDong hd, int id1)
        {
            NguoiDung nd = (NguoiDung)Session["Taikhoan"];
            List<Rel_CV_Q> re = db.Rel_CV_Qs.OrderBy(n => n.IdChucVu).Where(n => n.IdChucVu == nd.IdChucVu).ToList();
            foreach (var a in re)
            {
                if (nd != null && a.Quyen.MoTa == "xhdddksv")
                {
                    hd = db.HoatDongs.SingleOrDefault(n => n.IdHoatDong == id1);
                    hd.SLDaDangKi = hd.SLDaDangKi - 1;

                    DangKiThamGiaHD p = db.DangKiThamGiaHDs.SingleOrDefault(n => n.IdDangKiHD == position.IdDangKiHD);
                    p.XoaTam = true;

                    try
                    {
                        UpdateModel(hd);
                        db.SubmitChanges();

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