using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SystemExamWebSite.DataBase;
using SystemExamWebSite.Models;

namespace SystemExamWebSite.Controllers
{
    public class HomeController : Controller
    {

        // DataBase
        ExamSystemEntities db = new ExamSystemEntities();
        // GET: Home
        //عرض الصفحة الرئيسية للموقع
        public ActionResult Index()
        {
            return View();
        } 
        // صفحة تسجيل دخول الطالب
        [HttpGet]
        public ActionResult StudentLogin()
        {
            return View();
        }
        // زرار تسجيل دخول الطالب
        [HttpPost]
        public ActionResult StudentLogin(LoginStudent StuLog)
        {
            if (ModelState.IsValid)
            {
                if (db.tbl_Student.Where(m => m.Student_Email == StuLog.Student_Email && m.Student_Password == StuLog.Password && m.IsDeleted == false).FirstOrDefault() == null)
                {
                    ViewBag.msg = "كلمة السر أو البريد الإلكتروني غير صحيح";
                }
                else
                {
                    // إرسال بيانات المسئول إلي الكنترولر الخاص به
                    var name = db.tbl_Student.Single(x => x.Student_Email == StuLog.Student_Email);
                    Session["Studnet_Id"] = name.Student_Id;
                    Session["Studnet_Name"] = name.Student_Name;
                    Session["Department_Id"] = name.Department_Id;
                    Session["Level_Id"] = name.Level_Id;
                    return RedirectToAction("Index", "Student");
                }
            }
            return View();
        }
        // صفحة ستجيل طالب  جديد
        [HttpGet]
        public ActionResult StudentRegister()
        {
            
            //لملأ عناصر  الدروب داونلست بالبيانات            
            List<tbl_Level> levlist = db.tbl_Level.ToList();
            List<tbl_Department> Deplist = db.tbl_Department.ToList();

            ViewData["Levels"] = new SelectList(levlist, "Level_Id", "Level_Name");
            ViewData["Departments"] = new SelectList(Deplist, "Department_Id", "Department_Name");
            return View();
        }
        // زرار إضافة طالب جديد
        [HttpPost]
        public ActionResult StudentRegister(RegisterStudent regStu)
        {
            if (ModelState.IsValid)
            {
                if(!db.tbl_Student.Any(x=>x.Student_Email == regStu.Student_Email))
                {
                    tbl_Student stu = new tbl_Student();
                    stu.Student_Name = regStu.Student_Name;
                    stu.Student_Email = regStu.Student_Email;
                    stu.Student_Phone = regStu.Student_Phone;
                    stu.Student_Password = regStu.Password;
                    stu.Student_CreatedOn = DateTime.Now;
                    stu.Level_Id = regStu.Level_Id;
                    stu.Department_Id = regStu.Department_ID;
                    stu.IsDeleted = false;
                    db.tbl_Student.Add(stu);
                    db.SaveChanges();
                    return RedirectToAction("StudentLogin");
                }
                else
                {
                    ViewBag.mssg = "هذا الحساب موجود بالفعل";
                    List<tbl_Level> levlist = db.tbl_Level.ToList();
                    List<tbl_Department> Deplist = db.tbl_Department.ToList();
                    ViewData["Levels"] = new SelectList(levlist, "Level_Id", "Level_Name");
                    ViewData["Departments"] = new SelectList(Deplist, "Department_Id", "Department_Name");

                }
            }
            return View(regStu);
        }
        // صفحة تسجيل دخول مدرس
        [HttpGet]
        public ActionResult TeacherLogin()
        {
            return View();
        }
        // زرار تسجيل دخول مدرس
        [HttpPost]
        public ActionResult TeacherLogin(LoginTecher TechLog)
        {
            if (ModelState.IsValid)
            {
                if (db.tbl_Teacher.Where(m => m.Teacher_Email == TechLog.Teacher_Email && m.Teacher_Password == TechLog.Password && m.IsDeleted == false&&m.IsAccepted == true).FirstOrDefault() == null)
                {
                    ViewBag.msg = "كلمة السر أو البريد الإلكتروني غير صحيح";
                }
                else
                {
                    // إرسال بيانات المسئول إلي الكنترولر الخاص به
                    var name = db.tbl_Teacher.Single(x => x.Teacher_Email == TechLog.Teacher_Email);
                    Session["Teacher_Id"] = name.Teacher_Id;
                    Session["Teacher_Name"] = name.Teacher_Name;
                    return RedirectToAction("Index", "Teacher");
                }
            }
            return View();
        }
        // صفحة تسجيل مدرس جديد
        [HttpGet]
        public ActionResult RegisterTeacher()
        {
            return View();
        }
        // زرار تسجيل مدرس جديد
        [HttpPost]
        public ActionResult RegisterTeacher(RegisterTeacher RgTech)
        {
            if (ModelState.IsValid)
            {
                if (!db.tbl_Teacher.Any(x => x.Teacher_Email == RgTech.Teacher_Email))
                {
                    tbl_Teacher teach = new tbl_Teacher();
                    teach.Teacher_Name = RgTech.Teacher_Name;
                    teach.Teacher_Email = RgTech.Teacher_Email;
                    teach.Teacher_Password = RgTech.Password;
                    teach.Teacher_Phone = RgTech.Teacher_Phone;
                    teach.Teacher_CreatedOn = DateTime.Now;
                    teach.IsAccepted = false;
                    teach.IsDeleted = false;
                    db.tbl_Teacher.Add(teach);
                    db.SaveChanges();
                    return RedirectToAction("TeacherLogin");
                }
                else
                {
                    ViewBag.msg = "هذا الحساب موجود بالفعل";
                }



            }
            return View(RgTech);
        }

        // صفحة تسجيل دخول مسئول
        [HttpGet]
        public ActionResult AdminLogin()
        {
            return View();
        }
        // زرار تسجيل دخول مسئول
        [HttpPost]
        public ActionResult AdminLogin(LoginAdmin AdLog)
        {
            if (ModelState.IsValid)
            {
                if (db.tbl_Admin.Where(m => m.Admin_Email == AdLog.Admin_Email && m.Admin_Password == AdLog.Password && m.IsDeleted == false).FirstOrDefault() == null)
                {
                    ViewBag.msg = "كلمة السر أو البريد الإلكتروني غير صحيح";
                }
                else
                {
                    // إرسال بيانات المسئول إلي الكنترولر الخاص به
                    var name = db.tbl_Admin.Single(x => x.Admin_Email == AdLog.Admin_Email);
                    Session["Admin_Id"] = name.Admin_Id;
                    Session["Admin_Name"] = name.Admin_Name;
                    return RedirectToAction("Index", "Admin");
                }
            }
            return View();
        }
    }
}