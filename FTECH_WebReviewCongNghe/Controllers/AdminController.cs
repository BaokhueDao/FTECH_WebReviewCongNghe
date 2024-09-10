using System;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using FTECH_WebReviewCongNghe.Models;
using PagedList;

namespace FTECH_WebReviewCongNghe.Controllers
{
    public class AdminController : Controller
    {
        private string GetUserRole()
        {
            TAIKHOAN tk = (TAIKHOAN)Session["account"];
            return tk?.CHUCVU.TENCHUCVU;
        }

        public ActionResult Index(int? index)
        {
            string role = GetUserRole();
            if (role == null)
            {
                return RedirectToAction("signin", "Login");
            }

            dbReviewDoCongNgheFTECHEntities db = new dbReviewDoCongNgheFTECHEntities();
            int pageSize = 8;
            if (index == null)
            {
                index = 1;
            }
            var pagedList = db.BAIVIET.OrderBy(m => m.MABAIVIET).ToPagedList((int)index, pageSize);
            PageRS pageRS = new PageRS(pagedList.ToList(), (int)index, pagedList.PageSize, pagedList.PageCount);
            ViewBag.pageRS = pageRS;
            ViewBag.dsDoitac = db.DOITAC.ToList();
            ViewBag.dsTaiKhoan = db.TAIKHOAN.ToList();
            // Điều hướng đến view tương ứng dựa trên vai trò
            switch (role)
            {
                case "ADMIN":
                    // ADMIN có thể truy cập tất cả các trang
                    return View("AccountManage");
                case "USER ACCOUNT MANAGEMENT":
                    if (role == "ADMIN" || role == "USER ACCOUNT MANAGEMENT")
                        return View("AccountManage");
                    break;
                case "AFFILIATE PARTNER MANAGEMENT":
                    if (role == "ADMIN" || role == "AFFILIATE PARTNER MANAGEMENT")
                        return View("PartnerManage");
                    break;
                case "AFFILIATE MANAGEMENT":
                    if (role == "ADMIN" || role == "AFFILIATE MANAGEMENT")
                        return View("CategoryManage");
                    break;
                case "CONTENT MANAGEMENT":
                    if (role == "ADMIN" || role == "CONTENT MANAGEMENT")
                        return View("PostManage");
                    break;
                case "USER":
                    if (role == "ADMIN" || role == "USER")
                        return View("~/Home/Index");
                    break;
                default:
                    return RedirectToAction("signin", "Login");
            }

            // Nếu không khớp với bất kỳ điều kiện nào
            return RedirectToAction("signin", "Login");
        }      

        //------------------TAIKHOAN---------------------

        public ActionResult AccountManage()
        {
            string role = GetUserRole();
            if (role == null || role != "ADMIN")
            {
                return RedirectToAction("signin", "Login");
            }

            dbReviewDoCongNgheFTECHEntities db = new dbReviewDoCongNgheFTECHEntities();
            ViewBag.dsTaiKhoan = db.TAIKHOAN.ToList();
            return View();
        }
        public ActionResult addAccount()
        {
            string role = GetUserRole();
            if (role == null || role != "ADMIN")
            {
                return RedirectToAction("signin", "Login");
            }

            dbReviewDoCongNgheFTECHEntities db = new dbReviewDoCongNgheFTECHEntities();
            ViewBag.dsChucVu = db.CHUCVU.ToList();
            ViewBag.dsTAIKHOAN = db.TAIKHOAN.ToList();
            ViewBag.tk = new FTECH_WebReviewCongNghe.Models.TAIKHOAN();
            return View();
        }

        [HttpPost]
        public ActionResult AddAccount(TAIKHOAN model)
        {
            string role = GetUserRole();
            if (role == null || role != "ADMIN")
            {
                return RedirectToAction("signin", "Login");
            }

            dbReviewDoCongNgheFTECHEntities db = new dbReviewDoCongNgheFTECHEntities();
            if (db.TAIKHOAN.Any(t => t.EMAIL == model.EMAIL))
            {
                ModelState.AddModelError("UsernameExists", "Username already exists.");
                ViewBag.dsChucVu = db.CHUCVU.ToList();
                return View(model);
            }

            model.TRANGTHAI = 1;
            db.TAIKHOAN.Add(model);
            db.SaveChanges();
            return RedirectToAction("AccountManage");
        }


