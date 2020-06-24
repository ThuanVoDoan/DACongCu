using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DATH.Models;
using Excel = Microsoft.Office.Interop.Excel;
using System.Security.Cryptography;
using System.Text;

namespace DATH.Controllers.Quantri
{
    public class DsSinhvienTunglopController : Controller
    {
        ActionDataContext db = new ActionDataContext();
        // GET: DsSinhvienTunglop
        [AcceptVerbs(HttpVerbs.Get | HttpVerbs.Post)]
        public ActionResult Index(int id, HttpPostedFileBase excelfile)
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
                    if (nd != null && excelfile != null)
                    {
                        Lop hd = db.Lops.SingleOrDefault(n => n.IdLop == id);
                        List<NguoiDung> lstdk = db.NguoiDungs.OrderBy(n => n.IdInfo).ToList();
                        ViewBag.tenlop = hd.TenLop;
                        ViewBag.idlop = hd.IdLop;

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
                                List<NguoiDung> listds = new List<NguoiDung>();
                                for (int row = 2; row <= range.Rows.Count; row++)
                                {
                                    NguoiDung ds = new NguoiDung();
                                    ds.SoId = string.Concat(((Excel.Range)range.Cells[row, 2]).Text);
                                    ds.Ho_Ten = string.Concat(((Excel.Range)range.Cells[row, 3]).Text);
                                    ds.SDT = string.Concat(((Excel.Range)range.Cells[row, 4]).Text);
                                    ds.IdChucVu = 4;
                                    ds.TaiKhoan = ds.SoId;

                                    string pass = string.Concat(((Excel.Range)range.Cells[row, 7]).Text);
                                    MD5CryptoServiceProvider md5 = new MD5CryptoServiceProvider();

                                    byte[] bHash = md5.ComputeHash(Encoding.UTF8.GetBytes(pass));

                                    StringBuilder sbHash = new StringBuilder();

                                    foreach (byte b in bHash)
                                    {

                                        sbHash.Append(String.Format("{0:x2}", b));

                                    }
                                    pass = sbHash.ToString();

                                    ds.MatKhau = pass;
                                    ds.IdLop = id;
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
                                                List<NguoiDung> lstdk2 = db.NguoiDungs.Where(n => n.SoId.Trim() == d.SoId.Trim()).OrderBy(n => n.IdInfo).ToList();
                                                if (lstdk2.Count == 0)
                                                {
                                                    db.NguoiDungs.InsertOnSubmit(d);
                                                    db.SubmitChanges();
                                                }

                                                foreach (var y in lstdk2)
                                                {
                                                    List<NguoiDung> lstdk1 = db.NguoiDungs.Where(n => (n.IdInfo == y.IdInfo) && (n.SoId.Trim() == d.SoId.Trim()) && (n.XoaTam == false)).OrderBy(n => n.IdInfo).ToList();
                                                    if (lstdk1.Count == 1)
                                                    {
                                                        foreach (var t in lstdk1)
                                                        {
                                                            t.SoId = d.SoId;
                                                            t.Ho_Ten = d.Ho_Ten;
                                                            t.SDT = d.SDT;
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
                            return RedirectToAction("Index", "DsSinhvienTunglop", id);
                            //return View(lstdk.Where(n => (n.IdLop == hd.IdLop) && (n.XoaTam == false)).ToList());
                        }
                        else
                        {
                            ViewBag.Error = "Bạn chưa chọn file đúng !";
                            return View(lstdk.Where(n => (n.IdLop == hd.IdLop) && (n.XoaTam == false)).ToList());
                        }
                    }
                    else if (nd != null && a.Quyen.MoTa == "xdssvtl" && excelfile == null)
                    {
                        Lop hd = db.Lops.SingleOrDefault(n => n.IdLop == id);
                        List<NguoiDung> lstdk = db.NguoiDungs.OrderBy(n => n.IdInfo).ToList();
                        ViewBag.tenlop = hd.TenLop;
                        ViewBag.idlop = hd.IdLop;
                        return View(lstdk.Where(n => (n.IdLop == hd.IdLop) && (n.XoaTam == false)).ToList());
                    }
                    
                }
                return RedirectToAction("Loi", "Login");
            }
        }

