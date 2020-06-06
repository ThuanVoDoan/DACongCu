using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DATH.Models;
using PagedList;
using PagedList.Mvc;
using System.IO;

namespace DATH.Controllers.Quantri
{
    public class TrangChuQtController : Controller
    {
        ActionDataContext db = new ActionDataContext();
        // GET: TrangChuQt
        public ActionResult Index(int? page)
        {
            if (Session["Taikhoan"] == null || Session["Taikhoan"].ToString() == "")
            {
                return RedirectToAction("Index", "Login");
            }
            else
            {
                int pageSize = 10;
                int pageNumber = (page ?? 1);
                NguoiDung nd = (NguoiDung)Session["Taikhoan"];
                List<Rel_CV_Q> re = db.Rel_CV_Qs.OrderBy(n => n.IdChucVu).Where(n => n.IdChucVu == nd.IdChucVu).ToList();
                foreach (var a in re)
                {
                    if (nd != null && a.Quyen.MoTa == "xdshd")
                    {
                        return View(db.HoatDongs.OrderBy(n => n.IdHoatDong).Where(n=>n.XoaTam == false).ToPagedList(pageNumber, pageSize));
                    }
                }
                return RedirectToAction("Loi", "Login");
            }
        }

        [HttpPost]
        public ActionResult KetQuaTimKiem(FormCollection f, int? page)
        {
            string sTuKhoa = f["txtTimKiem"].ToString();
            ViewBag.TuKhoa = sTuKhoa;
            List<HoatDong> lstKQTK = db.HoatDongs.Where(n => n.TenHoatDong.Contains(sTuKhoa)).ToList();

            int pageNumber = (page ?? 1);
            int pageSize = 9;

            if (lstKQTK.Count() == 0 && sTuKhoa == null)
            {
                ViewBag.ThongBao = "Không tìm thấy kết quả";
                return View(db.HoatDongs.OrderBy(n => n.TenHoatDong).Where(n => n.XoaTam == false).ToPagedList(pageNumber, pageSize));
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
            int pageSize = 9;

            if (lstKQTK.Count() == 0 && sTuKhoa == null)
            {
                ViewBag.ThongBao = "Không tìm thấy kết quả";

                return View(db.HoatDongs.OrderBy(n => n.TenHoatDong).Where(n => n.XoaTam == false).ToPagedList(pageNumber, pageSize));
            }
            return View(lstKQTK.OrderBy(n => n.TenHoatDong).Where(n => n.XoaTam == false).ToPagedList(pageNumber, pageSize));
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
        public ActionResult ThemMoi(HoatDong ds, HttpPostedFileBase fileUpload)
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
                        ds.XoaTam = false;
                        ds.HinhAnh = fileName;
                        ds.TrangThai = "cd";
                        ds.IdInfo = nd.IdInfo;
                        ds.SLDaDangKi = 0;
                        ds.ThoiGianT = DateTime.Now;
                        db.HoatDongs.InsertOnSubmit(ds);
                        db.SubmitChanges();
                    }
                }
                return RedirectToAction("Index", "TrangChuQt");
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
                List<Rel_CV_Q> re = db.Rel_CV_Qs.OrderBy(n => n.IdChucVu).Where(n => n.IdChucVu == nd.IdChucVu).ToList();
                foreach (var a in re)
                {
                    if (nd != null && a.Quyen.MoTa == "shd")
                    {
                        HoatDong sv = db.HoatDongs.SingleOrDefault(n => n.IdHoatDong == id);
                        ViewBag.IdDonViToChuc = new SelectList(db.DonViToChucs.ToList().OrderBy(n => n.IdDonViToChuc), "IdDonViToChuc", "TenDonVi",sv.IdDonViToChuc);
                        if (sv == null)
                        {
                            Response.StatusCode = 404;
                            return null;
                        }
                        return View(sv);
                    }
                }
                    