        [HttpGet]
        public ActionResult updateAccount(int? ID)
        {
            string role = GetUserRole();
            if (role == null || role != "ADMIN")
            {
                return RedirectToAction("AccountManage");
            }

            if (ID == null)
            {
                return RedirectToAction("AccountManage");
            }
            dbReviewDoCongNgheFTECHEntities db = new dbReviewDoCongNgheFTECHEntities();
            TAIKHOAN tk = db.TAIKHOAN.Find(ID);
            ViewBag.tk = tk;
            ViewBag.dsChucVu = db.CHUCVU.ToList();
            return View();
        }

        [HttpPost]
        public ActionResult updateAccount(TAIKHOAN tk)
        {
            dbReviewDoCongNgheFTECHEntities db = new dbReviewDoCongNgheFTECHEntities();
            TAIKHOAN tkOld = db.TAIKHOAN.Find(tk.ID);
            tkOld.MATKHAU = tk.MATKHAU;
            tkOld.CHUCVU = db.CHUCVU.Find(tk.IDCHUCVU);
            db.SaveChanges();
            return RedirectToAction("AccountManage");
        }

        [HttpPost]
        public void deleteAccount(int? ID)
        {
            string role = GetUserRole();
            if (role == null || role != "ADMIN")
            {
                return;
            }

            dbReviewDoCongNgheFTECHEntities db = new dbReviewDoCongNgheFTECHEntities();
            TAIKHOAN tk = db.TAIKHOAN.Find(ID);
            tk.TRANGTHAI = 0;
            db.SaveChanges();
        }

        [HttpPost]
        public void unKeyAccount(int? ID)
        {
            string role = GetUserRole();
            if (role == null || role != "ADMIN")
            {
                return;
            }

            dbReviewDoCongNgheFTECHEntities db = new dbReviewDoCongNgheFTECHEntities();
            TAIKHOAN tk = db.TAIKHOAN.Find(ID);
            tk.TRANGTHAI = 1;
            db.SaveChanges();
        }


        //--------------------DANHMUC---------------------

        public ActionResult CategoryManage()
        {
            string role = GetUserRole();
            if (role == null)
            {
                return RedirectToAction("signin", "Login");
            }

            dbReviewDoCongNgheFTECHEntities db = new dbReviewDoCongNgheFTECHEntities();
            ViewBag.dsDanhMuc = db.DANHMUC.ToList();
            return View();
        }

        public ActionResult addCategory()
        {
            string role = GetUserRole();
            if (role == null)
            {
                return RedirectToAction("signin", "Login");
            }
            return View();
        }

        [HttpPost]
        public ActionResult addCategory(DANHMUC model, HttpPostedFileBase fileAnh)
        {
            string role = GetUserRole();
            if (role == null)
            {
                return RedirectToAction("signin", "Login");
            }

            dbReviewDoCongNgheFTECHEntities db = new dbReviewDoCongNgheFTECHEntities();

            /*  if (fileAnh.ContentLength > 0)
              {
                  String rootFolder = Server.MapPath("/MySource/img/product/");
                  String pathImage = rootFolder + fileAnh.FileName;
                  fileAnh.SaveAs(pathImage);
                  model.HINHANH = fileAnh.FileName;
              }*/

            db.DANHMUC.Add(model);
            db.SaveChanges();
            return RedirectToAction("CategoryManage");
        }

        public ActionResult updateCategory(int? ID)
        {
            string role = GetUserRole();
            if (role == null)
            {
                return RedirectToAction("signin", "Login");
            }
            if (ID == null)
            {
                return RedirectToAction("CategoryManage");
            }
            dbReviewDoCongNgheFTECHEntities db = new dbReviewDoCongNgheFTECHEntities();
            DANHMUC dm = db.DANHMUC.Find(ID);
            ViewBag.dm = dm;
            return View();
        }

        [HttpPost]
        public ActionResult updateCategory(DANHMUC model, HttpPostedFileBase fileAnh)
        {
            string role = GetUserRole();
            if (role == null)
            {
                return RedirectToAction("signin", "Login");
            }

            dbReviewDoCongNgheFTECHEntities db = new dbReviewDoCongNgheFTECHEntities();
            DANHMUC dm = db.DANHMUC.Find(model.MADANHMUC);

            /*    if (fileAnh != null)
                {
                    String rootFolder = Server.MapPath("/MySource/img/product/");
                    String pathImage = rootFolder + fileAnh.FileName;
                    fileAnh.SaveAs(pathImage);
                    dm.HINHANH = fileAnh.FileName;
                }*/

            dm.TENDANHMUC = model.TENDANHMUC;
            //  dm.MOTA = model.MOTA;
            db.SaveChanges();
            return RedirectToAction("CategoryManage");
        }

