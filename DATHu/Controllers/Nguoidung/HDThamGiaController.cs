using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DATH.Models;
using System.IO;

namespace DATH.Controllers.Nguoidung
{
    public class HDThamGiaController : Controller
    {
        ActionDataContext db = new ActionDataContext();
        // GET: HDThamGia
        public ActionResult Index(int id = 0)
        {
            if (Session["Taikhoan"] == null || Session["Taikhoan"].ToString() == "")
            {
                return RedirectToAction("Index", "Login");
            }
            else
            {
                NguoiDung nd = (NguoiDung)Session["Taikhoan"];
                NguoiDung hd = db.NguoiDungs.SingleOrDefault(n => n.IdInfo == id);
                List<DangKiThamGiaHD> lstdk = db.DangKiThamGiaHDs.OrderBy(n => n.IdDangKiHD).ToList();
                List<ThongBao> lsttb = new List<ThongBao>();

                foreach (var hd1 in lstdk)
                {
                    var b = new ThongBao();
                    b.IdDangKiHD = hd1.IdDangKiHD;
                    b.TenHoatDong = hd1.HoatDong.TenHoatDong;
                    b.IdHoatDong = hd1.HoatDong.IdHoatDong;
                    b.ThoiGianBD = hd1.HoatDong.ThoiGianBD;
                    b.ThoiGianKT = hd1.HoatDong.ThoiGianKT;
                    b.TrangThai = hd1.TrangThai;
                    b.XoaTamhd = hd1.HoatDong.XoaTam;
                    b.TrangThaihd = hd1.HoatDong.TrangThai;
                    b.IdInfo = hd1.IdInfo;
                    b.XoaTam = hd1.XoaTam;
                    var a = hd1.HoatDong.ThoiGianBD;
                    DateTime aDateTime = Convert.ToDateTime(a);
                    System.TimeSpan d = aDateTime.Subtract(DateTime.Now);
                    int days = Convert.ToInt32(d.TotalDays);
                    Boolean isAdd = false;

                    if (days > 0)
                    {
                        isAdd = true;
                        b.ngayconlai = days;
                    }
                    else if (days <= 0)
                    {
                        isAdd = true;
                    }
                    if (isAdd)
                    {
                        lsttb.Add(b);
                    }
                }
                return View(lsttb.Where(n => ((n.IdInfo == nd.IdInfo) && (n.XoaTam == false)) || ((n.IdInfo == nd.IdInfo) && (n.TrangThaihd == "dh"))).ToList());
            }
        }

        public JsonResult Get(int id)
        {
            List<DangKiThamGiaHD> lstdk = db.DangKiThamGiaHDs.OrderBy(n => n.IdDangKiHD).ToList();
            List<ThongBao> lsttb = new List<ThongBao>();

            foreach (var hd1 in lstdk)
            {
                var b = new ThongBao();
                b.IdDangKiHD = hd1.IdDangKiHD;
                b.IdHoatDong = hd1.IdHoatDong;
                b.TenHoatDong = hd1.HoatDong.TenHoatDong;
                lsttb.Add(b);
            }
            var c = lsttb.Where(x => x.IdDangKiHD == id).FirstOrDefault();
            if (c == null) return null;
            return new JsonResult()
            {
                Data = new { NAME = c.TenHoatDong, HD = c.IdHoatDong },
                JsonRequestBehavior = JsonRequestBehavior.DenyGet
            };
        }

        [HttpPost]
        public string Delete(ThongBao position)
        {
            NguoiDung nd = (NguoiDung)Session["Taikhoan"];
            List<Rel_CV_Q> re = db.Rel_CV_Qs.OrderBy(n => n.IdChucVu).Where(n => n.IdChucVu == nd.IdChucVu).ToList();
            foreach (var a in re)
            {
                if (nd != null)
                {
                    HoatDong hd = db.HoatDongs.SingleOrDefault(n => n.IdHoatDong == position.IdHoatDong);
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

        public FileResult Download()
        {
            string fileName = "GCN.docx";
            var path = Path.Combine(Server.MapPath("~/images/"), fileName);
            //byte[] fileBytes = System.IO.File.ReadAllBytes("DATH:\\~/images\\dllme.txt");
            return File(path, System.Net.Mime.MediaTypeNames.Application.Octet, fileName);
        }
    }
}