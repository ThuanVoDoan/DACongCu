using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DATH.Models;
using System.Security.Cryptography;
using System.Text;

namespace DATH.Controllers
{
    public class LoginController : Controller
    {
        ActionDataContext db = new ActionDataContext();
        // GET: Login
        public ActionResult Index()
        {
            if(Session["Taikhoan"] != null)
            {
                Session.Clear();
            }
            return View();
        }

        [HttpPost]
        public ActionResult Index(FormCollection f)
        {
            var tendn = f.Get("username").ToString();
            var mk = f.Get("password").ToString();
            MD5CryptoServiceProvider md5 = new MD5CryptoServiceProvider();

            byte[] bHash = md5.ComputeHash(Encoding.UTF8.GetBytes(mk));

            StringBuilder sbHash = new StringBuilder();

            foreach (byte b in bHash)
            {

                sbHash.Append(String.Format("{0:x2}", b));

            }
            mk = sbHash.ToString();

            if (String.IsNullOrEmpty(tendn))
            {
                ViewData["Loi1"] = "Phải nhập tên";
            }
            else if (String.IsNullOrEmpty(mk))
            {
                ViewData["Loi2"] = "Phải nhập mật khẩu";
            }
            else
            {
                NguoiDung nd = db.NguoiDungs.SingleOrDefault(n => n.TaiKhoan == tendn && n.MatKhau == mk);
                List<NguoiDung> li = db.NguoiDungs.ToList();
                foreach(var a in li)
                {
                    if (tendn != a.SoId && mk != a.MatKhau)
                    {
                        ViewBag.ThongBao = "Vui lòng nhập lại !";
                    }
                }
                
                if (nd != null)
                {
                    Session["Taikhoan"] = nd;
                    if (nd.ChucVu.MoTa == "BCS" || nd.ChucVu.MoTa == "SV")
                    {
                        return RedirectToAction("Index", "TrangChu");
                    }
                    else if (nd.ChucVu.MoTa == "DK" || nd.ChucVu.MoTa == "K")
                    {
                        return RedirectToAction("Index", "TrangchuQt");
                    }
                }
            }
            return View();
        }

        public ActionResult HienThiTenSinhVienPartial()
        {

            if (Session["Taikhoan"] == null || Session["Taikhoan"].ToString() == "")
            {
                ViewBag.hoten = "Đăng nhập";
            }
            else
            {
                NguoiDung nd = (NguoiDung)Session["Taikhoan"];
                ViewBag.hoten = nd.Ho_Ten;
            }

            return PartialView();
        }

        public ActionResult HienThiChucVuPartial()
        {
            if (Session["Taikhoan"] == null || Session["Taikhoan"].ToString() == "")
            {
                ViewBag.chucvu = "";
            }
            else
            {
                NguoiDung nd = (NguoiDung)Session["Taikhoan"];
                if (nd.ChucVu.MoTa == "BCS")
                {
                    ViewBag.chucvu = "(" + nd.ChucVu.TenChucVu + ")";
                }else if(nd.ChucVu.MoTa == "SV")
                {
                    ViewBag.chucvu = "";
                }
                else
                {
                    ViewBag.chucvu = nd.ChucVu.TenChucVu;
                } 
            }

            return PartialView();
        }

        public ActionResult TaoHoatDong()
        {
            if (Session["Taikhoan"] == null || Session["Taikhoan"].ToString() == "")
            {
                ViewBag.taohd = "";
            }
            else
            {
                NguoiDung nd = (NguoiDung)Session["Taikhoan"];
                ViewBag.taohd = "Tạo Hoạt Động" + "  ";
            }
            return PartialView();
        }

        public ActionResult DSHoatDongDatao()
        {
            if (Session["Taikhoan"] == null || Session["Taikhoan"].ToString() == "")
            {
                ViewBag.hddatao = "";
            }
            else
            {
                NguoiDung nd = (NguoiDung)Session["Taikhoan"];
                ViewBag.hddatao = "Hoạt Động Đã Tạo" + "  ";
            }
            return PartialView();
        }