        [HttpPost]
        public void deleteCategory(int? MADANHMUC)
        {
            string role = GetUserRole();
            if (role == null)
            {
                RedirectToAction("signin", "Login");
                return;
            }

            dbReviewDoCongNgheFTECHEntities db = new dbReviewDoCongNgheFTECHEntities();
            var danhMucDelete = db.DANHMUC.Where(m => m.MADANHMUC == MADANHMUC).SingleOrDefault();
            db.DANHMUC.Remove(danhMucDelete);
            db.SaveChanges();
        }




        //--------------DOITAC---------------
        public ActionResult PartnerManage()
        {
            string role = GetUserRole();
            if (role == null || (role != "AFFILIATE PARTNER MANAGEMENT" && role != "ADMIN"))
            {
                return RedirectToAction("signin", "Login");
            }

            using (var db = new dbReviewDoCongNgheFTECHEntities())
            {
                ViewBag.dsDoitac = db.DOITAC.ToList();
                return View();
            }
        }
        public ActionResult addPartner()
        {
            string role = GetUserRole();
            if (role == null || (role != "AFFILIATE PARTNER MANAGEMENT" && role != "ADMIN"))
            {
                return RedirectToAction("signin", "Login");
            }

           using (var db = new dbReviewDoCongNgheFTECHEntities())
            {
                ViewBag.dsDoitac = db.DOITAC.ToList();
                ViewBag.dt = new FTECH_WebReviewCongNghe.Models.DOITAC();
                return View();
            }
        }
        [HttpPost]
        public ActionResult addPartner(DOITAC dtmodel, HttpPostedFileBase HOPDONG, string THOIGIANBATDAU)
        {
            string role = GetUserRole();
            if (role == null || (role != "AFFILIATE PARTNER MANAGEMENT" && role != "ADMIN"))
            {
                return RedirectToAction("signin", "Login");
            }

            using (var db = new dbReviewDoCongNgheFTECHEntities())
            {
                if (HOPDONG != null && HOPDONG.ContentLength > 0)
                {
                    string rootFolder = Server.MapPath("/MySource/file/");
                    if (!Directory.Exists(rootFolder))
                    {
                        Directory.CreateDirectory(rootFolder);
                    }
                    string fileName = Path.GetFileName(HOPDONG.FileName);
                    string pathFile = Path.Combine(rootFolder, fileName);
                    HOPDONG.SaveAs(pathFile);
                    dtmodel.HOPDONG = fileName;                   
                }
                if (DateTime.TryParseExact(THOIGIANBATDAU, "MM/dd/yyyy", null, System.Globalization.DateTimeStyles.None, out DateTime parsedDate))
                {
                    dtmodel.THOIGIANBATDAU = parsedDate;
                }
                else
                {
                    ModelState.AddModelError("THOIGIANBATDAU", "Định dạng ngày không hợp lệ. Vui lòng chọn lại.");
                    return View(dtmodel);
                }
                dtmodel.PHUONGTHUCTHANHTOAN = "Chuyển khoản";
                dtmodel.NGAYTAO = DateTime.Now;
                dtmodel.TRANGTHAI = 1;
                db.DOITAC.Add(dtmodel);
                db.SaveChanges();
                return RedirectToAction("PartnerManage");
            }
        }

        [HttpGet]
        public ActionResult updatePartner(int? ID)
        {
            string role = GetUserRole();
            if (role == null || (role != "AFFILIATE PARTNER MANAGEMENT" && role != "ADMIN"))
            {
                return RedirectToAction("signin", "Login");
            }

            using (var db = new dbReviewDoCongNgheFTECHEntities())
            {
                DOITAC dt = db.DOITAC.Find(ID);
                ViewBag.dt = dt;
                return View();
            }
        }

