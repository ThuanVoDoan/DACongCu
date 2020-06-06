using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DATH.Models;

namespace DATH.Controllers.Nguoidung
{
    public class HDDaTaoBCSController : Controller
    {
        ActionDataContext db = new ActionDataContext();
        // GET: HDDaTaoBCS
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
                    if (nd != null && a.Quyen.MoTa == "xdshdbcs")
                    {
                        List<HoatDong> lsthd = db.HoatDongs.OrderBy(n => n.IdHoatDong).ToList();
                        return View(lsthd.Where(n => (n.IdInfo == nd.IdInfo) && (n.XoaTam == false) || ((n.XoaTam == true) && (n.TrangThai =="dh") )).ToList());
                    }
                }
                return RedirectToAction("Index", "TrangChu");
            }
        }

        public JsonResult Get(int id)
        {
            var position = db.HoatDongs.Where(x => x.IdHoatDong == id).FirstOrDefault();
            if (position == null) return null;
            return new JsonResult()
            {
                Data = new
                {
                    NAME = position.TenHoatDong,
                    THOIGIANTAO = position.ThoiGianT
                },
                JsonRequestBehavior = JsonRequestBehavior.DenyGet
            };
        }

        [HttpPost]
        public string Delete(HoatDong position)
        {
            NguoiDung nd = (NguoiDung)Session["Taikhoan"];
            List<Rel_CV_Q> re = db.Rel_CV_Qs.OrderBy(n => n.IdChucVu).Where(n => n.IdChucVu == nd.IdChucVu).ToList();
            foreach (var a in re)
            {
                if (nd != null)
                {
                        HoatDong p = db.HoatDongs.SingleOrDefault(n => n.IdHoatDong == position.IdHoatDong);
                        p.TrangThai = "dh";
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