                return RedirectToAction("Index", "TrangChuQt");
            }
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult ChinhSua(HoatDong position, HttpPostedFileBase fileUpload)
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
                    if (nd != null && a.Quyen.MoTa == "shd")
                    {
                        if (fileUpload == null && nd != null)
                        {
                            HoatDong p = db.HoatDongs.SingleOrDefault(n => n.IdHoatDong == position.IdHoatDong);
                            p.TenHoatDong = position.TenHoatDong;
                            p.MoTaChinh = position.MoTaChinh;
                            p.Diadiem = position.Diadiem;
                            p.ThoiGianBD = position.ThoiGianBD;
                            p.ThoiGianKT = position.ThoiGianKT;
                            p.GioiHanNguoiThamGia = position.GioiHanNguoiThamGia;
                            p.IdDonViToChuc = position.IdDonViToChuc;
                            p.NoiDungChinhSua = position.NoiDungChinhSua;
                            p.ThoiGiand = DateTime.Now;

                            if(p.NoiDungChinhSua != null)
                            {
                                p.TrangThai = "ccs";
                                UpdateModel(p);
                                db.SubmitChanges();
                            }
                            else
                            {
                                UpdateModel(p);
                                db.SubmitChanges();
                            }

                            return RedirectToAction("Index", "TrangChuQt");
                        }
                        else if (fileUpload != null && nd != null)
                        {
                            HoatDong d = db.HoatDongs.SingleOrDefault(n => n.IdHoatDong == position.IdHoatDong);
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
                            return RedirectToAction("Index", "TrangChuQt");
                        }
                    }
                }
                return RedirectToAction("Loi", "Login");
            }
        }

        /// <summary>
        /// ajax
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>

        public JsonResult GetHoatdong(int id)
        {
            var position = db.HoatDongs.Where(x => x.IdHoatDong == id).FirstOrDefault();
            if (position == null) return null;
            return new JsonResult()
            {
                Data = new {ID = position.IdHoatDong,HINHANH = position.HinhAnh,MOTACHINH = position.MoTaChinh, NAME = position.TenHoatDong, DIADIEM = position.Diadiem,THOIGIANTAO = position.ThoiGianT,THOIGIANBATDAU = position.ThoiGianBD,THOIGIANKETTHUC = position.ThoiGianKT,
                GIOIHAN = position.GioiHanNguoiThamGia,SLDADANGKY = position.SLDaDangKi,DONVITOCHUC = position.IdDonViToChuc,NGUOITAO = position.NguoiDung.Ho_Ten, NDCHINHSUA = position.NoiDungChinhSua},
                JsonRequestBehavior = JsonRequestBehavior.DenyGet
            };
        }

        public JsonResult GetHoatdong1(int id1)
        {
            var position = db.HoatDongs.Where(x => x.IdHoatDong == id1).FirstOrDefault();
            if (position == null) return null;

            DateTime aDateTime = Convert.ToDateTime(position.ThoiGianBD);
            var day  = aDateTime.Day;
            var month = aDateTime.Month;
            var year = aDateTime.Year;
            var hours = aDateTime.Hour;
            var minute = aDateTime.Minute;

            DateTime aDateTime1 = Convert.ToDateTime(position.ThoiGianT);
            var day1 = aDateTime1.Day;
            var month1 = aDateTime1.Month;
            var year1 = aDateTime1.Year;

            DateTime aDateTime2 = Convert.ToDateTime(position.ThoiGianKT);
            var day2 = aDateTime2.Day;
            var month2 = aDateTime2.Month;
            var year2 = aDateTime2.Year;

            return new JsonResult()
            {
                Data = new
                {
                    ID = position.IdHoatDong,
                    HINHANH = position.HinhAnh,
                    MOTACHINH = position.MoTaChinh,
                    NAME = position.TenHoatDong,
                    DIADIEM = position.Diadiem,
                    THOIGIANTAO = day1 + "/" + month1 + "/" + year1,
                    THOIGIANBATDAU = day + "/" + month + "/" + year,
                    THOIGIANKETTHUC = day2 + "/" + month2 + "/" + year2,
                    GIOIHAN = position.GioiHanNguoiThamGia,
                    SLDADANGKY = position.SLDaDangKi,
                    DONVITOCHUC = position.DonViToChuc.TenDonVi,
                    NGUOITAO = position.NguoiDung.Ho_Ten,
                    NDCHINHSUA = position.NoiDungChinhSua,
                },
                JsonRequestBehavior = JsonRequestBehavior.DenyGet
            };
        }

        [HttpPost]
        public string Add(HoatDong position)
        {
            NguoiDung nd = (NguoiDung)Session["Taikhoan"];
            List<Rel_CV_Q> re = db.Rel_CV_Qs.OrderBy(n => n.IdChucVu).Where(n => n.IdChucVu == nd.IdChucVu).ToList();
            foreach (var a in re)
            {
                if (nd != null && a.Quyen.MoTa == "thd")
                {
                    try
                    {
                        if (position.ThoiGianBD < DateTime.Now || position.ThoiGianKT < DateTime.Now)
                        {
                            return "Ngày tháng không hợp lệ";
                        }
                        position.ThoiGianT = DateTime.Now;
                        position.SLDaDangKi = 0;
                        position.TrangThai = "cd";
                        position.IdInfo = nd.IdInfo;
                        position.XoaTam = false;
                        db.HoatDongs.InsertOnSubmit(position);
                        db.SubmitChanges();
                    }
                    catch (Exception ex)
                    {
                        return ex.Message;
                    }
                    return "ok";
                }
                else if (nd != null && a.Quyen.MoTa == "thd" && nd.ChucVu.MoTa == "K")
                {
                    try
                    {
                        position.ThoiGianT = DateTime.Now;
                        position.SLDaDangKi = 0;
                        position.TrangThai = "dd";
                        position.IdInfo = nd.IdInfo;
                        position.XoaTam = false;
                        db.HoatDongs.InsertOnSubmit(position);
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
        public string Edit(HoatDong position)
        {
            NguoiDung nd = (NguoiDung)Session["Taikhoan"];
            List<Rel_CV_Q> re = db.Rel_CV_Qs.OrderBy(n => n.IdChucVu).Where(n => n.IdChucVu == nd.IdChucVu).ToList();
            foreach (var a in re)
            {
                if (nd != null && a.Quyen.MoTa == "shd")
                {
                    HoatDong p = db.HoatDongs.SingleOrDefault(n => n.IdHoatDong == position.IdHoatDong);
                    //p.HinhAnh = position.HinhAnh;
                    p.TenHoatDong = position.TenHoatDong;
                    p.MoTaChinh = position.MoTaChinh;
                    p.Diadiem = position.Diadiem;
                    p.ThoiGianBD = position.ThoiGianBD;
                    p.ThoiGianKT = position.ThoiGianKT;
                    p.GioiHanNguoiThamGia = position.GioiHanNguoiThamGia;
                    p.IdDonViToChuc = position.IdDonViToChuc;
                    p.NoiDungChinhSua = position.NoiDungChinhSua;
                    p.ThoiGiand = DateTime.Now;

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
        public string DeleteHoatdong(HoatDong position)
        {
            NguoiDung nd = (NguoiDung)Session["Taikhoan"];
            List<Rel_CV_Q> re = db.Rel_CV_Qs.OrderBy(n => n.IdChucVu).Where(n => n.IdChucVu == nd.IdChucVu).ToList();
            foreach (var a in re)
            {
                if (nd != null && a.Quyen.MoTa == "xhd")
                {
                    HoatDong p = db.HoatDongs.SingleOrDefault(n => n.IdHoatDong == position.IdHoatDong);
                    List<DangKiThamGiaHD> lst = db.DangKiThamGiaHDs.Where(n => n.IdHoatDong == p.IdHoatDong).ToList();
                    if (nd.ChucVu.MoTa == "K")
                    {
                        try
                        {
                            if(lst.Count() > 0)
                            {
                                foreach (var i in lst)
                                {
                                    i.XoaTam = true;
                                    UpdateModel(i);
                                    db.SubmitChanges();

                                    p.XoaTam = true;
                                    p.TrangThai = "dh";
                                    UpdateModel(p);
                                    db.SubmitChanges();
                                }
                            }
                            else if(lst.Count() == 0)
                            {
                                p.XoaTam = true;
                                p.TrangThai = "dh";
                                UpdateModel(p);
                                db.SubmitChanges();
                            }
                        }
                        catch (Exception ex)
                        {
                            return ex.Message;
                        }
                        return "ok";
                    }
                    else if ((nd.ChucVu.MoTa == "DK" && p.DonViToChuc.MaDonVi.Trim() == "DO") || (nd.ChucVu.MoTa == "DK" && p.DonViToChuc.MaDonVi.Trim() == "LO"))
                    {
                        try
                        {
                            if (lst.Count() > 0)
                            {
                                foreach (var i in lst)
                                {
                                    i.XoaTam = true;
                                    UpdateModel(i);
                                    db.SubmitChanges();

                                    p.XoaTam = true;
                                    p.TrangThai = "dh";
                                    UpdateModel(p);
                                    db.SubmitChanges();
                                }
                            }
                            else if (lst.Count() == 0)
                            {
                                p.XoaTam = true;
                                p.TrangThai = "dh";
                                UpdateModel(p);
                                db.SubmitChanges();
                            }
                        }
                        catch (Exception ex)
                        {
                            return ex.Message;
                        }
                        return "ok";
                    }
                    return "Bạn không có quyền xóa hoạt động được tạo bởi khoa";
                }
            }
            return "Bạn không có quyền xóa";
        }

        [HttpPost]
        public string DuyetHd(HoatDong position)
        {
            NguoiDung nd = (NguoiDung)Session["Taikhoan"];
            List<Rel_CV_Q> re = db.Rel_CV_Qs.OrderBy(n => n.IdChucVu).Where(n => n.IdChucVu == nd.IdChucVu).ToList();
            foreach (var a in re)
            {
                if (nd != null && a.Quyen.MoTa == "shd" )
                {
                    HoatDong p = db.HoatDongs.SingleOrDefault(n => n.IdHoatDong == position.IdHoatDong);
                    if (nd.ChucVu.MoTa == "K")
                    {
                        if (p.TrangThai == "dd")
                        {
                            try
                            {
                                p.TrangThai = "cd";
                                UpdateModel(p);
                                db.SubmitChanges();
                            }
                            catch (Exception ex)
                            {
                                return ex.Message;
                            }
                        }
                        else if (p.TrangThai == "cd")
                        {
                            try
                            {
                                p.TrangThai = "dd";
                                p.ThoiGiand = DateTime.Now;
                                UpdateModel(p);
                                db.SubmitChanges();
                            }
                            catch (Exception ex)
                            {
                                return ex.Message;
                            }
                        }
                        else if (p.TrangThai == "dh")
                        {
                            try
                            {
                                p.TrangThai = "ccs";
                                p.XoaTam = false;
                                UpdateModel(p);
                                db.SubmitChanges();
                            }
                            catch (Exception ex)
                            {
                                return ex.Message;
                            }
                        }
                        else if (p.TrangThai == "ccs")
                        {
                            try
                            {
                                p.TrangThai = "dd";
                                p.NoiDungChinhSua = null;
                                p.ThoiGiand = DateTime.Now;
                                UpdateModel(p);
                                db.SubmitChanges();
                            }
                            catch (Exception ex)
                            {
                                return ex.Message;
                            }
                        }
                        return "ok";
                    }
                    else if((nd.ChucVu.MoTa == "DK" && p.DonViToChuc.MaDonVi.Trim() == "DO") || (nd.ChucVu.MoTa == "DK" && p.DonViToChuc.MaDonVi.Trim() == "LO"))
                    {
                        if (p.TrangThai == "dd")
                        {
                            try
                            {
                                p.TrangThai = "cd";
                                UpdateModel(p);
                                db.SubmitChanges();
                            }
                            catch (Exception ex)
                            {
                                return ex.Message;
                            }
                        }
                        else if (p.TrangThai == "cd")
                        {
                            try
                            {
                                p.TrangThai = "dd";
                                UpdateModel(p);
                                db.SubmitChanges();
                            }
                            catch (Exception ex)
                            {
                                return ex.Message;
                            }
                        }
                        else if (p.TrangThai == "dh")
                        {
                            try
                            {
                                p.XoaTam = false;
                                p.TrangThai = "ccs";
                                UpdateModel(p);
                                db.SubmitChanges();
                            }
                            catch (Exception ex)
                            {
                                return ex.Message;
                            }
                        }
                        else if (p.TrangThai == "ccs")
                        {
                            try
                            {
                                p.TrangThai = "dd";
                                p.NoiDungChinhSua = null;
                                UpdateModel(p);
                                db.SubmitChanges();
                            }
                            catch (Exception ex)
                            {
                                return ex.Message;
                            }
                        }
                        return "ok";
                    }
                    return "Bạn không có quyền xét duyệt hoạt động được tạo bởi khoa";
                }
            }
            return "Bạn không có quyền xét duyệt";
        }

        public ActionResult Hoatdongdaduyet(int? page)
        {
            if (Session["Taikhoan"] == null || Session["Taikhoan"].ToString() == "")
            {
                return RedirectToAction("Index", "Login");
            }
            else
            {
                int pageSize = 10;
                int pageNumber = (page ?? 1);
                NguoiDung nd = (NguoiDung)Session["Taikhoan"];
                List<Rel_CV_Q> re = db.Rel_CV_Qs.OrderBy(n => n.IdChucVu).Where(n => n.IdChucVu == nd.IdChucVu).ToList();
                foreach (var a in re)
                {
                    if (nd != null && a.Quyen.MoTa == "xdshddd")
                    {
                        return View(db.HoatDongs.OrderBy(n => n.ThoiGianT).Where(n=>(n.TrangThai == "dd") && (n.ThoiGianBD > DateTime.Now) && (n.XoaTam == false)).ToPagedList(pageNumber, pageSize));
                    }
                }
                return RedirectToAction("Loi", "Login");
            }
        }

        public ActionResult Hoatdongchuaduyet(int? page)
        {
            if (Session["Taikhoan"] == null || Session["Taikhoan"].ToString() == "")
            {
                return RedirectToAction("Index", "Login");
            }
            else
            {
                int pageSize = 10;
                int pageNumber = (page ?? 1);
                NguoiDung nd = (NguoiDung)Session["Taikhoan"];
                List<Rel_CV_Q> re = db.Rel_CV_Qs.OrderBy(n => n.IdChucVu).Where(n => n.IdChucVu == nd.IdChucVu).ToList();
                foreach (var a in re)
                {
                    if (nd != null && a.Quyen.MoTa == "xdshdcd")
                    {
                        return View(db.HoatDongs.OrderBy(n => n.ThoiGianT).Where(n => (n.TrangThai == "cd") && (n.XoaTam == false)).ToPagedList(pageNumber, pageSize));
                    }
                }
                return RedirectToAction("Loi", "Login");
            }
        }

        public ActionResult Hoatdongchochinhsua(int? page)
        {
            if (Session["Taikhoan"] == null || Session["Taikhoan"].ToString() == "")
            {
                return RedirectToAction("Index", "Login");
            }
            else
            {
                int pageSize = 10;
                int pageNumber = (page ?? 1);
                NguoiDung nd = (NguoiDung)Session["Taikhoan"];
                List<Rel_CV_Q> re = db.Rel_CV_Qs.OrderBy(n => n.IdChucVu).Where(n => n.IdChucVu == nd.IdChucVu).ToList();
                foreach (var a in re)
                {
                    if (nd != null && a.Quyen.MoTa == "xdshdchs")
                    {
                        return View(db.HoatDongs.OrderBy(n => n.IdHoatDong).Where(n => (n.TrangThai == "ccs") && (n.XoaTam == false)).ToPagedList(pageNumber, pageSize));
                    }
                }
                return RedirectToAction("Loi", "Login");
            }
        }

        public ActionResult Hoatdongdahuy(int? page)
        {
            if (Session["Taikhoan"] == null || Session["Taikhoan"].ToString() == "")
            {
                return RedirectToAction("Index", "Login");
            }
            else
            {
                int pageSize = 10;
                int pageNumber = (page ?? 1);
                NguoiDung nd = (NguoiDung)Session["Taikhoan"];
                List<Rel_CV_Q> re = db.Rel_CV_Qs.OrderBy(n => n.IdChucVu).Where(n => n.IdChucVu == nd.IdChucVu).ToList();
                foreach (var a in re)
                {
                    if (nd != null && a.Quyen.MoTa == "xdshddh")
                    {
                        return View(db.HoatDongs.OrderBy(n => n.IdHoatDong).Where(n => (n.TrangThai == "dh") && (n.XoaTam == true)).ToPagedList(pageNumber, pageSize));
                    }
                }
                return RedirectToAction("Loi", "Login");
            }
        }

        [HttpPost]
        public ActionResult Sapxep(FormCollection form, int? page)
        {
            int pageNumber = (page ?? 1);
            int pageSize = 10;
            string sTuKhoa = form["sapxep"].ToString();
            ViewBag.TuKhoa = sTuKhoa;
            if (sTuKhoa == "1")
            {
                ViewBag.lhd = "mới đăng";
                return View(db.HoatDongs.OrderByDescending(n=>n.ThoiGiand).Where(n => (n.TrangThai == "dd") && (n.ThoiGianBD >= DateTime.Now)).ToPagedList(pageNumber, pageSize));
            }
            else if(sTuKhoa == "2")
            {
                ViewBag.lhd = "sắp diễn ra";
                return View(db.HoatDongs.OrderBy(n => n.ThoiGianBD).Where(n=> (n.TrangThai == "dd") && (n.ThoiGianBD >= DateTime.Now)).ToPagedList(pageNumber, pageSize));
            }
            return RedirectToAction("Index", "TrangChuQt");
        }

        [HttpGet]
        public ActionResult Sapxep(string sTuKhoa, int? page)
        {
            int pageNumber = (page ?? 1);
            int pageSize = 10;
            ViewBag.TuKhoa = sTuKhoa;
            if (sTuKhoa == "1")
            {
                return View(db.HoatDongs.OrderByDescending(n => n.ThoiGiand).Where(n => (n.TrangThai == "dd") && (n.ThoiGianBD >= DateTime.Now)).ToPagedList(pageNumber, pageSize));
            }
            else if (sTuKhoa == "2")
            {
                return View(db.HoatDongs.OrderBy(n => n.ThoiGianBD).Where(n => (n.TrangThai == "dd") && (n.ThoiGianBD >= DateTime.Now)).ToPagedList(pageNumber, pageSize));
            }
            return RedirectToAction("Index", "TrangChuQt");
        }

        public ActionResult Hoatdongdangdienra(int? page)
        {
            if (Session["Taikhoan"] == null || Session["Taikhoan"].ToString() == "")
            {
                return RedirectToAction("Index", "Login");
            }
            else
            {
                int pageSize = 10;
                int pageNumber = (page ?? 1);
                NguoiDung nd = (NguoiDung)Session["Taikhoan"];
                List<Rel_CV_Q> re = db.Rel_CV_Qs.OrderBy(n => n.IdChucVu).Where(n => n.IdChucVu == nd.IdChucVu).ToList();
                foreach (var a in re)
                {
                    if (nd != null)
                    {
                        return View(db.HoatDongs.OrderBy(n => n.ThoiGianT).Where(n => (n.TrangThai == "dd") && (n.ThoiGianBD <= DateTime.Now) && (n.ThoiGianKT >= DateTime.Now) && (n.XoaTam == false)).ToPagedList(pageNumber, pageSize));
                    }
                }
                return RedirectToAction("Loi", "Login");
            }
        }

        public ActionResult Hoatdongchuadienra(int? page)
        {
            if (Session["Taikhoan"] == null || Session["Taikhoan"].ToString() == "")
            {
                return RedirectToAction("Index", "Login");
            }
            else
            {
                int pageSize = 10;
                int pageNumber = (page ?? 1);
                NguoiDung nd = (NguoiDung)Session["Taikhoan"];
                List<Rel_CV_Q> re = db.Rel_CV_Qs.OrderBy(n => n.IdChucVu).Where(n => n.IdChucVu == nd.IdChucVu).ToList();
                foreach (var a in re)
                {
                    if (nd != null)
                    {
                        return View(db.HoatDongs.OrderBy(n => n.ThoiGianT).Where(n => (n.TrangThai == "dd") && (n.ThoiGianBD > DateTime.Now) && (n.XoaTam == false)).ToPagedList(pageNumber, pageSize));
                    }
                }
                return RedirectToAction("Loi", "Login");
            }
        }

        public ActionResult Hoatdongdaketthuc(int? page)
        {
            if (Session["Taikhoan"] == null || Session["Taikhoan"].ToString() == "")
            {
                return RedirectToAction("Index", "Login");
            }
            else
            {
                int pageSize = 10;
                int pageNumber = (page ?? 1);
                NguoiDung nd = (NguoiDung)Session["Taikhoan"];
                List<Rel_CV_Q> re = db.Rel_CV_Qs.OrderBy(n => n.IdChucVu).Where(n => n.IdChucVu == nd.IdChucVu).ToList();
                foreach (var a in re)
                {
                    if (nd != null)
                    {
                        return View(db.HoatDongs.OrderBy(n => n.ThoiGianT).Where(n => (n.TrangThai == "dd") && (n.ThoiGianKT < DateTime.Now) && (n.XoaTam == false)).ToPagedList(pageNumber, pageSize));
                    }
                }
                return RedirectToAction("Loi", "Login");
            }
        }
    }
}