        [HttpPost]
        public ActionResult updatePartner(DOITAC dtinput, HttpPostedFileBase HOPDONG)
        {
            string role = GetUserRole();
            if (role == null || (role != "AFFILIATE PARTNER MANAGEMENT" && role != "ADMIN"))
            {
                return RedirectToAction("signin", "Login");
            }

            using (var db = new dbReviewDoCongNgheFTECHEntities())
            {
                DOITAC dt = db.DOITAC.Find(dtinput.MADOITAC);
                if (HOPDONG != null && HOPDONG.ContentLength > 0)
                {
                    string rootFolder = Server.MapPath("/MySource/file/");
                    if (!Directory.Exists(rootFolder))
                    {
                        Directory.CreateDirectory(rootFolder);
                    }
                    string fileName = Path.GetFileName(HOPDONG.FileName);
                    string pathFile = Path.Combine(rootFolder, fileName);
                    HOPDONG.SaveAs(pathFile);
                    dt.HOPDONG = fileName;
                }
                dt.CANHANDAIDIEN = dtinput.CANHANDAIDIEN;
                dt.SDT = dtinput.SDT;
                dt.DIACHI = dtinput.DIACHI;
                dt.SOTAIKHOAN = dtinput.SOTAIKHOAN;
                dt.NGANHANG = dtinput.NGANHANG;
                dt.TENTAIKHOAN = dtinput.TENTAIKHOAN;
                db.SaveChanges();
                return RedirectToAction("PartnerManage");
            }
        }     
       [HttpPost]
        public ActionResult deletePartner(int? ID)
        {
            string role = GetUserRole();
            if (role == null || (role != "AFFILIATE PARTNER MANAGEMENT" && role != "ADMIN"))
            {
                return RedirectToAction("signin", "Login");
            }
            if (ID == null)
            {
                return RedirectToAction("PartnerManage");
            }

            using (var db = new dbReviewDoCongNgheFTECHEntities())
            {
                DOITAC dt = db.DOITAC.Find(ID);
                if (dt == null)
                {
                    return RedirectToAction("PartnerManage");
                }
                db.DOITAC.Remove(dt);
                db.SaveChanges();
                return RedirectToAction("PartnerManage");
            }
        }



        [HttpGet]
        public ActionResult detailPartner(int? ID)
        {
            string role = GetUserRole();
            if (role == null || (role != "AFFILIATE PARTNER MANAGEMENT" && role != "ADMIN"))
            {
                return RedirectToAction("signin", "Login");
            }

            using (var db = new dbReviewDoCongNgheFTECHEntities())
            {
                DOITAC dt = db.DOITAC.Find(ID);             
                ViewBag.dt = dt;                
                return View();
            }
        }
        [HttpPost]
        public ActionResult detailPartner()
        {
            string role = GetUserRole();
            if (role == null || (role != "AFFILIATE PARTNER MANAGEMENT" && role != "ADMIN"))
            {
                return RedirectToAction("signin", "Login");
            }

            using (var db = new dbReviewDoCongNgheFTECHEntities())
            {
                ViewBag.dsDoitac = db.DOITAC.ToList();           
                return View();
            }
        }


        //-----------------BAIVIET-----------------
        public ActionResult PostManage(int? MALOAI, int? MATRANGTHAI, int? MADANHMUC)
        {
            string role = GetUserRole();
            if (role == null)
            {
                return RedirectToAction("signin", "Login");
            }

            dbReviewDoCongNgheFTECHEntities db = new dbReviewDoCongNgheFTECHEntities();
            var baiViets = db.BAIVIET.AsQueryable();



            if (MALOAI.HasValue)
            {
                baiViets = baiViets.Where(b => b.MALOAI == MALOAI);
            }

            if (MATRANGTHAI.HasValue)
            {
                baiViets = baiViets.Where(b => b.MATRANGTHAI == MATRANGTHAI);
            }

            if (MADANHMUC.HasValue)
            {
                baiViets = baiViets.Where(b => b.MADANHMUC == MADANHMUC);
            }

            ViewBag.dsBAIVIET = baiViets.ToList();
            ViewBag.dsTRANGTHAI = db.TRANGTHAIBAIDANG.ToList();
            ViewBag.dsLOAIBAIDANG = db.LOAIBAIDANG.ToList();
            ViewBag.dsDANHMUC = db.DANHMUC.ToList();
            return View();
        }
        public ActionResult addPost()
        {
            string role = GetUserRole();
            if (role == null)
            {
                return RedirectToAction("signin", "Login");
            }
            if (role != "ADMIN" && role != "CONTENT MANAGEMENT")
            {
                return RedirectToAction("PostManage");
            }

            dbReviewDoCongNgheFTECHEntities db = new dbReviewDoCongNgheFTECHEntities();
            ViewBag.dsDANHMUC = db.DANHMUC.ToList();
            ViewBag.dsLOAIBAIDANG = db.LOAIBAIDANG.ToList();
            return View();
        }