        public JsonResult Get(int id)
        {
            var position = db.NguoiDungs.Where(x => x.IdInfo == id).FirstOrDefault();
            if (position == null) return null;
            var a  = new SelectList(db.Lops.ToList().OrderBy(n => n.IdLop), "IdLop", "TenLop", position.IdLop);
            return new JsonResult()
            {
                Data = new {CV = position.IdChucVu ,MASO = position.SoId, NAME = position.Ho_Ten, SDT = position.SDT, LOP = position.Lop.TenLop.Trim(), IdLop = position.IdLop, TAIKHOAN = position.TaiKhoan, MATKHAU = position.MatKhau, SE = a  },
                JsonRequestBehavior = JsonRequestBehavior.DenyGet
            };
        }

        [HttpPost]
        public string Add(NguoiDung position, string pass)
        {
            NguoiDung nd = (NguoiDung)Session["Taikhoan"];
            List<Rel_CV_Q> re = db.Rel_CV_Qs.OrderBy(n => n.IdChucVu).Where(n => n.IdChucVu == nd.IdChucVu).ToList();
            foreach (var a in re)
            {
                if (nd != null && a.Quyen.MoTa == "tttsv")
                {
                    try
                    {
                        position.MatKhau = "8888";

                        position.XoaTam = false;
                        position.IdChucVu = 4;
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
            return "Bạn không có quyền thêm";
        }

        [HttpPost]
        public string Edit(NguoiDung position, string Tenlop)
        {
            NguoiDung nd = (NguoiDung)Session["Taikhoan"];
            List<Rel_CV_Q> re = db.Rel_CV_Qs.OrderBy(n => n.IdChucVu).Where(n => n.IdChucVu == nd.IdChucVu).ToList();
            foreach (var a in re)
            {
                if (nd != null && a.Quyen.MoTa == "sttsv")
                {
                    NguoiDung p = db.NguoiDungs.SingleOrDefault(n => n.IdInfo == position.IdInfo);
                    List<Lop> lstsv = db.Lops.ToList();
                    try
                    {
                        foreach (var fi in lstsv)
                        {
                            if (fi.TenLop.Trim().Equals(Tenlop.Trim()))
                            {
                                p.IdLop = fi.IdLop;
                                UpdateModel(p);
                                db.SubmitChanges();
                            }
                        }
                        //UpdateModel(p);
                        //db.SubmitChanges();
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
        public string Delete(NguoiDung position, HoatDong hd, DangKiThamGiaHD dk)
        {
            NguoiDung nd = (NguoiDung)Session["Taikhoan"];
            List<Rel_CV_Q> re = db.Rel_CV_Qs.OrderBy(n => n.IdChucVu).Where(n => n.IdChucVu == nd.IdChucVu).ToList();
            foreach (var a in re)
            {
                if (nd != null && a.Quyen.MoTa == "xsvthd")
                {
                    NguoiDung p = db.NguoiDungs.SingleOrDefault(n => n.IdInfo == position.IdInfo);
                    p.XoaTam = true;

                    try
                    {
                        List<DangKiThamGiaHD> lstdk = db.DangKiThamGiaHDs.OrderBy(n => n.IdDangKiHD).Where(n => n.IdInfo == position.IdInfo).ToList();
                        
                        foreach (var a1 in lstdk)
                        {
                            List<HoatDong> lsthd = db.HoatDongs.OrderBy(n => n.IdHoatDong).Where(n => n.IdHoatDong == a1.IdHoatDong).ToList();
                            foreach (var a2 in lsthd)
                            {
                                a2.SLDaDangKi = a2.SLDaDangKi - 1;
                                UpdateModel(a2);
                                db.SubmitChanges();
                            }
                            a1.XoaTam = true;
                            UpdateModel(a1);
                            db.SubmitChanges();
                        }

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