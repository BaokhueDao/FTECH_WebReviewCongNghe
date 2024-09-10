using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using FTECH_WebReviewCongNghe.Models;
using Microsoft.VisualStudio.Services.WebApi;

namespace FTECH_WebReviewCongNghe.Controllers
{
    public class HomeController : Controller
    {
        // GET: Home
        public ActionResult Index(int? MADANHMUC, int? index, String txtSearch)
        {
            dbReviewDoCongNgheFTECHEntities db = new dbReviewDoCongNgheFTECHEntities();
            int pageSize = 8;
            PagedList.IPagedList<BAIVIET> pagedList;
            // Nếu index truyền về null thì gán mặc định trang hiển thị là 1
            if (index == null)
            {
                index = 1;
            }
            // Kiểm tra có phân trang theo mã danh mục hay không
            if (MADANHMUC != null)
            {
                // phân trang theo số trang và danh mục
                pagedList = db.BAIVIET.Where(m => m.MADANHMUC == MADANHMUC).OrderBy(m => m.MADANHMUC).ToPagedList((int)index, pageSize);
            }
            // Kiểm tra có phần trang theo tìm kiếm hay không
            else if (txtSearch != null)
            {
                // phân trang theo tìm kiếm
                pagedList = db.BAIVIET.Where(m => m.TENBAIVIET.Contains(txtSearch)).OrderBy(m => m.MADANHMUC).ToPagedList((int)index, pageSize);
                ViewBag.txtSearch = txtSearch;
            }
            else
            {
                // phân trang theo số trang
                pagedList = db.BAIVIET.OrderBy(m => m.MABAIVIET).ToPagedList((int)index, pageSize);
            }
            // tạo 1 đối tượng PageRS để trả về client
            PageRS pageRS = new PageRS(pagedList.ToList(), (int)index, pagedList.PageSize, pagedList.PageCount);
            ViewBag.pageRS = pageRS;
            ViewBag.dsDANHMUC = db.DANHMUC.ToList();
            ViewBag.MDM = MADANHMUC;
            return View();
        }

        public ActionResult itemDetail(String MABAIVIET)
        {
            if (MABAIVIET != null)
            {
                dbReviewDoCongNgheFTECHEntities db = new dbReviewDoCongNgheFTECHEntities();
                BAIVIET sp = db.BAIVIET.Where(m => m.MABAIVIET + "" == MABAIVIET).ToList()[0];
                ViewBag.sp = sp;
                ViewBag.dsDANHMUC = db.DANHMUC;
                List<BAIVIET> sanPhamCungDanhMuc = db.BAIVIET.Where(m => m.MADANHMUC == sp.MADANHMUC && m.MABAIVIET != sp.MABAIVIET).ToList();
                ViewBag.sanPhamCungDanhMuc = sanPhamCungDanhMuc;
            }
            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}