using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DATH.Models;
using System.IO;
using Excel = Microsoft.Office.Interop.Excel;

namespace DATH.Controllers.Quantri
{
    public class DsSinhvienTheohdController : Controller
    {
        ActionDataContext db = new ActionDataContext();

        // GET: DsSinhvienTheohd
        [AcceptVerbs(HttpVerbs.Get | HttpVerbs.Post)]
        public ActionResult Index(int id, HttpPostedFileBase fileUpload, HttpPostedFileBase excelfile)
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
                    if (nd != null && a.Quyen.MoTa == "xdssvthd" && fileUpload == null && excelfile == null)
                    {
                        HoatDong hd = db.HoatDongs.SingleOrDefault(n => n.IdHoatDong == id);
                        List<DangKiThamGiaHD> lstdk = db.DangKiThamGiaHDs.OrderBy(n => n.IdDangKiHD).ToList();
                        ViewBag.tenhd = hd.TenHoatDong;
                        ViewBag.bg = hd.Background;
                        ViewBag.idhd = id;
                        ViewBag.tgkt = hd.ThoiGianKT;
                        ViewBag.tgbd = hd.ThoiGianBD;
                        return View(lstdk.Where(n => (n.IdHoatDong == hd.IdHoatDong) && (n.XoaTam == false)).ToList());
                    }
                    if (nd != null && a.Quyen.MoTa == "tbgcn" && fileUpload != null)
                    {
                        HoatDong hd = db.HoatDongs.SingleOrDefault(n => n.IdHoatDong == id);
                        List<DangKiThamGiaHD> lstdk = db.DangKiThamGiaHDs.OrderBy(n => n.IdDangKiHD).ToList();
                        var fileName = Path.GetFileName(fileUpload.FileName);
                        var path = Path.Combine(Server.MapPath("~/images"), fileName);
                        ViewBag.chucvu = nd.ChucVu.MoTa;
                        if (System.IO.File.Exists(path))
                            ViewBag.ThongBao = "Hình ảnh đã tồn tại";
                        else
                        {
                            fileUpload.SaveAs(path);
                        }
                        hd.Background = fileName;
                        UpdateModel(hd);
                        db.SubmitChanges();
                        ViewBag.tenhd = hd.TenHoatDong;
                        ViewBag.bg = hd.Background;
                        ViewBag.idhd = id;
                        ViewBag.tgkt = hd.ThoiGianKT;
                        ViewBag.tgbd = hd.ThoiGianBD;
                        return View(lstdk.Where(n => (n.IdHoatDong == hd.IdHoatDong) && (n.XoaTam == false)).ToList());
                    }
                    if (nd != null && excelfile != null)
                    {
                        HoatDong hd = db.HoatDongs.SingleOrDefault(n => n.IdHoatDong == id);
                        List<DangKiThamGiaHD> lstdk = db.DangKiThamGiaHDs.OrderBy(n => n.IdDangKiHD).ToList();
                        ViewBag.tenhd = hd.TenHoatDong;
                        ViewBag.bg = hd.Background;
                        ViewBag.idhd = id;
                        ViewBag.tgkt = hd.ThoiGianKT;
                        ViewBag.tgbd = hd.ThoiGianBD;
                        if (excelfile.FileName.EndsWith("xls") || excelfile.FileName.EndsWith("xlsx"))
                        {
                            string path = Server.MapPath("~/Excel/" + excelfile.FileName);
                            if (System.IO.File.Exists(path))
                            {
                                System.IO.File.Delete(path);
                            }
                            excelfile.SaveAs(path);
                            // đọc dữ liệu từ file excel
                            Excel.Application application = new Excel.Application();
                            Excel.Workbook workbook = application.Workbooks.Open(path);
                            Excel.Worksheet worksheet = workbook.ActiveSheet;
                            try
                            {
                                Excel.Range range = worksheet.UsedRange;
                                List<DangKiThamGiaHD> listds = new List<DangKiThamGiaHD>();
                                for (int row = 2; row <= range.Rows.Count; row++)
                                {
                                    DangKiThamGiaHD ds = new DangKiThamGiaHD();
                                    var mssv = string.Concat(((Excel.Range)range.Cells[row, 2]).Text);
                                    List<NguoiDung> lstsv = db.NguoiDungs.ToList();
                                    foreach (var fi in lstsv)
                                    {
                                        if (fi.SoId.Trim() == mssv)
                                        {
                                            ds.IdInfo = fi.IdInfo;
                                        }
                                    }

                                    ds.IdHoatDong = id;
                                    ds.TrangThai = Boolean.Parse(((Excel.Range)range.Cells[row, 4]).Text);
                                    ds.XoaTam = false;
                                    listds.Add(ds);
                                }
                                workbook.Close(0);
                                application.Quit();
                                try
                                {
                                    if (ModelState.IsValid)
                                    {
                                        foreach (var c in lstdk)
                                        {
                                            foreach (var d in listds)
                                            {
                                                List<DangKiThamGiaHD> lstdk2 = db.DangKiThamGiaHDs.Where(n => (n.IdInfo == d.IdInfo) && (n.IdHoatDong == id)).OrderBy(n => n.IdDangKiHD).ToList();
                                                if (lstdk2.Count == 0)
                                                {
                                                    db.DangKiThamGiaHDs.InsertOnSubmit(d);
                                                    db.SubmitChanges();
                                                }
                                                foreach (var y in lstdk2)
                                                {
                                                    List<DangKiThamGiaHD> lstdk1 = db.DangKiThamGiaHDs.Where(n => (n.IdDangKiHD == y.IdDangKiHD) && (n.IdInfo == d.IdInfo) && (n.IdHoatDong == id)).OrderBy(n => n.IdDangKiHD).ToList();
                                                    if (lstdk1.Count == 1)
                                                    {
                                                        foreach (var t in lstdk1)
                                                        {
                                                            t.TrangThai = d.TrangThai;
                                                            t.XoaTam = d.XoaTam;
                                                            UpdateModel(t);
                                                            db.SubmitChanges();
                                                        }
                                                    }
                                                }

                                            }
                                        }
                                    }
                                }
                                catch (Exception e)
                                {
                                    Console.WriteLine(e);
                                    db.SubmitChanges();
                                }
                            }
                            catch (Exception ex)
                            {
                                workbook.Close(0);
                                application.Quit();
                            }

                            //return View(lstdk.Where(n => (n.IdHoatDong == hd.IdHoatDong) && (n.XoaTam == false)).ToList());
                            return RedirectToAction("Index", "DsSinhvienTheohd", id);
                        }
                        else
                        {
                            ViewBag.Error = "Bạn chưa chọn file đúng !";
                            return View(lstdk.Where(n => (n.IdHoatDong == hd.IdHoatDong) && (n.XoaTam == false)).ToList());
                        }
                    }
                }
                return Content("Bạn không có quyền này");
            }
        }

        public JsonResult Get(int id)
        {
            var position = db.DangKiThamGiaHDs.Where(x => x.IdDangKiHD == id).FirstOrDefault();
            if (position == null) return null;
            return new JsonResult()
            {
                Data = new { IDHD = position.HoatDong.IdHoatDong, MASO = position.NguoiDung.SoId, NAME = position.NguoiDung.Ho_Ten },
                JsonRequestBehavior = JsonRequestBehavior.DenyGet
            };
        }

        public JsonResult Gethd(int id)
        {
            var position = db.HoatDongs.Where(x => x.IdHoatDong == id).FirstOrDefault();
            if (position == null) return null;
            return new JsonResult()
            {
                Data = new
                {
                    ID = position.IdHoatDong,
                    HINHANH = position.HinhAnh,
                    MOTACHINH = position.MoTaChinh,
                    NAME = position.TenHoatDong,
                    DIADIEM = position.Diadiem,
                    THOIGIANTAO = position.ThoiGianT,
                    THOIGIANBATDAU = position.ThoiGianBD,
                    THOIGIANKETTHUC = position.ThoiGianKT,
                    GIOIHAN = position.GioiHanNguoiThamGia,
                    SLDADANGKY = position.SLDaDangKi,
                    DONVITOCHUC = position.IdDonViToChuc,
                    NGUOITAO = position.NguoiDung.Ho_Ten,
                    NDCHINHSUA = position.NoiDungChinhSua
                },
                JsonRequestBehavior = JsonRequestBehavior.DenyGet
            };
        }

        [HttpPost]
        public string Delete(DangKiThamGiaHD position)
        {
            NguoiDung nd = (NguoiDung)Session["Taikhoan"];
            List<Rel_CV_Q> re = db.Rel_CV_Qs.OrderBy(n => n.IdChucVu).Where(n => n.IdChucVu == nd.IdChucVu).ToList();
            foreach (var a in re)
            {
                if (nd != null && a.Quyen.MoTa == "xsvthd")
                {
                    DangKiThamGiaHD p = db.DangKiThamGiaHDs.SingleOrDefault(n => n.IdDangKiHD == position.IdDangKiHD);

                    try
                    {
                        p.XoaTam = true;
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
        public string Add(DangKiThamGiaHD position, HoatDong hd, int id = 0)
        {
            NguoiDung nd = (NguoiDung)Session["Taikhoan"];
            List<Rel_CV_Q> re = db.Rel_CV_Qs.OrderBy(n => n.IdChucVu).Where(n => n.IdChucVu == nd.IdChucVu).ToList();
            foreach (var a in re)
            {
                if (nd != null)
                {
                    hd = db.HoatDongs.SingleOrDefault(n => n.IdHoatDong == id);
                    hd.SLDaDangKi = hd.SLDaDangKi + 1;
                    try
                    {
                        UpdateModel(hd);
                        db.SubmitChanges();

                        position.IdHoatDong = id;
                        position.XoaTam = false;
                        position.TrangThai = false;
                        db.DangKiThamGiaHDs.InsertOnSubmit(position);
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
        public string Xacnhanthamgia(DangKiThamGiaHD position)
        {
            NguoiDung nd = (NguoiDung)Session["Taikhoan"];
            List<Rel_CV_Q> re = db.Rel_CV_Qs.OrderBy(n => n.IdChucVu).Where(n => n.IdChucVu == nd.IdChucVu).ToList();
            foreach (var a in re)
            {
                if (nd != null)
                {
                    DangKiThamGiaHD p = db.DangKiThamGiaHDs.SingleOrDefault(n => n.IdDangKiHD == position.IdDangKiHD);
                    if (p.TrangThai == true)
                    {
                        try
                        {
                            p.TrangThai = false;
                            UpdateModel(p);
                            db.SubmitChanges();
                        }
                        catch (Exception ex)
                        {
                            return ex.Message;
                        }
                        return "ok";
                    }
                    else if (p.TrangThai == false)
                    {
                        try
                        {
                            p.TrangThai = true;
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
            }
            return "Bạn không có quyền này";
        }

        public ActionResult InChungNhan(int id)
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
                    DangKiThamGiaHD ds = db.DangKiThamGiaHDs.SingleOrDefault(n => n.IdDangKiHD == id);

                    var a = ds.HoatDong.ThoiGianBD;
                    DateTime aDateTime = Convert.ToDateTime(a);
                    ViewBag.day = aDateTime.Day;
                    ViewBag.month = aDateTime.Month;
                    ViewBag.year = aDateTime.Year;
                    ViewBag.scn = ds.IdDangKiHD;
                    if (ds == null)
                    {
                        Response.StatusCode = 404;
                        return null;
                    }
                    return View(ds);
                }
                else
                {
                    return RedirectToAction("Index", "Main");
                }
            }
        }

        [HttpPost]
        public string LayCn(DangKiThamGiaHD position)
        {
            DangKiThamGiaHD p = db.DangKiThamGiaHDs.SingleOrDefault(n => n.IdDangKiHD == position.IdDangKiHD);
            if (p.LayCn == false)
            {
                try
                {
                    p.LayCn = true;
                    UpdateModel(p);
                    db.SubmitChanges();
                }
                catch (Exception ex)
                {
                    return ex.Message;
                }
                return "ok";
            }
            return "ko";
        }
    }
}