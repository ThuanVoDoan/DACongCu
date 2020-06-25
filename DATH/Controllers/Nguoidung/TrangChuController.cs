using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DATH.Models;
using PagedList;
using PagedList.Mvc;

namespace DATH.Controllers.Nguoidung
{
    public class TrangChuController : Controller
    {
        ActionDataContext db = new ActionDataContext();
        // GET: TrangChu
        public ActionResult Index(int? page)
        {
            NguoiDung nd = (NguoiDung)Session["Taikhoan"];
            int pageSize = 12;
            int pageNumber = (page ?? 1);
            return View(db.HoatDongs.ToList().OrderBy(n => n.IdHoatDong).Where(n => (n.TrangThai == "dd") && (n.ThoiGianBD > DateTime.Now)).ToPagedList(pageNumber, pageSize));
        }

        public ActionResult Hoatdongcualop(int? page)
        {
            if (Session["Taikhoan"] == null || Session["Taikhoan"].ToString() == "")
            {
                return RedirectToAction("Index", "Login");
            }
            else
            {
                NguoiDung nd = (NguoiDung)Session["Taikhoan"];
                int pageSize = 12;
                int pageNumber = (page ?? 1);
                ViewBag.tenlop = nd.Lop.TenLop;
                return View(db.HoatDongs.ToList().OrderBy(n => n.IdHoatDong).Where(n => (n.TrangThai == "dd") && (n.ThoiGianBD > DateTime.Now) && (n.NguoiDung.IdLop == nd.IdLop)).ToPagedList(pageNumber, pageSize));
            }
        }

        [HttpPost]
        public ActionResult KetQuaTimKiem(FormCollection f, int? page)
        {
            string sTuKhoa = f["txtTimKiem"].ToString();
            ViewBag.TuKhoa = sTuKhoa;
            List<HoatDong> lstKQTK = db.HoatDongs.Where(n=>n.TenHoatDong.Contains(sTuKhoa)).ToList();
            
            int pageNumber = (page ?? 1);
            int pageSize = 12;

            if (lstKQTK.Count() == 0 && sTuKhoa == null)
            {
                ViewBag.ThongBao = "Không tìm thấy";
                return View(db.HoatDongs.OrderBy(n => n.TenHoatDong).Where(n=>n.XoaTam==false).ToPagedList(pageNumber, pageSize));
            }
            ViewBag.ThongBao = "Đã tìm thấy " + lstKQTK.Count + " kết quả";
            return View(lstKQTK.OrderBy(n => n.TenHoatDong).Where(n => n.XoaTam == false).ToPagedList(pageNumber, pageSize));
        }

        [HttpGet]
        public ActionResult KetQuaTimKiem(string sTuKhoa, int? page)
        {
            ViewBag.TuKhoa = sTuKhoa;
            List<HoatDong> lstKQTK = db.HoatDongs.Where(n => n.TenHoatDong.Contains(sTuKhoa)).ToList();

            //phân trang
            int pageNumber = (page ?? 1);
            int pageSize = 12;

            if (lstKQTK.Count() == 0 && sTuKhoa == null)
            {
                ViewBag.ThongBao = "Không tìm thấy";

                return View(db.HoatDongs.OrderBy(n => n.TenHoatDong).Where(n => n.XoaTam == false).ToPagedList(pageNumber, pageSize));
            }
            return View(lstKQTK.OrderBy(n => n.TenHoatDong).Where(n => n.XoaTam == false).ToPagedList(pageNumber, pageSize));
        }

        public ActionResult XemChiTiet(int HoatDong = 0)
        {
            HoatDong hoatdong = db.HoatDongs.SingleOrDefault(n => n.IdHoatDong == HoatDong);
            ViewBag.a = "Đã đủ số lượng người đăng ký";
            ViewBag.b = hoatdong.SLDaDangKi;
            ViewBag.c = hoatdong.GioiHanNguoiThamGia;
            ViewBag.idhd = hoatdong.IdHoatDong;
            if (hoatdong == null)
            {
                Response.StatusCode = 404;
                return null;
            }else if(hoatdong.TrangThai == "dd")
            {

                if (Session["Taikhoan"] == null || Session["Taikhoan"].ToString() == "")
                {
                    ViewBag.dangky = "Đăng ký";
                }
                else
                {
                    NguoiDung nd = (NguoiDung)Session["Taikhoan"];
                    List<DangKiThamGiaHD> re = db.DangKiThamGiaHDs.Where(n=>(n.IdInfo == nd.IdInfo) && (n.IdHoatDong == HoatDong) && (n.XoaTam == false)).OrderBy(n => n.IdDangKiHD).ToList();
                    
                    if(re.Count == 0)
                    {
                        ViewBag.dangky = "Đăng ký";
                    }else if(re.Count == 1)
                    {
                        ViewBag.dangky = "Đã đăng ký";
                    }
                }
                return View(hoatdong);
            }
            return RedirectToAction("Index", "TrangChu");
        }

        public JsonResult Get(int id)
        {
            var position = db.HoatDongs.Where(x => x.IdHoatDong == id).FirstOrDefault();
            if (position == null) return null;
            return new JsonResult()
            {
                Data = new
                {
                    NAME = position.TenHoatDong
                },
                JsonRequestBehavior = JsonRequestBehavior.DenyGet
            };
        }

        [HttpPost]
        public string Add(HoatDong hd)
        {
            if (Session["Taikhoan"] == null || Session["Taikhoan"].ToString() == "")
            {
                return "Bạn phải đăng nhập";
            }
            else
            {
                
                NguoiDung nd = (NguoiDung)Session["Taikhoan"];
                //List<Rel_CV_Q> re = db.Rel_CV_Qs.OrderBy(n => n.IdChucVu).Where(n => n.IdChucVu == nd.IdChucVu).ToList();
                //foreach (var a in re)
                //{
                    if (nd != null)
                    {
                        hd = db.HoatDongs.SingleOrDefault(n => n.IdHoatDong == hd.IdHoatDong);
                        hd.SLDaDangKi = hd.SLDaDangKi + 1;

                        DangKiThamGiaHD dk = new DangKiThamGiaHD();
                        try
                        {
                            UpdateModel(hd);
                            db.SubmitChanges();

                            dk.IdHoatDong = hd.IdHoatDong;
                            dk.IdInfo = nd.IdInfo;
                            dk.TrangThai = false;
                            dk.XoaTam = false;
                            dk.LayCn = false;
                            db.DangKiThamGiaHDs.InsertOnSubmit(dk);
                            db.SubmitChanges();
                        }
                        catch (Exception ex)
                        {
                            return ex.Message;
                        }
                        return "ok";
                   }
                //}
            }
            
            return "Lỗi đăng ký";
        }

        public ActionResult AnhTrangChu()
        {
            if (Session["Taikhoan"] == null || Session["Taikhoan"].ToString() == "")
            {
                return PartialView();
            }
            else
            {
                NguoiDung nd = (NguoiDung)Session["Taikhoan"];
                AnhTrangChu atc = db.AnhTrangChus.SingleOrDefault();

                return PartialView(atc);
            }
        }
    }
}