        [HttpPost]
        public ActionResult addPost(BAIVIET model, HttpPostedFileBase myfile)
        {
            string role = GetUserRole();
            if (role == null)
            {
                return RedirectToAction("signin", "Login");
            }
            if (role != "ADMIN" && role != "CONTENT MANAGEMENT")
            {
                return RedirectToAction("PostManage");
            }

            dbReviewDoCongNgheFTECHEntities db = new dbReviewDoCongNgheFTECHEntities();

            if (myfile.ContentLength > 0)
            {
                String rootFolder = Server.MapPath("/MySource/img/product/");
                String pathImage = rootFolder + myfile.FileName;
                myfile.SaveAs(pathImage);

                model.HINHANH = myfile.FileName;
            }
            model.MATRANGTHAI = 1;
            model.NGAYDANG = DateTime.Today;
            db.BAIVIET.Add(model);
            db.SaveChanges();
            return RedirectToAction("PostManage");
        }



        
        public ActionResult reviewPost(int? MABAIVIET)
        {
            string role = GetUserRole();
            if (role == null)
            {
                return RedirectToAction("signin", "Login");
            }

            if (MABAIVIET == null)
            {
                return RedirectToAction("postManage");
            }
            dbReviewDoCongNgheFTECHEntities db = new dbReviewDoCongNgheFTECHEntities();
            BAIVIET bv = db.BAIVIET.FirstOrDefault(m => m.MABAIVIET == MABAIVIET);
            if (bv == null)
            {
                return RedirectToAction("postManage");
            }

            ViewBag.bv = bv;
            ViewBag.dsBAIVIET = db.BAIVIET.ToList();
            ViewBag.dsTRANGTHAI = db.TRANGTHAIBAIDANG.ToList();
            ViewBag.dsLOAIBAIDANG = db.LOAIBAIDANG.ToList();
            ViewBag.dsDANHMUC = db.DANHMUC.ToList();
            return View();
        }

        [HttpPost]
        public ActionResult reviewPost(BAIVIET model, HttpPostedFileBase fileAnh, bool? rvpost)
        {
            string role = GetUserRole();
            if (role == null)
            {
                return RedirectToAction("signin", "Login");
            }
            if (model.MABAIVIET == null)
            {
                return RedirectToAction("postManage");
            }
            dbReviewDoCongNgheFTECHEntities db = new dbReviewDoCongNgheFTECHEntities();

            BAIVIET bv = db.BAIVIET.Find(model.MABAIVIET);
            if (bv == null)
            {
                return RedirectToAction("postManage");
            }

            if (fileAnh != null)
            {
                String rootFolder = Server.MapPath("/MySource/img/product/");
                String pathImage = rootFolder + fileAnh.FileName;
                fileAnh.SaveAs(pathImage);
                bv.HINHANH = fileAnh.FileName;
            }

            bv.TENBAIVIET = model.TENBAIVIET;
            bv.TACGIA = model.TACGIA;
            bv.MADANHMUC = model.MADANHMUC;
            bv.MALOAI = model.MALOAI;
            bv.MATRANGTHAI = rvpost == true ? 2 : 5;
            bv.NOIDUNG = model.NOIDUNG;

            db.SaveChanges();
            return RedirectToAction("postManage");
        }
        [HttpPost]
        public ActionResult deletePost(int? MABAIVIET)
        {
            string role = GetUserRole();
            if (role == null || (role != "CONTENT MANAGEMENT" && role != "ADMIN"))
            {
                return RedirectToAction("signin", "Login");
            }
            if (MABAIVIET == null)
            {
                return RedirectToAction("postManage");
            }

            using (var db = new dbReviewDoCongNgheFTECHEntities())
            {
                BAIVIET bv = db.BAIVIET.Find(MABAIVIET);
                if (bv == null)
                {
                    return RedirectToAction("postManage");
                }
                db.BAIVIET.Remove(bv);
                db.SaveChanges();
                return RedirectToAction("postManage");
            }
        }

    }

}