        public ActionResult DSHoatDongCuaLop()
        {
            if (Session["Taikhoan"] == null || Session["Taikhoan"].ToString() == "")
            {
                ViewBag.hdcualop = "";
            }
            else
            {
                NguoiDung nd = (NguoiDung)Session["Taikhoan"];
                ViewBag.hdcualop = "Hoạt Động Của Lớp";
            }
            return PartialView();
        }

        public ActionResult DSHoatDongChinhSua()
        {
            if (Session["Taikhoan"] == null || Session["Taikhoan"].ToString() == "")
            {
                ViewBag.hdchinhsua = "";
            }
            else
            {
                NguoiDung nd = (NguoiDung)Session["Taikhoan"];
                ViewBag.hdchinhsua = "Hoạt Động Cần Chỉnh Sửa";
            }
            return PartialView();
        }

        public ActionResult DSHoatDongDangKy()
        {
            if (Session["Taikhoan"] == null || Session["Taikhoan"].ToString() == "")
            {
                ViewBag.hddangky = "";
            }
            else
            {
                NguoiDung nd = (NguoiDung)Session["Taikhoan"];
                ViewBag.hddangky = "Hoạt Động đã đăng ký";
            }
            return PartialView();
        }

        public ActionResult DangXuat()
        {
            if (Session["Taikhoan"] == null || Session["Taikhoan"].ToString() == "")
            {
                ViewBag.dangxuat = "";
            }
            else
            {
                NguoiDung nd = (NguoiDung)Session["Taikhoan"];
                ViewBag.dangxuat = "Đăng xuất";
            }
            return PartialView();
        }

        public ActionResult Loi()
        {
            return View();
        }

        public ActionResult Chucnang()
        {
            if (Session["Taikhoan"] == null || Session["Taikhoan"].ToString() == "")
            {
                ViewBag.chucnang = "";
            }
            else
            {
                NguoiDung nd = (NguoiDung)Session["Taikhoan"];
                ViewBag.chucnang = nd.ChucVu.MoTa;
            }
            return PartialView();
        }

        [HttpGet]
        public ActionResult DoiMK()
        {
            if (Session["Taikhoan"] == null || Session["Taikhoan"].ToString() == "")
            {
                return RedirectToAction("Index", "Login");
            }
            else
            {
                NguoiDung nd = (NguoiDung)Session["Taikhoan"];
                NguoiDung a = db.NguoiDungs.SingleOrDefault(n => n.IdInfo == nd.IdInfo);
                ViewBag.mk = a.MatKhau;
                return View(a);
            }
        }

        [HttpPost]
        public ActionResult DoiMK(NguoiDung po, FormCollection f)
        {
            var pass = f.Get("pass").ToString();

            MD5CryptoServiceProvider md5 = new MD5CryptoServiceProvider();
            byte[] bHash = md5.ComputeHash(Encoding.UTF8.GetBytes(pass));
            StringBuilder sbHash = new StringBuilder();
            foreach (byte b in bHash)
            {

                sbHash.Append(String.Format("{0:x2}", b));

            }
            pass = sbHash.ToString();

            NguoiDung nd = (NguoiDung)Session["Taikhoan"];
            NguoiDung a = db.NguoiDungs.SingleOrDefault(n => n.IdInfo == po.IdInfo);
            a.MatKhau = pass;

            try
            {
                UpdateModel(a);
                db.SubmitChanges();
            }
            catch(Exception e)
            {

            }
            return RedirectToAction("Index","TrangChu");
        }

        public ActionResult DoiMatKhau()
        {
            if (Session["Taikhoan"] == null || Session["Taikhoan"].ToString() == "")
            {
                ViewBag.dmk = "";
            }
            else
            {
                NguoiDung nd = (NguoiDung)Session["Taikhoan"];
                ViewBag.dmk = "Đổi mật khẩu";
            }
            return PartialView();
        }
    }
}