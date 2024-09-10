using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using FTECH_WebReviewCongNghe.Models;

namespace FTECH_WebReviewCongNghe.Controllers
{
    public class LoginController : Controller
    {
        public ActionResult signin()
        {
            return View();
        }

        [HttpPost]
        public ActionResult signin(TAIKHOAN tk)
        {
            dbReviewDoCongNgheFTECHEntities db = new dbReviewDoCongNgheFTECHEntities();
            List<TAIKHOAN> tkFound = db.TAIKHOAN.Where(m => m.EMAIL == tk.EMAIL && m.MATKHAU == tk.MATKHAU && m.TRANGTHAI == 1).ToList();
            if (tkFound.Count > 0)
            {
                Session["account"] = tkFound[0];
                Session["role"] = tkFound[0].CHUCVU.TENCHUCVU; // Store the user role in session
                return RedirectToAction("Index", "Home");
            }
            else
            {
                ViewBag.err = "1";
                return View();
            }
        }

        public ActionResult signup()
        {
            return View();
        }

        [HttpPost]
        public ActionResult signup(TAIKHOAN tk, String repeatPassword)
        {
            String err = null;
            dbReviewDoCongNgheFTECHEntities db = new dbReviewDoCongNgheFTECHEntities();
            TAIKHOAN tkFind = db.TAIKHOAN.Where(m => m.EMAIL == tk.EMAIL).SingleOrDefault();
            if (tkFind != null)//kiểm tra tồn tại chưa
            {
                err = "1";
            }
            else if (!tk.MATKHAU.Equals(repeatPassword))//kiểm tra mk và nhập lại mk có khớp ko
            {
                err = "2";
            }
            else // đk thành công
            {
                err = "0";
                tk.IDCHUCVU = db.CHUCVU.Where(m => m.TENCHUCVU.Equals("USER")).SingleOrDefault().ID;
                tk.TRANGTHAI = 1;
                db.TAIKHOAN.Add(tk);
                db.SaveChanges();
            }
            ViewBag.err = err;
            return View();

        }
        public ActionResult logout()
        {
            Session["account"] = null;
            Session["cart"] = null;
            return RedirectToAction("index", "Home");
        }
    }
}