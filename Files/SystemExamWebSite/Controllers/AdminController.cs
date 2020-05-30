using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using SystemExamWebSite.DataBase;
using SystemExamWebSite.Models;
using SystemExamWebSite.Models.AdminController;

namespace SystemExamWebSite.Controllers
{
    public class AdminController : Controller
    {
        //للتعامل مع قاعة البياناتcre
        ExamSystemEntities db = new ExamSystemEntities();
        // GET: Admin
        // عرض الفحة الرئيسية للمسئول اللي بيكون فيها إحصائيات لكل حاجة 
        public ActionResult Index()
        {
            // شرط بيستخدم إن لو في حد حاول يدخل الرابط وهو مش مسجل دخول فبيرجعه لصفحة تسجيل الدخول
            if (Session["Admin_Id"] == null || Session["Admin_Name"] == null)
            {
                return RedirectToAction("AdminLogin", "Home");
            }
            else
            {
                // للحصول على اسم المسئول الحالي
                string Name = Session["Admin_Name"].ToString();
                // للحصول على رقم المعرف للمسئول الحالي
                int Id = Convert.ToInt32(Session["Admin_Id"]);
                // إرسال المسئول الحالي إلى صفحة العرض
                ViewBag.Name = Name;
                // للحصول على عدد الصفوف لكل جدول لكي يتم عرض الاعداد
                var numberStud = db.tbl_Student.Where(a => a.IsDeleted == false).Count();
                var numberTechOk = db.tbl_Teacher.Where(a => a.IsDeleted == false && a.IsAccepted == true).Count();
                var numberTechNon = db.tbl_Teacher.Where(a => a.IsDeleted == false && a.IsAccepted == false).Count();
                var numberAdmin = db.tbl_Admin.Where(a => a.IsDeleted == false).Count();
                var numberAnnounc = db.tbl_Announcement.Where(a => a.IsDeleted == false).Count();
                var numberSubje = db.tbl_Subject.Count();
                var numberExam = db.tbl_Exam.Count();
                var numberlev = db.tbl_Level.Count();
                var numberDept = db.tbl_Department.Count();
                var numbermcq = db.tbl_QuestionMCQ.Count();
                var numbertf = db.tbl_QuestionTrueFalse.Count();
                var numberIsNonAccepted = db.tbl_Teacher.Where(a=>a.IsAccepted == false && a.IsDeleted == false).Count();
                // إرسال عدد الصفوف غلى صفحة العرض
                ViewBag.NumberStudent = numberStud.ToString();
                ViewBag.NumberTeacherOk = numberTechOk.ToString();
                ViewBag.NumberTeacherNon = numberTechNon.ToString();
                ViewBag.NumberAdmins = numberAdmin.ToString();
                ViewBag.NumberAnnouncement = numberAnnounc.ToString();
                ViewBag.NumbeSubjeects = numberSubje.ToString();
                ViewBag.NumberExams = numberExam.ToString();
                ViewBag.NumberLevels = numberlev.ToString();
                ViewBag.NumberDepartments = numberDept.ToString();
                ViewBag.NumberQMCQ = numbermcq.ToString();
                ViewBag.NumberQTAF = numbertf.ToString();
                ViewBag.NumberOfTeacherNonAccepted = numberIsNonAccepted.ToString();
                return View();
            }

        }
       // لتسحيل الخروج وإرسال المسئول إلى الصفحة الرئيسية للموقع
        public ActionResult Logout()
        {
            Session.Abandon();
            Session.RemoveAll();
            return RedirectToAction("Index","Home");
        }
        // صفحة إدارة المسئولين
        public ActionResult AdminMangement()
        {
            // لجلب عدد المدرسين الذين لم يتم قبولهم في النظام وإرسالها إلى صفحة العرض
            var numberIsNonAccepted = db.tbl_Teacher.Where(a => a.IsAccepted == false && a.IsDeleted == false).Count();
            ViewBag.NumberOfTeacherNonAccepted = numberIsNonAccepted.ToString();
            // شرط بيستخدم إن لو في حد حاول يدخل الرابط وهو مش مسجل دخول فبيرجعه لصفحة تسجيل الدخول
            if (Session["Admin_Id"] == null || Session["Admin_Name"] == null)
            {
                return RedirectToAction("Index", "Home");
            }
            else
            {
                // جلب اسم المسئول الحالي
                string Name = Session["Admin_Name"].ToString();
                // جلب رقم المعرف للمسئول الحالي
                int Id = Convert.ToInt32(Session["Admin_Id"]);
                //إرسال اسم المسئول لصفحة العرض
                ViewBag.Name = Name;
                //  عرض كل المسئولين عدا المسئول الحالي
                return View(db.tbl_Admin.Where(a=>a.IsDeleted == false && a.Admin_Id !=Id).ToList());
            }

        }
        // صفحة إضافة مسئول
        [HttpGet]
        public ActionResult AddAdmin()
        {
            // شرط بيستخدم إن لو في حد حاول يدخل الرابط وهو مش مسجل دخول فبيرجعه لصفحة تسجيل الدخول
            if (Session["Admin_Id"] == null || Session["Admin_Name"] == null)
            {
                return RedirectToAction("AdminLogin", "Home");
            }
            else
            {
                return View();
            }
           
        }
        // زرار إضافة مسئول        
        [HttpPost]
        public ActionResult AddAdmin(AdminMangementModel AddAdmin)
        {

            // شرط بيستخدم إن لو في حد حاول يدخل الرابط وهو مش مسجل دخول فبيرجعه لصفحة تسجيل الدخول  
            if (Session["Admin_Id"] == null || Session["Admin_Name"] == null)
            {
                return RedirectToAction("AdminLogin", "Home");
            }
            else
            {
                if (ModelState.IsValid)
                {
                    // التأكد أن البريد المدخل ليس موجود مسبقا
                    if (!db.tbl_Admin.Any(x => x.Admin_Email == AddAdmin.Admin_Email))
                    {
                        tbl_Admin adm = new tbl_Admin();
                        adm.Admin_Name = AddAdmin.Admin_Name;
                        adm.Admin_Email = AddAdmin.Admin_Email;
                        adm.Admin_Password = AddAdmin.Password;
                        adm.Admin_Phone = AddAdmin.Admin_Phone;
                        adm.Admin_CreatedOn = DateTime.Now;
                        adm.IsDeleted = false;
                        db.tbl_Admin.Add(adm);
                        db.SaveChanges();
                        return RedirectToAction("AdminMangement");
                    }
                    else
                    {
                        ViewBag.msg = "هذا الحساب موجود بالفعل";
                    }
                }
                return View(AddAdmin);
            }
        }
        // صفحة تعديل مسئول
        [HttpGet]
        public ActionResult EditAdmin(int? id)
        {
            // شرط بيستخدم إن لو في حد حاول يدخل الرابط وهو مش مسجل دخول فبيرجعه لصفحة تسجيل الدخول
            if (Session["Admin_Id"] == null || Session["Admin_Name"] == null)
            {
                return RedirectToAction("AdminLogin", "Home");
            }
            else
            {
                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                else
                {
                    // البحث عن المسئول
                    var CurrentAdmin = db.tbl_Admin.Find(id);
                    TempData["AdminId"] = id;
                    TempData.Keep();
                    return View(CurrentAdmin);
                }
            }
               
        }
        // زرار تعديل مسئول
        [HttpPost]
        public ActionResult EditAdmin([Bind(Exclude =("Admin_CreatedOn,IsDeleted"))]tbl_Admin EdAdm)
        {
            // شرط بيستخدم إن لو في حد حاول يدخل الرابط وهو مش مسجل دخول فبيرجعه لصفحة تسجيل الدخول
            if (Session["Admin_Id"] == null || Session["Admin_Name"] == null)
            {
                return RedirectToAction("AdminLogin", "Home");
            }
            else
            {
                if (ModelState.IsValid)
                {

                    int id = (int)TempData["AdminId"];
                    var adm = db.tbl_Admin.Where(a => a.Admin_Id == id).FirstOrDefault();
                    adm.Admin_Name = EdAdm.Admin_Name;
                    adm.Admin_Email = EdAdm.Admin_Email;
                    adm.Admin_Password = EdAdm.Admin_Password;
                    adm.Admin_Phone = EdAdm.Admin_Phone;
                    db.Entry(adm).State = EntityState.Modified;
                    db.SaveChanges();
                    return RedirectToAction("AdminMangement");
                }
                return View();
            }
        }
        // صفحة تفاصيل مسئول
        public ActionResult DetailsAdmin(int? id)
        {

            // شرط بيستخدم إن لو في حد حاول يدخل الرابط وهو مش مسجل دخول فبيرجعه لصفحة تسجيل الدخول      
            if (Session["Admin_Id"] == null || Session["Admin_Name"] == null)
            {
                return RedirectToAction("AdminLogin", "Home");
            }
            else
            {
                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                else
                {
                    var CurrentAdmin = db.tbl_Admin.Single(a => a.Admin_Id == id && a.IsDeleted==false);
                    return View(CurrentAdmin);
                }

            }
        }
        //صفحة حذف المسئول
        [HttpGet]
        public ActionResult DeleteAdmin(int? id)
        {

            // شرط بيستخدم إن لو في حد حاول يدخل الرابط وهو مش مسجل دخول فبيرجعه لصفحة تسجيل الدخول     
            if (Session["Admin_Id"] == null || Session["Admin_Name"] == null)
            {
                return RedirectToAction("AdminLogin", "Home");
            }
            else
            {
                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                else
                {
                    var CurrentAdmin = db.tbl_Admin.Find(id);
                    TempData["AdminId"] = id;
                    TempData.Keep();
                    return View(CurrentAdmin);
                }
            }
        }
        //زرار حذف مسئول
        [HttpPost]
        public ActionResult DeleteAdmin([Bind(Exclude = ("Admin_CreatedOn,Admin_Name,Admin_Email,Admin_Password,Admin_CreatedOn"))]tbl_Admin EdAdm)
        {

            // شرط بيستخدم إن لو في حد حاول يدخل الرابط وهو مش مسجل دخول فبيرجعه لصفحة تسجيل الدخول      
            if (Session["Admin_Id"] == null || Session["Admin_Name"] == null)
            {
                return RedirectToAction("AdminLogin", "Home");
            }
            else
            {
                    int id = (int)TempData["AdminId"];
                    var adm = db.tbl_Admin.Where(a => a.Admin_Id == id).FirstOrDefault();
                    adm.IsDeleted = true;
                    db.Entry(adm).State = EntityState.Modified;
                    db.SaveChanges();
                    return RedirectToAction("AdminMangement");
             }           
        }
         // صفحة إدارة هيئة التدريس
        public ActionResult TeacherMangement()
        {
            // لجلب عدد المدرسين الذين لم يتم قبولهم في النظام وإرسالها إلى صفحة العرض
            var numberIsNonAccepted = db.tbl_Teacher.Where(a => a.IsAccepted == false && a.IsDeleted == false).Count();
            ViewBag.NumberOfTeacherNonAccepted = numberIsNonAccepted.ToString();
            // شرط بيستخدم إن لو في حد حاول يدخل الرابط وهو مش مسجل دخول فبيرجعه لصفحة تسجيل الدخول
            if (Session["Admin_Id"] == null || Session["Admin_Name"] == null)
            {
                return RedirectToAction("Index", "Home");
            }
            else
            {
                string Name = Session["Admin_Name"].ToString();
                ViewBag.Name = Name;
                return View(db.tbl_Teacher.Where(a => a.IsDeleted == false && a.IsAccepted == true).ToList());
            }

        }
        // صفحة إضافة مدرس
        [HttpGet]
        public ActionResult AddTeacher()
        { 
            if (Session["Admin_Id"] == null || Session["Admin_Name"] == null)
            {
                return RedirectToAction("AdminLogin", "Home");
            }
            else
            {
                return View();
            }

        }
        // زرار إضافة مدرس
        [HttpPost]
        public ActionResult AddTeacher(RegisterTeacher AddTeach)
        {
            if (Session["Admin_Id"] == null || Session["Admin_Name"] == null)
            {
                return RedirectToAction("AdminLogin", "Home");
            }
            else
            {
                if (ModelState.IsValid)
                {
                    if (!db.tbl_Teacher.Any(x => x.Teacher_Email == AddTeach.Teacher_Email))
                    {
                        tbl_Teacher tech = new tbl_Teacher();
                        tech.Teacher_Name = AddTeach.Teacher_Name;
                        tech.Teacher_Email = AddTeach.Teacher_Email;
                        tech.Teacher_Password = AddTeach.Password;
                        tech.Teacher_Phone = AddTeach.Teacher_Phone;
                        tech.Teacher_CreatedOn = DateTime.Now;
                        tech.IsDeleted = false;
                        tech.IsAccepted = false;
                        db.tbl_Teacher.Add(tech);
                        db.SaveChanges();
                        return RedirectToAction("TeacherMangement");
                    }
                    else
                    {
                        ViewBag.msg = "هذا الحساب موجود بالفعل";
                    }
                }
                return View(AddTeach);
            }
        }
        // صفحة تعديل مدرس
        [HttpGet]
        public ActionResult EditTeacher(int? id)
        {
            // To Define Admin If donot Get it Will direct Page To Home 
            if (Session["Admin_Id"] == null || Session["Admin_Name"] == null)
            {
                return RedirectToAction("AdminLogin", "Home");
            }
            else
            {
                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                else
                {
                    var CurrentTeacher = db.tbl_Teacher.Find(id);
                    TempData["TeacherId"] = id;
                    TempData.Keep();
                    return View(CurrentTeacher);
                }
            }

        }
       // زرار تعديل مدرس
        [HttpPost]
        public ActionResult EditTeacher([Bind(Exclude = ("Admin_CreatedOn,IsDeleted,IsAccepted"))]tbl_Teacher EdTech)
        {
            if (Session["Admin_Id"] == null || Session["Admin_Name"] == null) 
            {
                return RedirectToAction("AdminLogin", "Home");
            }
            else
            {
                if (ModelState.IsValid)
                {

                    int id = (int)TempData["TeacherId"];
                    var tech = db.tbl_Teacher.Where(a => a.Teacher_Id == id).FirstOrDefault();
                    tech.Teacher_Name = EdTech.Teacher_Name;
                    tech.Teacher_Email = EdTech.Teacher_Email;
                    tech.Teacher_Password = EdTech.Teacher_Password;
                    tech.Teacher_Phone = EdTech.Teacher_Phone;
                    db.Entry(tech).State = EntityState.Modified;
                    db.SaveChanges();
                    return RedirectToAction("TeacherMangement");
                }
                return View();
            }
        }
       // صفحة تفاصيل مدرس
        public ActionResult DetailsTeacher(int? id)
        {
            if (Session["Admin_Id"] == null || Session["Admin_Name"] == null)
            {
                return RedirectToAction("AdminLogin", "Home");
            }
            else
            {
                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                else
                {
                    var CurrentTeacher = db.tbl_Teacher.Single(a => a.Teacher_Id == id && a.IsDeleted == false && a.IsAccepted == true);
                    return View(CurrentTeacher);
                }
            }
        }
        // صفحة حذف مدرس
        [HttpGet]
        public ActionResult DeleteTeacher(int? id)
        {
            // To Define Admin If donot Get it Will direct Page To Home 
            if (Session["Admin_Id"] == null || Session["Admin_Name"] == null)
            {
                return RedirectToAction("AdminLogin", "Home");
            }
            else
            {
                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                else
                {
                    var CurrentTeacher= db.tbl_Teacher.Find(id);
                    TempData["TeacherId"] = id;
                    TempData.Keep();
                    return View(CurrentTeacher);
                }
            }
        }
        // زرار حذف مدرس
        [HttpPost]
        public ActionResult DeleteTeacher([Bind(Exclude = ("Teacher_CreatedOn,Teacher_Name,Teacher_Email,Teacher_Password,IsAccepted"))]tbl_Teacher EdTeach)
        {
            if (Session["Admin_Id"] == null || Session["Admin_Name"] == null)
            {
                return RedirectToAction("AdminLogin", "Home");
            }
            else
            {
                int id = (int)TempData["TeacherId"];
                var teach = db.tbl_Teacher.Where(a => a.Teacher_Id == id).FirstOrDefault();
                teach.IsDeleted = true;
                db.Entry(teach).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("TeacherMangement");
            }
        }
        // صفحة رفض مدرس وإعادته إلى صفحة الطلبات
        [HttpGet]
        public ActionResult CancelTeacher(int? id)
        {
            if (Session["Admin_Id"] == null || Session["Admin_Name"] == null)
            {
                return RedirectToAction("AdminLogin", "Home");
            }
            else
            {
                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                else
                {
                    var CurrentTeacher = db.tbl_Teacher.Find(id);
                    TempData["TeacherId"] = id;
                    TempData.Keep();
                    return View(CurrentTeacher);
                }
            }
        }
        // زرار رفض مدرس
        [HttpPost]
        public ActionResult CancelTeacher([Bind(Exclude = ("Teacher_CreatedOn,Teacher_Name,Teacher_Email,Teacher_Password,IsDeleted"))]tbl_Teacher EdTeach)
        {
            if (Session["Admin_Id"] == null || Session["Admin_Name"] == null)
            {
                return RedirectToAction("AdminLogin", "Home");
            }
            else
            {
                int id = (int)TempData["TeacherId"];
                var teach = db.tbl_Teacher.Where(a => a.Teacher_Id == id).FirstOrDefault();
                teach.IsAccepted = false;
                db.Entry(teach).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("TeacherMangement");
            }
        }
        // صفحة عرض طلبات هيئة التدريس
        public ActionResult AcceptedTeacherMangement ()
        {
            var numberIsNonAccepted = db.tbl_Teacher.Where(a => a.IsAccepted == false && a.IsDeleted == false).Count();
            ViewBag.NumberOfTeacherNonAccepted = numberIsNonAccepted.ToString();
            if (Session["Admin_Id"] == null || Session["Admin_Name"] == null)
            {
                return RedirectToAction("Index", "Home");
            }
            else
            {
                string Name = Session["Admin_Name"].ToString();
                ViewBag.Name = Name;
                return View(db.tbl_Teacher.Where(a => a.IsDeleted == false && a.IsAccepted == false).ToList());
            }
        }
       // صفحة قبول مدرس
        [HttpGet]
        public ActionResult AcceptTeacher(int? id)
        {
            if (Session["Admin_Id"] == null || Session["Admin_Name"] == null)
            {
                return RedirectToAction("AdminLogin", "Home");
            }
            else
            {
                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                else
                {
                    var CurrentTeacher = db.tbl_Teacher.Find(id);
                    TempData["TeacherId"] = id;
                    TempData.Keep();
                    return View(CurrentTeacher);
                }
            }
        }
        // زرار قبول مدرس
        [HttpPost]
        public ActionResult AcceptTeacher([Bind(Exclude = ("Teacher_CreatedOn,Teacher_Name,Teacher_Email,Teacher_Password,IsDeleted"))]tbl_Teacher EdTeach)
        {
            // To Define Admin If donot Get it Will direct Page To Home 
            if (Session["Admin_Id"] == null || Session["Admin_Name"] == null)
            {
                return RedirectToAction("AdminLogin", "Home");
            }
            else
            {
                int id = (int)TempData["TeacherId"];
                var teach = db.tbl_Teacher.Where(a => a.Teacher_Id == id).FirstOrDefault();
                teach.IsAccepted = true;
                db.Entry(teach).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("AcceptedTeacherMangement");
            }
        }
        // صفحة تفاصيل المسئول الحالي من الاعدادات
        public ActionResult DetailsMe()
        {
            if (Session["Admin_Id"] == null || Session["Admin_Name"] == null)
            {
                return RedirectToAction("AdminLogin", "Home");
            }
            else
            {
                int Id = Convert.ToInt32(Session["Admin_Id"]);
                var CurrentAdmin = db.tbl_Admin.Single(a => a.Admin_Id == Id);
                return View(CurrentAdmin);

            }
        }
        //صفحة تعديل المسئول الحالي من الاعدادات
        [HttpGet]
        public ActionResult EditMe()
        {
            if (Session["Admin_Id"] == null || Session["Admin_Name"] == null)
            {
                return RedirectToAction("AdminLogin", "Home");
            }
            else
            {
                int Id = Convert.ToInt32(Session["Admin_Id"]);
                var CurrentAdmin = db.tbl_Admin.Find(Id);
                TempData["AdminId"] = Id;
                TempData.Keep();
                return View(CurrentAdmin);
            }

        }
        // زرار تعديل المسئول الحالي
        [HttpPost]
        public ActionResult EditMe([Bind(Exclude = ("Admin_CreatedOn,IsDeleted"))]tbl_Admin EdAdm)
        {
            if (Session["Admin_Id"] == null || Session["Admin_Name"] == null)
            {
                return RedirectToAction("AdminLogin", "Home");
            }
            else
            {
                if (ModelState.IsValid)
                {

                    int id = (int)TempData["AdminId"];
                    var adm = db.tbl_Admin.Where(a => a.Admin_Id == id).FirstOrDefault();
                    adm.Admin_Name = EdAdm.Admin_Name;
                    adm.Admin_Email = EdAdm.Admin_Email;
                    adm.Admin_Password = EdAdm.Admin_Password;
                    adm.Admin_Phone = EdAdm.Admin_Phone;
                    db.Entry(adm).State = EntityState.Modified;
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
                return View();
            }
        }
        // صفحة حذف المسئول الحالي
        [HttpGet]
        public ActionResult DeleteMe()
        {
            if (Session["Admin_Id"] == null || Session["Admin_Name"] == null)
            {
                return RedirectToAction("AdminLogin", "Home");
            }
            else
            {
                int Id = Convert.ToInt32(Session["Admin_Id"]);
                var CurrentAdmin = db.tbl_Admin.Find(Id);
                TempData["AdminId"] = Id;
                TempData.Keep();
                return View(CurrentAdmin);
            }
        }
        // زرار حذف المسئول الحالي ويتم توجيه بعدها إلى الصفحة الرئيسية
        [HttpPost]
        public ActionResult DeleteMe([Bind(Exclude = ("Admin_CreatedOn,Admin_Name,Admin_Email,Admin_Password,Admin_CreatedOn"))]tbl_Admin EdAdm)
        {
            if (Session["Admin_Id"] == null || Session["Admin_Name"] == null)
            {
                return RedirectToAction("AdminLogin", "Home");
            }
            else
            {
                int id = (int)TempData["AdminId"];
                var adm = db.tbl_Admin.Where(a => a.Admin_Id == id).FirstOrDefault();
                adm.IsDeleted = true;
                db.Entry(adm).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index","Home");
            }
        }
        // صفحة إدارة الطلاب
        public ActionResult StudentMangement()
        {
            var numberIsNonAccepted = db.tbl_Teacher.Where(a => a.IsAccepted == false && a.IsDeleted == false).Count();
            ViewBag.NumberOfTeacherNonAccepted = numberIsNonAccepted.ToString();
            if (Session["Admin_Id"] == null || Session["Admin_Name"] == null)
            {
                return RedirectToAction("Index", "Home");
            }
            else
            {
                string Name = Session["Admin_Name"].ToString();
                ViewBag.Name = Name;
                return View(db.tbl_Student.Where(a => a.IsDeleted == false).ToList());
            }
        }
        // صفحة تفاصيل الطالب
        public ActionResult DetailsStudent(int? id)
        {
            if (Session["Admin_Id"] == null || Session["Admin_Name"] == null)
            {
                return RedirectToAction("AdminLogin", "Home");
            }
            else
            {
                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                else
                {
                    var CurrentSudent = db.tbl_Student.Single(a => a.Student_Id == id && a.IsDeleted == false);
                    return View(CurrentSudent);
                }

            }
        }
        // صفحة حذف طالب
        [HttpGet]
        public ActionResult DeleteStudent(int? id)
        {
            if (Session["Admin_Id"] == null || Session["Admin_Name"] == null)
            {
                return RedirectToAction("AdminLogin", "Home");
            }
            else
            {
                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                else
                {
                    var CurrentStudent = db.tbl_Student.Find(id);
                    TempData["StudentId"] = id;
                    TempData.Keep();
                    return View(CurrentStudent);
                }
            }
        }
        // زرار حذف طالب
        [HttpPost]
        public ActionResult DeleteStudent([Bind(Exclude = ("Student_CreatedOn,Student_Name,Student_Email,Student_Password,IsDeleted,Level_Id,Department_Id"))]tbl_Student EdStu)
        {
            if (Session["Admin_Id"] == null || Session["Admin_Name"] == null)
            {
                return RedirectToAction("AdminLogin", "Home");
            }
            else
            {
                int id = (int)TempData["StudentId"];
                var stu = db.tbl_Student.Where(a => a.Student_Id == id).FirstOrDefault();
                stu.IsDeleted = true;
                db.Entry(stu).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("StudentMangement");
            }
        }
        // صفحة إدارة المستويات
        public ActionResult LevlesManagement()
        {
            var numberIsNonAccepted = db.tbl_Teacher.Where(a => a.IsAccepted == false && a.IsDeleted == false).Count();
            ViewBag.NumberOfTeacherNonAccepted = numberIsNonAccepted.ToString();
            if (Session["Admin_Id"] == null || Session["Admin_Name"] == null)
            {
                return RedirectToAction("Index", "Home");
            }
            else
            {
                string Name = Session["Admin_Name"].ToString();
                ViewBag.Name = Name;
                return View(db.tbl_Level.ToList());
            }
        }
        // صفحة إضافة مستوى
        [HttpGet]
        public ActionResult AddLevel()
        { 
            if (Session["Admin_Id"] == null || Session["Admin_Name"] == null)
            {
                return RedirectToAction("AdminLogin", "Home");
            }
            else
            {
                return View();
            }

        }
        // زرار إضافة مستوى 
        [HttpPost]
        public ActionResult AddLevel(tbl_Level AddLevl)
        {
            if (Session["Admin_Id"] == null || Session["Admin_Name"] == null)
            {
                return RedirectToAction("AdminLogin", "Home");
            }
            else
            {
                if (ModelState.IsValid)
                {
                    if (!db.tbl_Level.Any(x => x.Level_Code == AddLevl.Level_Code))
                    {
                        tbl_Level lev = new tbl_Level();
                        lev.Level_Name = AddLevl.Level_Name;
                        lev.Level_Code = AddLevl.Level_Code;
                     
                        db.tbl_Level.Add(lev);
                        db.SaveChanges();
                        return RedirectToAction("LevlesManagement");
                    }
                    else
                    {
                        ViewBag.msg = "هذا الكود موجود بالفعل";
                    }
                }
                return View(AddLevl);
            }
        }
        // صفحة تعديل مستوى
        [HttpGet]
        public ActionResult EditLevel(int? id)
        {
            if (Session["Admin_Id"] == null || Session["Admin_Name"] == null)
            {
                return RedirectToAction("AdminLogin", "Home");
            }
            else
            {
                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                else
                {
                    var CurrentLev = db.tbl_Level.Find(id);
                    TempData["LevelId"] = id;
                    TempData.Keep();
                    return View(CurrentLev);
                }
            }

        }
        // زرار تعديل مستوى
        [HttpPost]
        public ActionResult EditLevel(tbl_Level EdLev)
        {
            if (Session["Admin_Id"] == null || Session["Admin_Name"] == null)
            {
                return RedirectToAction("AdminLogin", "Home");
            }
            else
            {
                if (ModelState.IsValid)
                {
                        int id = (int)TempData["LevelId"];
                        var lev = db.tbl_Level.Where(a => a.Level_Id == id).FirstOrDefault();
                        lev.Level_Name = EdLev.Level_Name;
                        lev.Level_Code = EdLev.Level_Code;
                        db.Entry(lev).State = EntityState.Modified;
                        db.SaveChanges();
                        return RedirectToAction("LevlesManagement");
                }
                return View();
            }
        }
        // صفحة حذف مستوى
        [HttpGet]
        public ActionResult DeleteLevel(int? id)
        {
            if (Session["Admin_Id"] == null || Session["Admin_Name"] == null)
            {
                return RedirectToAction("AdminLogin", "Home");
            }
            else
            {
                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                else
                {
                    var CurrentLevel = db.tbl_Level.Find(id);
                    TempData["LevelId"] = id;
                    TempData.Keep();
                    return View(CurrentLevel);
                }
            }
        }
        // زرار حذف مستوى
        [HttpPost]
        public ActionResult DeleteLevel(tbl_Level DelLevel)
        {
            if (Session["Admin_Id"] == null || Session["Admin_Name"] == null)
            {
                return RedirectToAction("AdminLogin", "Home");
            }
            else
            {
                try
                {
                    int id = (int)TempData["LevelId"];
                    tbl_Level lev = db.tbl_Level.Find(id);
                    db.tbl_Level.Remove(lev);
                    db.SaveChanges();
                    return RedirectToAction("LevlesManagement");
                }
                catch 
                {
                    ViewBag.msg = "يوجد طلبة في هذا المستوى";
                }
            }
            return View();
        }
        // صفحة إدارة الأقسام
        public ActionResult DepartmentsManagement()
        {
            var numberIsNonAccepted = db.tbl_Teacher.Where(a => a.IsAccepted == false && a.IsDeleted == false).Count();
            ViewBag.NumberOfTeacherNonAccepted = numberIsNonAccepted.ToString();
            if (Session["Admin_Id"] == null || Session["Admin_Name"] == null)
            {
                return RedirectToAction("Index", "Home");
            }
            else
            {
                string Name = Session["Admin_Name"].ToString();
                ViewBag.Name = Name;
                return View(db.tbl_Department.ToList());
            }
        }
        // إضافة قسم
        [HttpGet]
        public ActionResult AddDepartment()
        { 
            if (Session["Admin_Id"] == null || Session["Admin_Name"] == null)
            {
                return RedirectToAction("AdminLogin", "Home");
            }
            else
            {
                return View();
            }

        }
        // زرار إضافة قسم
        [HttpPost]
        public ActionResult AddDepartment(tbl_Department AddDept)
        {
            if (Session["Admin_Id"] == null || Session["Admin_Name"] == null)
            {
                return RedirectToAction("AdminLogin", "Home");
            }
            else
            {
                if (ModelState.IsValid)
                {
                    if (!db.tbl_Department.Any(x => x.Department_Code == AddDept.Department_Code))
                    {
                        tbl_Department dep = new tbl_Department();
                        dep.Department_Name = AddDept.Department_Name;
                        dep.Department_Code = AddDept.Department_Code;
                        db.tbl_Department.Add(dep);
                        db.SaveChanges();
                        return RedirectToAction("DepartmentsManagement");
                    }
                    else
                    {
                        ViewBag.msg = "هذا الكود موجود بالفعل";
                    }
                }
                return View(AddDept);
            }
        }
        // صفحة تعديل قسم
        [HttpGet]
        public ActionResult EditDepartment(int? id)
        {
            if (Session["Admin_Id"] == null || Session["Admin_Name"] == null)
            {
                return RedirectToAction("AdminLogin", "Home");
            }
            else
            {
                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                else
                {
                    var CurrentDep = db.tbl_Department.Find(id);
                    TempData["DepartmentId"] = id;
                    TempData.Keep();
                    return View(CurrentDep);
                }
            }

        }
        // زرار تعديل قسم
        [HttpPost]
        public ActionResult EditDepartment(tbl_Department EdDep)
        {
            if (Session["Admin_Id"] == null || Session["Admin_Name"] == null)
            {
                return RedirectToAction("AdminLogin", "Home");
            }
            else
            {
                if (ModelState.IsValid)
                {
                    int id = (int)TempData["DepartmentId"];
                    var Dep = db.tbl_Department.Where(a => a.Department_Id == id).FirstOrDefault();
                    Dep.Department_Name = EdDep.Department_Name;
                    Dep.Department_Code = EdDep.Department_Code;
                    db.Entry(Dep).State = EntityState.Modified;
                    db.SaveChanges();
                    return RedirectToAction("DepartmentsManagement");
                }
                return View();
            }
        }
        // صفحة حذف قسم
        [HttpGet]
        public ActionResult DeleteDepartment(int? id)
        {
            if (Session["Admin_Id"] == null || Session["Admin_Name"] == null)
            {
                return RedirectToAction("AdminLogin", "Home");
            }
            else
            {
                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                else
                {
                    var CurrentDep = db.tbl_Department.Find(id);
                    TempData["DepartmentId"] = id;
                    TempData.Keep();
                    return View(CurrentDep);
                }
            }
        }
        // زرار حذف قسم
        [HttpPost]
        public ActionResult DeleteDepartment(tbl_Level DelLevel)
        {
            if (Session["Admin_Id"] == null || Session["Admin_Name"] == null)
            {
                return RedirectToAction("AdminLogin", "Home");
            }
            else
            {
                try
                {
                    int id = (int)TempData["DepartmentId"];
                    tbl_Department Dep = db.tbl_Department.Find(id);
                    db.tbl_Department.Remove(Dep);
                    db.SaveChanges();
                    return RedirectToAction("DepartmentsManagement");
                }
                catch 
                {
                    ViewBag.msg = "يوجد طلبة في هذا القسم";
                }

            }
            return View();
        }
    }
}  