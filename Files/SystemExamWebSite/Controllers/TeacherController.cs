using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using SystemExamWebSite.DataBase;
using SystemExamWebSite.Models;

namespace SystemExamWebSite.Controllers
{
    public class TeacherController : Controller
    {
        // GET: Teacher
        //للتعامل مع قاعة البيانات
        ExamSystemEntities db = new ExamSystemEntities();
        // عرض الفحة الرئيسية للمدرس
        public ActionResult Index()
        {
            // شرط بيستخدم إن لو في حد حاول يدخل الرابط وهو مش مسجل دخول فبيرجعه لصفحة تسجيل الدخول
            if (Session["Teacher_Id"] == null || Session["Teacher_Name"] == null)
            {
                return RedirectToAction("TeacherLogin", "Home");
            }
            else
            {
                // للحصول على اسم المدرس الحالي
                string Name = Session["Teacher_Name"].ToString();
                // للحصول على رقم المعرف المدرس الحالي
                int Id = Convert.ToInt32(Session["Teacher_Id"]);
                // إرسال المدرس الحالي إلى صفحة العرض
                ViewBag.Name = Name;
                // عرض الإعلانات
                var Anonoun = db.tbl_Announcement.Where(a => a.IsDeleted == false && a.Teacher_Id == Id).OrderByDescending(a => a.Announcement_Id).ToList();
                return View(Anonoun);
            }
        }
        // لتسحيل الخروج وإرسال المسئول إلى الصفحة الرئيسية للموقع
        public ActionResult Logout()
        {
            Session.Abandon();
            Session.RemoveAll();
            return RedirectToAction("Index", "Home");
        }
        // صفحة تفاصيل المدرس الحالي من الاعدادات
        public ActionResult DetailsMe()
        {
            if (Session["Teacher_Id"] == null || Session["Teacher_Name"] == null)
            {
                return RedirectToAction("TeacherLogin", "Home");
            }
            else
            {
                int Id = Convert.ToInt32(Session["Teacher_Id"]);
                var CurrentTeacher = db.tbl_Teacher.Single(a => a.Teacher_Id == Id);
                return View(CurrentTeacher);

            }
        }
        //صفحة تعديل المدرس الحالي من الاعدادات
        [HttpGet]
        public ActionResult EditMe()
        {
            if (Session["Teacher_Id"] == null || Session["Teacher_Name"] == null)
            {
                return RedirectToAction("TeacherLogin", "Home");
            }
            else
            {
                int Id = Convert.ToInt32(Session["Teacher_Id"]);
                var CurrentTeacher = db.tbl_Teacher.Find(Id);
                TempData["TeacherId"] = Id;
                TempData.Keep();
                return View(CurrentTeacher);
            }

        }
        // زرار تعديل المدرس الحالي
        [HttpPost]
        public ActionResult EditMe([Bind(Exclude = ("Teacher_CreatedOn,IsAccepted,IsDeleted"))]tbl_Teacher EdTeach)
        {
            if (Session["Teacher_Id"] == null || Session["Teacher_Name"] == null)
            {
                return RedirectToAction("TeacherLogin", "Home");
            }
            else
            {
                if (ModelState.IsValid)
                {

                    int id = (int)TempData["TeacherId"];
                    var adTeach = db.tbl_Teacher.Where(a => a.Teacher_Id == id).FirstOrDefault();
                    adTeach.Teacher_Name = EdTeach.Teacher_Name;
                    adTeach.Teacher_Email = EdTeach.Teacher_Email;
                    adTeach.Teacher_Password = EdTeach.Teacher_Password;
                    adTeach.Teacher_Phone = EdTeach.Teacher_Phone;
                    db.Entry(adTeach).State = EntityState.Modified;
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
                return View();
            }
        }
        // صفحة حذف المدرس الحالي
        [HttpGet]
        public ActionResult DeleteMe()
        {
            if (Session["Teacher_Id"] == null || Session["Teacher_Name"] == null)
            {
                return RedirectToAction("TeacherLogin", "Home");
            }
            else
            {
                int Id = Convert.ToInt32(Session["Teacher_Id"]);
                var CurrentTeacher = db.tbl_Teacher.Find(Id);
                TempData["TeacherId"] = Id;
                TempData.Keep();
                return View(CurrentTeacher);
            }
        }
        // زرار حذف المدرس الحالي ويتم توجيه بعدها إلى الصفحة الرئيسية
        [HttpPost]
        public ActionResult DeleteMe([Bind(Exclude = ("Teacher_CreatedOn,Teacher_Name,Teacher_Email,Teacher_Password,Teacher_CreatedOn,IsAccepted"))]tbl_Teacher EdTeah)
        {
            if (Session["Teacher_Id"] == null || Session["Teacher_Name"] == null)
            {
                return RedirectToAction("TeacherLogin", "Home");
            }
            else
            {
                int id = (int)TempData["TeacherId"];
                var teach = db.tbl_Teacher.Where(a => a.Teacher_Id == id).FirstOrDefault();
                teach.IsDeleted = true;
                db.Entry(teach).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index", "Home");
            }
        }
        // صفحة نتائج الطلاب
        public ActionResult ExamResult()
        {
            if (Session["Teacher_Id"] == null || Session["Teacher_Name"] == null)
            {
                return RedirectToAction("TeacherLogin", "Home");
            }
            else
            {
                string Name = Session["Teacher_Name"].ToString();
                ViewBag.Name = Name;
                return View(db.tbl_Student.Where(a => a.IsDeleted == false ).ToList());
            }
        }
        // صفحة تفاصيل  نتيجة الطالب
        public ActionResult DetailsStudentResult(int? id)
        {
            if (Session["Teacher_Id"] == null || Session["Teacher_Name"] == null)
            {
                return RedirectToAction("TeacherLogin", "Home");
            }
            else
            {
                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                else
                {
                    var s = db.tbl_Student.Single(a => a.Student_Id == id && a.IsDeleted == false);
                    ViewBag.StudentName = s.Student_Name;
                    int Idt = Convert.ToInt32(Session["Teacher_Id"]);
                    var CurrentSudent = db.tbl_Result.Where(a => a.Student_Id == id && a.tbl_Student.IsDeleted == false && a.tbl_Exam.tbl_Teacher.Teacher_Id ==Idt).ToList();
                    return View(CurrentSudent);
                }

            }
        }
        // صفحة الطلاب
        public ActionResult Student()
        {
            if (Session["Teacher_Id"] == null || Session["Teacher_Name"] == null)
            {
                return RedirectToAction("TeacherLogin", "Home");
            }
            else
            {
                string Name = Session["Teacher_Name"].ToString();
                ViewBag.Name = Name;
                return View(db.tbl_Student.Where(a => a.IsDeleted == false).ToList());
            }
        }
        // صفحة تفاصيل الطالب
        public ActionResult DetailsStudent(int? id)
        {
            if (Session["Teacher_Id"] == null || Session["Teacher_Name"] == null)
            {
                return RedirectToAction("TeacherLogin", "Home");
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
        // صفحة إدارة الإعلانات
        public ActionResult AnnouncementManagement()
        {
            if (Session["Teacher_Id"] == null || Session["Teacher_Name"] == null)
            {
                return RedirectToAction("TeacherLogin", "Home");
            }
            else
            {
                string Name = Session["Teacher_Name"].ToString();
                int Id = Convert.ToInt32(Session["Teacher_Id"]);
                ViewBag.Name = Name;
                return View(db.tbl_Announcement.Where(a => a.IsDeleted == false && a.Teacher_Id == Id).ToList());
            }
        }
        // صفحة تفاصيل الإعلان
        public ActionResult DetailsAnnouncement(int? id)
        {
            if (Session["Teacher_Id"] == null || Session["Teacher_Name"] == null)
            {
                return RedirectToAction("TeacherLogin", "Home");
            }
            else
            {
                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                else
                {
                    var CurrentAnnouncement = db.tbl_Announcement.Single(a => a.Announcement_Id == id && a.IsDeleted == false);
                    return View(CurrentAnnouncement);
                }

            }
        }
        // صفحة إضافة إعلان
        [HttpGet]
        public ActionResult AddAnnouncement()
        {
            if (Session["Teacher_Id"] == null || Session["Teacher_Name"] == null)
            {
                return RedirectToAction("TeacherLogin", "Home");
            }
            else
            {
                //لملأ عناصر  الدروب داونلست بالبيانات
                List<tbl_Level> levlist = db.tbl_Level.ToList();
                List<tbl_Department> Deplist = db.tbl_Department.ToList();
                ViewData["Levels"] = new SelectList(levlist, "Level_Id", "Level_Name");
                ViewData["Departments"] = new SelectList(Deplist, "Department_Id", "Department_Name");
                return View();
            }

        }
        // زرار إضافة إعلان
        [HttpPost]
        public ActionResult AddAnnouncement(tbl_Announcement AddAnnou)
        {
            if (Session["Teacher_Id"] == null || Session["Teacher_Name"] == null)
            {
                return RedirectToAction("TeacherLogin", "Home");
            }
            else
            {
                if (ModelState.IsValid)
                {
                    // للحصول على رقم المعرف المدرس الحالي
                    int Id = Convert.ToInt32(Session["Teacher_Id"]);
                    tbl_Announcement Ann = new tbl_Announcement();
                    Ann.Announcement_Title = AddAnnou.Announcement_Title;
                    Ann.Announcement_Details = AddAnnou.Announcement_Details;
                    Ann.Announcement_CreatedOn = DateTime.Now;
                    Ann.Teacher_Id = Id;
                    Ann.Level_Id = Convert.ToInt32(AddAnnou.Level_Id);
                    Ann.Department_Id = Convert.ToInt32(AddAnnou.Department_Id);
                    Ann.IsDeleted = false;
                    db.tbl_Announcement.Add(Ann);
                    db.SaveChanges();
                    return RedirectToAction("AnnouncementManagement");
                }
               else
                {
                    List<tbl_Level> levlist = db.tbl_Level.ToList();
                    List<tbl_Department> Deplist = db.tbl_Department.ToList();
                    ViewData["Levels"] = new SelectList(levlist, "Level_Id", "Level_Name");
                    ViewData["Departments"] = new SelectList(Deplist, "Department_Id", "Department_Name");
                    return View(AddAnnou);

                }
            }
        }
        // صفحة حذف إعلان
        [HttpGet]
        public ActionResult DeleteAnnouncement(int? id)
        {

            if (Session["Teacher_Id"] == null || Session["Teacher_Name"] == null)
            {
                return RedirectToAction("TeacherLogin", "Home");
            }
            else
            {
                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                else
                {
                    var CurrentAnnouncement = db.tbl_Announcement.Find(id);
                    TempData["AnnouncementId"] = id;
                    TempData.Keep();
                    return View(CurrentAnnouncement);
                }
            }
        }
        // زرار حذف إعلان
        [HttpPost]
        public ActionResult DeleteAnnouncement(tbl_Teacher EdTeach)
        {
            if (Session["Teacher_Id"] == null || Session["Teacher_Name"] == null)
            {
                return RedirectToAction("TeacherLogin", "Home");
            }
            else
            {
                int id = (int)TempData["AnnouncementId"];
                var Anno = db.tbl_Announcement.Where(a => a.Announcement_Id == id).FirstOrDefault();
                Anno.IsDeleted = true;
                db.Entry(Anno).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("AnnouncementManagement");
            }
        }
        // صفحة إدارة المواضيع
        public ActionResult SubjectManagement()
        {
            if (Session["Teacher_Id"] == null || Session["Teacher_Name"] == null)
            {
                return RedirectToAction("TeacherLogin", "Home");
            }
            else
            {
                string Name = Session["Teacher_Name"].ToString();
                ViewBag.Name = Name;
                int Id = Convert.ToInt32(Session["Teacher_Id"]);
                return View(db.tbl_Subject.Where(a=>a.Teacher_Id == Id).ToList());
            }
        }
        // صفحة إضافة موضوع
        [HttpGet]
        public ActionResult AddSubject()
        {

            if (Session["Teacher_Id"] == null || Session["Teacher_Name"] == null)
            {
                return RedirectToAction("TeacherLogin", "Home");
            }
            else
            {
                //لملأ عناصر  الدروب داونلست بالبيانات
                List<tbl_Level> levlist = db.tbl_Level.ToList();
                List<tbl_Department> Deplist = db.tbl_Department.ToList();
                ViewData["Levels"] = new SelectList(levlist, "Level_Id", "Level_Name");
                ViewData["Departments"] = new SelectList(Deplist, "Department_Id", "Department_Name");
                return View();
            }

        }
        // زرار إضافة موضوع 
        [HttpPost]
        public ActionResult AddSubject(tbl_Subject AddSubj)
        {

            if (Session["Teacher_Id"] == null || Session["Teacher_Name"] == null)
            {
                return RedirectToAction("TeacherLogin", "Home");
            }
            else
            {
               
                if (ModelState.IsValid)
                {
                    if (!db.tbl_Subject.Any(x => x.Subject_Code == AddSubj.Subject_Code))
                    {
                        int Id = Convert.ToInt32(Session["Teacher_Id"]);
                        tbl_Subject Sub = new tbl_Subject();
                        Sub.Subject_Name = AddSubj.Subject_Name;
                        Sub.Subject_Code = AddSubj.Subject_Code;
                        Sub.Department_Id = AddSubj.Department_Id;
                        Sub.Level_Id = AddSubj.Level_Id;
                        Sub.Subject_CreatedOn = DateTime.Now;
                        Sub.Teacher_Id = Id;
                        db.tbl_Subject.Add(Sub);
                        db.SaveChanges();
                        return RedirectToAction("SubjectManagement");
                    }
                    else
                    {
                        ViewBag.msg = "هذا الكود موجود بالفعل";
                       
                    }
                }
                List<tbl_Level> levlist = db.tbl_Level.ToList();
                List<tbl_Department> Deplist = db.tbl_Department.ToList();
                ViewData["Levels"] = new SelectList(levlist, "Level_Id", "Level_Name");
                ViewData["Departments"] = new SelectList(Deplist, "Department_Id", "Department_Name");
                return View(AddSubj);
            }
        }
        // صفحة تفاصيل موضوع
        public ActionResult DetailsSubject(int? id)
        {
            if (Session["Teacher_Id"] == null || Session["Teacher_Name"] == null)
            {
                return RedirectToAction("TeacherLogin", "Home");
            }
            else
            {
                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                else
                {
                    int Idt = Convert.ToInt32(Session["Teacher_Id"]);
                    var CurrentSubject = db.tbl_Subject.Single(a => a.Subject_Id == id && a.Teacher_Id == Idt);
                    return View(CurrentSubject);
                }

            }
        }
        // صفحة تعديل موضوع
        [HttpGet]
        public ActionResult EditSubject(int? id)
        {
            if (Session["Teacher_Id"] == null || Session["Teacher_Name"] == null)
            {
                return RedirectToAction("TeacherLogin", "Home");
            }
            else
            {
                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                else
                {
                    List<tbl_Level> levlist = db.tbl_Level.ToList();
                    List<tbl_Department> Deplist = db.tbl_Department.ToList();
                    ViewData["Levels"] = new SelectList(levlist, "Level_Id", "Level_Name");
                    ViewData["Departments"] = new SelectList(Deplist, "Department_Id", "Department_Name");

                    int Idt = Convert.ToInt32(Session["Teacher_Id"]);
                    var CurrentSubject = db.tbl_Subject.Single(a => a.Subject_Id == id && a.Teacher_Id == Idt);
                    TempData["SubjectId"] = id;
                    TempData.Keep();
                    return View(CurrentSubject);
                }
            }

        }
        // زرار تعديل موضوع
        [HttpPost]
        public ActionResult EditSubject([Bind(Exclude = "Subject_CreatedOn,Teacher_Id,Subject_Id")]tbl_Subject EdSub)
        {
            if (Session["Teacher_Id"] == null || Session["Teacher_Name"] == null)
            {
                return RedirectToAction("TeacherLogin", "Home");
            }
            else
            {
                if (ModelState.IsValid)
                {
                    int id = (int)TempData["SubjectId"];
                    var Subject = db.tbl_Subject.Where(a => a.Subject_Id == id).FirstOrDefault();
                    Subject.Subject_Name = EdSub.Subject_Name;
                    Subject.Level_Id = EdSub.Level_Id;
                    Subject.Department_Id = EdSub.Department_Id;
                    db.Entry(Subject).State = EntityState.Modified;
                    db.SaveChanges();
                    return RedirectToAction("SubjectManagement");
                }
                List<tbl_Level> levlist = db.tbl_Level.ToList();
                List<tbl_Department> Deplist = db.tbl_Department.ToList();
                ViewData["Levels"] = new SelectList(levlist, "Level_Id", "Level_Name");
                ViewData["Departments"] = new SelectList(Deplist, "Department_Id", "Department_Name");
                return View();
            }
        }
        // صفحة حذف موضوع
        [HttpGet]
        public ActionResult DeleteSubject(int? id)
        {

            if (Session["Teacher_Id"] == null || Session["Teacher_Name"] == null)
            {
                return RedirectToAction("TeacherLogin", "Home");
            }
            else
            {
                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                else
                {
                    int Idt = Convert.ToInt32(Session["Teacher_Id"]);
                    var CurrentSubject = db.tbl_Subject.Single(a => a.Subject_Id == id && a.Teacher_Id == Idt);
                    TempData["SubjectId"] = id;
                    TempData.Keep();
                    return View(CurrentSubject);
                }
            }
        }
        // زرار حذف موضوع
        [HttpPost]
        public ActionResult DeleteSubject(tbl_Subject DelSub)
        {
            if (Session["Teacher_Id"] == null || Session["Teacher_Name"] == null)
            {
                return RedirectToAction("TeacherLogin", "Home");
            }
            else
            {
                try
                {
                    int id = (int)TempData["SubjectId"];
                    tbl_Subject sub = db.tbl_Subject.Find(id);
                    db.tbl_Subject.Remove(sub);
                    db.SaveChanges();
                    return RedirectToAction("SubjectManagement");
                }
                catch
                {
                    ViewBag.msg = "يوجد بيانات متربطة بهذا الموضوع";
                }
            }
            return View();
        }
        // صفحة إدارة الامتحانات
        public ActionResult ExamManagement()
        {
            if (Session["Teacher_Id"] == null || Session["Teacher_Name"] == null)
            {
                return RedirectToAction("TeacherLogin", "Home");
            }
            else
            {
                string Name = Session["Teacher_Name"].ToString();
                ViewBag.Name = Name;
                int Id = Convert.ToInt32(Session["Teacher_Id"]);
                return View(db.tbl_Exam.Where(a => a.Teacher_Id == Id).ToList());
            }
        }
        // صفحة إضافة امتحان
        [HttpGet]
        public ActionResult AddExam()
        {

            if (Session["Teacher_Id"] == null || Session["Teacher_Name"] == null)
            {
                return RedirectToAction("TeacherLogin", "Home");
            }
            else
            {
                //لملأ عناصر  الدروب داونلست بالبيانات
                List<tbl_Subject> sub = db.tbl_Subject.ToList();
                ViewData["Subject"] = new SelectList(sub, "Subject_Id", "Subject_Name");
                return View();
            }

        }
        // زرار إضافة إمتحان
        [HttpPost]
        public ActionResult AddExam(tbl_Exam AddExam )
        {

            if (Session["Teacher_Id"] == null || Session["Teacher_Name"] == null)
            {
                return RedirectToAction("TeacherLogin", "Home");
            }
            else
            {

                if (ModelState.IsValid)
                {
                   if(AddExam.Exam_PassingMark < AddExam.Exam_TotalMark)
                    {
                        TimeSpan difTime = AddExam.Exam_EndOn.Subtract(AddExam.Exam_StartOn);
                        if (AddExam.Exam_EndOn > AddExam.Exam_StartOn && AddExam.Exam_StartOn>= DateTime.Now &&AddExam.Exam_EndOn>DateTime.Now &&(int ) difTime.TotalMinutes >AddExam.Exam_MuniteDuration && AddExam.Exam_MuniteDuration >0)
                        {
                            int Id = Convert.ToInt32(Session["Teacher_Id"]);
                            tbl_Exam exam = new tbl_Exam();
                            exam.Exam_Name = AddExam.Exam_Name;
                            exam.Exam_Instruction = AddExam.Exam_Instruction;
                            exam.Exam_MuniteDuration = (int)AddExam.Exam_MuniteDuration;
                            exam.Exam_TotalMark = (int)AddExam.Exam_TotalMark;
                            exam.Exam_PassingMark = (int)AddExam.Exam_PassingMark;
                            exam.Teacher_Id = Id;
                            exam.Subject_Id = AddExam.Subject_Id;
                            exam.Exam_StartOn = AddExam.Exam_StartOn;
                            exam.Exam_EndOn = AddExam.Exam_EndOn;
                            exam.Exam_CreatedOn = DateTime.Now;
                            db.tbl_Exam.Add(exam);
                            db.SaveChanges();
                            return RedirectToAction("ExamManagement");
                        }
                        else
                        {
                            ViewBag.msg = "هناك خطأ في اختيار وقت بدء وانهاء الاختبار ، لابد أن يكون وقت نهاية الاختبار اكبر من بدايته و وقت الامتحان كافي";

                        }

                    }
                   else
                    {
                        ViewBag.msg = "هناك خطأ في توزيع الدرجات ، لابد أن تكون درجات النجاح في المادة أقل من المجموع الكلي للدرجات";
                    }


                }
              
                List<tbl_Subject> sub = db.tbl_Subject.ToList();
                ViewData["Subject"] = new SelectList(sub, "Subject_Id", "Subject_Name");
                return View(AddExam);
            }
        }
        // صفحة تفاصيل امتحان
        public ActionResult DetailsExam(int? id)
        {
            if (Session["Teacher_Id"] == null || Session["Teacher_Name"] == null)
            {
                return RedirectToAction("TeacherLogin", "Home");
            }
            else
            {
                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                else
                {
                    int Idt = Convert.ToInt32(Session["Teacher_Id"]);
                    var CurrentExam = db.tbl_Exam.Single(a => a.Exam_Id == id && a.Teacher_Id == Idt);
                    return View(CurrentExam);
                }

            }
        }
        // صفحة تعديل امتحان
        [HttpGet]
        public ActionResult EditExam(int? id)
        {
            if (Session["Teacher_Id"] == null || Session["Teacher_Name"] == null)
            {
                return RedirectToAction("TeacherLogin", "Home");
            }
            else
            {
                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                else
                {
                    List<tbl_Subject> sub = db.tbl_Subject.ToList();
                    ViewData["Subject"] = new SelectList(sub, "Subject_Id", "Subject_Name");

                    int Idt = Convert.ToInt32(Session["Teacher_Id"]);
                    var CurrentExam = db.tbl_Exam.Single(a => a.Exam_Id == id && a.Teacher_Id == Idt);
                    TempData["ExamId"] = id;
                    TempData.Keep();
                    return View(CurrentExam);
                }
            }
        }
        [HttpPost]
        public ActionResult EditExam([Bind(Exclude = "Exam_CreatedOn,Teacher_Id")]tbl_Exam EdExam)
        {
            if (Session["Teacher_Id"] == null || Session["Teacher_Name"] == null)
            {
                return RedirectToAction("TeacherLogin", "Home");
            }
            else
            {
                if (ModelState.IsValid)
                {
                    if (EdExam.Exam_PassingMark < EdExam.Exam_TotalMark)
                    {

                        TimeSpan difTime = EdExam.Exam_EndOn.Subtract(EdExam.Exam_StartOn);
                        if (EdExam.Exam_EndOn > EdExam.Exam_StartOn && EdExam.Exam_StartOn >= DateTime.Now && EdExam.Exam_EndOn > DateTime.Now &&(int)difTime.TotalMinutes > EdExam.Exam_MuniteDuration && EdExam.Exam_MuniteDuration > 0)
                        {
                        int id = (int)TempData["ExamId"];
                        var exam = db.tbl_Exam.Where(a => a.Exam_Id == id).FirstOrDefault();
                        exam.Exam_Name = EdExam.Exam_Name;
                        exam.Exam_Instruction = EdExam.Exam_Instruction;
                        exam.Exam_MuniteDuration = (int)EdExam.Exam_MuniteDuration;
                        exam.Exam_TotalMark = (int)EdExam.Exam_TotalMark;
                        exam.Exam_PassingMark = (int)EdExam.Exam_PassingMark;
                        exam.Subject_Id = EdExam.Subject_Id;
                        exam.Exam_StartOn = EdExam.Exam_StartOn;
                        exam.Exam_EndOn = EdExam.Exam_EndOn;
                        db.Entry(exam).State = EntityState.Modified;
                        db.SaveChanges();
                        return RedirectToAction("ExamManagement");
                        }
                        else
                        {
                            ViewBag.msg = "هناك خطأ في اختيار وقت بدء وانهاء الاختبار ، لابد أن يكون وقت نهاية الاختبار اكبر من بدايته و وقت الامتحان كافي";

                        }
                    }
                    else
                    {
                        ViewBag.msg = "هناك خطأ في توزيع الدرجات ، لابد أن تكون درجات النجاح في المادة أقل من المجموع الكلي للدرجات";
                    }
                }
                List<tbl_Subject> sub = db.tbl_Subject.ToList();
                ViewData["Subject"] = new SelectList(sub, "Subject_Id", "Subject_Name");
                return View();
            }
        }
        // صفحة حذف امتحان
        [HttpGet]
        public ActionResult DeleteExam(int? id)
        {

            if (Session["Teacher_Id"] == null || Session["Teacher_Name"] == null)
            {
                return RedirectToAction("TeacherLogin", "Home");
            }
            else
            {
                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                else
                {
                    int Idt = Convert.ToInt32(Session["Teacher_Id"]);
                    var CurrentExam = db.tbl_Exam.Single(a => a.Exam_Id == id && a.Teacher_Id == Idt);
                    TempData["ExamId"] = id;
                    TempData.Keep();
                    return View(CurrentExam);
                }
            }
        }
        // زرار حذف امتحان
        [HttpPost]
        public ActionResult DeleteExam(tbl_Exam DelExam)
        {
            if (Session["Teacher_Id"] == null || Session["Teacher_Name"] == null)
            {
                return RedirectToAction("TeacherLogin", "Home");
            }
            else
            {
                try
                {
                    int id = (int)TempData["ExamId"];
                    tbl_Exam exam = db.tbl_Exam.Find(id);
                    db.tbl_Exam.Remove(exam);
                    db.SaveChanges();
                    return RedirectToAction("ExamManagement");
                }
                catch
                {
                    ViewBag.msg = "يوجد بيانات متربطة بهذا الامتحان";
                }
            }
            return View();
        }
        // صفحة إدارة الاسئلة
        public ActionResult QuestionsMangement()
        {
            if (Session["Teacher_Id"] == null || Session["Teacher_Name"] == null)
            {
                return RedirectToAction("TeacherLogin", "Home");
            }
            else
            {
                string Name = Session["Teacher_Name"].ToString();
                ViewBag.Name = Name;
                int Id = Convert.ToInt32(Session["Teacher_Id"]);
                return View(db.tbl_Exam.Where(a => a.Teacher_Id == Id).ToList());
            }
        }
        // إدارة الاختياري
        public ActionResult MCQ(int? Exams)
        {
            if (Session["Teacher_Id"] == null || Session["Teacher_Name"] == null)
            {
                return RedirectToAction("TeacherLogin", "Home");
            }
            else
            {
                List<tbl_Exam> exam = db.tbl_Exam.ToList();
                ViewData["Exams"] = new SelectList(exam, "Exam_Id", "Exam_Name");
                int Idt = Convert.ToInt32(Session["Teacher_Id"]);
                return View(db.tbl_QuestionMCQ.Where(a => a.Teacher_Id == Idt && a.Exam_Id == Exams || Exams == null));
            }
        }
        // صفحة إضافة سؤال ام سي كيو
        [HttpGet]
        public ActionResult AddMCQ()
        {

            if (Session["Teacher_Id"] == null || Session["Teacher_Name"] == null)
            {
                return RedirectToAction("TeacherLogin", "Home");
            }
            else
            {
                 
                List<tbl_Exam> exam = db.tbl_Exam.ToList();
                ViewData["Exams"] = new SelectList(exam, "Exam_Id", "Exam_Name");
                return View();
            }
        }
        // زرار إضافة سؤال ام سي كيو
        [HttpPost]
        public ActionResult AddMCQ(tbl_QuestionMCQ AddMcq)
        {

            if (Session["Teacher_Id"] == null || Session["Teacher_Name"] == null)
            {
                return RedirectToAction("TeacherLogin", "Home");
            }
            else
            {
                string Correct = null;
                if (ModelState.IsValid)
                {

                    int IdTech = Convert.ToInt32(Session["Teacher_Id"]);
                    var McqT = db.tbl_QuestionMCQ.Where(a => a.Exam_Id == AddMcq.Exam_Id && a.Teacher_Id == IdTech);
                    var TFT = db.tbl_QuestionTrueFalse.Where(a => a.Exam_Id == AddMcq.Exam_Id && a.Teacher_Id == IdTech);
                    var ExamT = db.tbl_Exam.Single(a => a.Exam_Id == AddMcq.Exam_Id && a.Teacher_Id == IdTech);

                    int DegMCq = McqT.Sum(a => (int ? )a.QuestionMCQ_Mark) ??0;
                    int DegTF = TFT.Sum(a => (int?) a.QuestionTrueFalse_Mark) ??0;

                    int Total = DegMCq + DegTF;
                    int TotalExam = ExamT.Exam_TotalMark;
                    if (Total < TotalExam &&  AddMcq.QuestionMCQ_Mark <= (TotalExam-Total))
                    {
                        if (AddMcq.QuestionMCQ_CorrectAnswer.Equals("A"))
                        {
                            Correct = AddMcq.QuestionMCQ_OptionA;
                        }
                        else if (AddMcq.QuestionMCQ_CorrectAnswer.Equals("B"))
                        {
                            Correct = AddMcq.QuestionMCQ_OptionB;
                        }
                        else if (AddMcq.QuestionMCQ_CorrectAnswer.Equals("C"))
                        {
                            Correct = AddMcq.QuestionMCQ_OptionC;
                        }
                        else if (AddMcq.QuestionMCQ_CorrectAnswer.Equals("D"))
                        {
                            Correct = AddMcq.QuestionMCQ_OptionD;
                        }

                        tbl_QuestionMCQ Mcq = new tbl_QuestionMCQ();
                        Mcq.QuestionMCQ_Text = AddMcq.QuestionMCQ_Text;
                        Mcq.QuestionMCQ_OptionA = AddMcq.QuestionMCQ_OptionA;
                        Mcq.QuestionMCQ_OptionB = AddMcq.QuestionMCQ_OptionB;
                        Mcq.QuestionMCQ_OptionC = AddMcq.QuestionMCQ_OptionC;
                        Mcq.QuestionMCQ_OptionD = AddMcq.QuestionMCQ_OptionD;
                        Mcq.QuestionMCQ_Mark = AddMcq.QuestionMCQ_Mark;
                        Mcq.Teacher_Id = IdTech;
                        Mcq.Exam_Id = AddMcq.Exam_Id;
                        Mcq.QuestionMCQ_CorrectAnswer = Correct;
                        db.tbl_QuestionMCQ.Add(Mcq);
                        db.SaveChanges();
                        return RedirectToAction("MCQ");
                    }                
                    else
                    {
                        ViewBag.msg = "لا يمكنك إضافة المزيد من الدرجات الرجاء التأكد من مجموعها";
                    }
                   
                }
                List<tbl_Exam> exam = db.tbl_Exam.ToList();
                ViewData["Exams"] = new SelectList(exam, "Exam_Id", "Exam_Name");
                return View(AddMcq);
            }
        }
        // صفحة تفاصيل سؤال
        public ActionResult DetailsMCQ(int? id)
        {
            if (Session["Teacher_Id"] == null || Session["Teacher_Name"] == null)
            {
                return RedirectToAction("TeacherLogin", "Home");
            }
            else
            {
                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                else
                {
                    int Idt = Convert.ToInt32(Session["Teacher_Id"]);
                    var CurrMcq = db.tbl_QuestionMCQ.Single(a => a.QuestionMCQ_Id == id && a.Teacher_Id == Idt);
                    return View(CurrMcq);
                }

            }
        }
        // صفحة تعديل سؤال
        [HttpGet]
        public ActionResult EditMCQ(int? id)
        {
            if (Session["Teacher_Id"] == null || Session["Teacher_Name"] == null)
            {
                return RedirectToAction("TeacherLogin", "Home");
            }
            else
            {
                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                else
                {
                    string Correct = null;
                    List<tbl_Exam> exam = db.tbl_Exam.ToList();
                    ViewData["Exams"] = new SelectList(exam, "Exam_Id", "Exam_Name");            

                    int Idt = Convert.ToInt32(Session["Teacher_Id"]);
                    var CurrMcq = db.tbl_QuestionMCQ.Single(a => a.QuestionMCQ_Id == id && a.Teacher_Id == Idt);
                    if (CurrMcq.QuestionMCQ_CorrectAnswer == CurrMcq.QuestionMCQ_OptionA)
                    {
                        Correct = "A";
                    }
                    else if (CurrMcq.QuestionMCQ_CorrectAnswer == CurrMcq.QuestionMCQ_OptionB)
                    {
                        Correct = "B";
                    }
                    else if (CurrMcq.QuestionMCQ_CorrectAnswer == CurrMcq.QuestionMCQ_OptionC)
                    {
                        Correct = "C";
                    }
                    else if (CurrMcq.QuestionMCQ_CorrectAnswer == CurrMcq.QuestionMCQ_OptionD)
                    {
                        Correct = "D";
                    }
                    CurrMcq.QuestionMCQ_CorrectAnswer = Correct;
                    TempData["QestionId"] = id;
                    TempData.Keep();
                    return View(CurrMcq);

                }
            }

        }
        // زرار تعديل سؤال
        [HttpPost]
        public ActionResult EditMCQ([Bind(Exclude = "Teacher_Id")]tbl_QuestionMCQ EdMcq)
        {
            if (Session["Teacher_Id"] == null || Session["Teacher_Name"] == null)
            {
                return RedirectToAction("TeacherLogin", "Home");
            }
            else
            {

                if (ModelState.IsValid)
                {
                    string Correct = null;
                    if (EdMcq.QuestionMCQ_CorrectAnswer.Equals("A"))
                    {
                        Correct = EdMcq.QuestionMCQ_OptionA;
                    }
                    else if (EdMcq.QuestionMCQ_CorrectAnswer.Equals("B"))
                    {
                        Correct = EdMcq.QuestionMCQ_OptionB;
                    }
                    else if (EdMcq.QuestionMCQ_CorrectAnswer.Equals("C"))
                    {
                        Correct = EdMcq.QuestionMCQ_OptionC;
                    }
                    else if (EdMcq.QuestionMCQ_CorrectAnswer.Equals("D"))
                    {
                        Correct = EdMcq.QuestionMCQ_OptionD;
                    }
                    int id = (int)TempData["QestionId"];
                    var mcq = db.tbl_QuestionMCQ.Where(a => a.QuestionMCQ_Id == id).FirstOrDefault();
                    mcq.QuestionMCQ_Text = EdMcq.QuestionMCQ_Text;
                    mcq.QuestionMCQ_OptionA = EdMcq.QuestionMCQ_OptionA;
                    mcq.QuestionMCQ_OptionB = EdMcq.QuestionMCQ_OptionB;
                    mcq.QuestionMCQ_OptionC = EdMcq.QuestionMCQ_OptionC;
                    mcq.QuestionMCQ_OptionD = EdMcq.QuestionMCQ_OptionD;
                    mcq.QuestionMCQ_CorrectAnswer = Correct;
                    mcq.Exam_Id = EdMcq.Exam_Id;
                    mcq.QuestionMCQ_Mark = EdMcq.QuestionMCQ_Mark;
                    db.Entry(mcq).State = EntityState.Modified;
                    db.SaveChanges();
                    return RedirectToAction("MCQ");

                }

                List<tbl_Exam> exam = db.tbl_Exam.ToList();
                ViewData["Exams"] = new SelectList(exam, "Exam_Id", "Exam_Name");
                return View();
            }
        }
        // صفحة حذف سؤل
        [HttpGet]
        public ActionResult DeleteMCQ(int? id)
        {

            if (Session["Teacher_Id"] == null || Session["Teacher_Name"] == null)
            {
                return RedirectToAction("TeacherLogin", "Home");
            }
            else
            {
                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                else
                {
                    int Idt = Convert.ToInt32(Session["Teacher_Id"]);
                    var CurrMcq = db.tbl_QuestionMCQ.FirstOrDefault(a => a.QuestionMCQ_Id == id && a.Teacher_Id == Idt);
                    TempData["QestionId"] = id;
                    TempData.Keep();
                    return View(CurrMcq);
                }
            }
        }
        // زرار حذف سؤال
        [HttpPost]
        public ActionResult DeleteMCQ(tbl_QuestionMCQ DelMcq)
        {
            if (Session["Teacher_Id"] == null || Session["Teacher_Name"] == null)
            {
                return RedirectToAction("TeacherLogin", "Home");
            }
            else
            {
                int id = (int)TempData["QestionId"];
                tbl_QuestionMCQ mcq = db.tbl_QuestionMCQ.Find(id);
                db.tbl_QuestionMCQ.Remove(mcq);
                db.SaveChanges();
                return RedirectToAction("MCQ");
            }
        }
        // إدارة التصحيحي
        public ActionResult TrueFalse(int? Exams)
        {
            if (Session["Teacher_Id"] == null || Session["Teacher_Name"] == null)
            {
                return RedirectToAction("TeacherLogin", "Home");
            }
            else
            {
                List<tbl_Exam> exam = db.tbl_Exam.ToList();
                ViewData["Exams"] = new SelectList(exam, "Exam_Id", "Exam_Name");
                int Idt = Convert.ToInt32(Session["Teacher_Id"]);
                return View(db.tbl_QuestionTrueFalse.Where(a => a.Teacher_Id == Idt && a.Exam_Id == Exams || Exams == null));
            }
        }
        // صفحة إضافة سؤال تصحيحي
        [HttpGet]
        public ActionResult AddTrueFalse()
        {

            if (Session["Teacher_Id"] == null || Session["Teacher_Name"] == null)
            {
                return RedirectToAction("TeacherLogin", "Home");
            }
            else
            {
                List<tbl_Exam> exam = db.tbl_Exam.ToList();
                ViewData["Exams"] = new SelectList(exam, "Exam_Id", "Exam_Name");
                return View();
            }
        }
        // زرار إضافة سؤال تصحيحي
        [HttpPost]
        public ActionResult AddTrueFalse(tbl_QuestionTrueFalse AddTF)
        {

            if (Session["Teacher_Id"] == null || Session["Teacher_Name"] == null)
            {
                return RedirectToAction("TeacherLogin", "Home");
            }
            else
            {
                
                if (ModelState.IsValid)
                {
                    int IdTech = Convert.ToInt32(Session["Teacher_Id"]);
                    var McqT = db.tbl_QuestionMCQ.Where(a => a.Exam_Id == AddTF.Exam_Id && a.Teacher_Id == IdTech);
                    var TFT = db.tbl_QuestionTrueFalse.Where(a => a.Exam_Id == AddTF.Exam_Id && a.Teacher_Id == IdTech);
                    var ExamT = db.tbl_Exam.Single(a => a.Exam_Id == AddTF.Exam_Id && a.Teacher_Id == IdTech);


                    int DegMCq = McqT.Sum(a => (int?)a.QuestionMCQ_Mark) ?? 0;
                    int DegTF = TFT.Sum(a => (int?)a.QuestionTrueFalse_Mark) ?? 0;

                    int Total = DegMCq + DegTF;
                    int TotalExam = ExamT.Exam_TotalMark;
                    if (Total < TotalExam && AddTF.QuestionTrueFalse_Mark <= (TotalExam - Total))
                    {

                        tbl_QuestionTrueFalse TOF = new tbl_QuestionTrueFalse();
                        TOF.QuestionTrueFalse_text = AddTF.QuestionTrueFalse_text;
                        TOF.QuestionTrueFalse_OptionA = "True";
                        TOF.QuestionTrueFalse_OptionB = "False";
                        TOF.QuestionTrueFalse_CorrectAnswer = AddTF.QuestionTrueFalse_CorrectAnswer;
                        TOF.QuestionTrueFalse_Mark = AddTF.QuestionTrueFalse_Mark;
                        TOF.Teacher_Id = IdTech;
                        TOF.Exam_Id = AddTF.Exam_Id;
                        db.tbl_QuestionTrueFalse.Add(TOF);
                        db.SaveChanges();
                        return RedirectToAction("TrueFalse");
                    }
                    else
                    {
                        ViewBag.msg = "لا يمكنك إضافة المزيد من الدرجات الرجاء التأكد من مجموعها";
                    }
                }
                List<tbl_Exam> exam = db.tbl_Exam.ToList();
                ViewData["Exams"] = new SelectList(exam, "Exam_Id", "Exam_Name");
                return View(AddTF);
            }
        }
        // صفحة تفاصيل سؤال
        public ActionResult DetailsTrueFalse(int? id)
        {
            if (Session["Teacher_Id"] == null || Session["Teacher_Name"] == null)
            {
                return RedirectToAction("TeacherLogin", "Home");
            }
            else
            {
                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                else
                {
                    int Idt = Convert.ToInt32(Session["Teacher_Id"]);
                    var CurrTF = db.tbl_QuestionTrueFalse.Single(a => a.QuestionTrueFalse_Id == id && a.Teacher_Id == Idt);
                    return View(CurrTF);
                }

            }
        }
        // صفحة تعديل سؤال
        [HttpGet]
        public ActionResult EditTrueFalse(int? id)
        {
            if (Session["Teacher_Id"] == null || Session["Teacher_Name"] == null)
            {
                return RedirectToAction("TeacherLogin", "Home");
            }
            else
            {
                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                else
                {
                    List<tbl_Exam> exam = db.tbl_Exam.ToList();
                    ViewData["Exams"] = new SelectList(exam, "Exam_Id", "Exam_Name");

                    int Idt = Convert.ToInt32(Session["Teacher_Id"]);
                    var CurrTOF = db.tbl_QuestionTrueFalse.Single(a => a.QuestionTrueFalse_Id == id && a.Teacher_Id == Idt);
                    TempData["QestionTOFId"] = id;
                    TempData.Keep();
                    return View(CurrTOF);
                }
            }

        }
        // زرار تعديل سؤال
        [HttpPost]
        public ActionResult EditTrueFalse([Bind(Exclude = "Teacher_Id,QuestionTrueFalse_OptionA,QuestionTrueFalse_OptionB")]tbl_QuestionTrueFalse EdTof)
        {
            if (Session["Teacher_Id"] == null || Session["Teacher_Name"] == null)
            {
                return RedirectToAction("TeacherLogin", "Home");
            }
            else
            {
                if (ModelState.IsValid)
                {
                    int id = (int)TempData["QestionTOFId"];
                    var tf = db.tbl_QuestionTrueFalse.Where(a => a.QuestionTrueFalse_Id == id).FirstOrDefault();
                    tf.QuestionTrueFalse_text = EdTof.QuestionTrueFalse_text;
                    tf.Exam_Id = EdTof.Exam_Id;
                    tf.QuestionTrueFalse_CorrectAnswer = EdTof.QuestionTrueFalse_CorrectAnswer;
                    tf.QuestionTrueFalse_Mark = EdTof.QuestionTrueFalse_Mark;
                    db.Entry(tf).State = EntityState.Modified;
                    db.SaveChanges();
                    return RedirectToAction("TrueFalse");
                }
                List<tbl_Exam> exam = db.tbl_Exam.ToList();
                ViewData["Exams"] = new SelectList(exam, "Exam_Id", "Exam_Name");
                return View();
            }
        }
        // صفحة حذف سؤال
        [HttpGet]
        public ActionResult DeleteTrueFalse(int? id)
        {

            if (Session["Teacher_Id"] == null || Session["Teacher_Name"] == null)
            {
                return RedirectToAction("TeacherLogin", "Home");
            }
            else
            {
                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                else
                {
                    int Idt = Convert.ToInt32(Session["Teacher_Id"]);
                    var CurrTOF = db.tbl_QuestionTrueFalse.Single(a => a.QuestionTrueFalse_Id == id && a.Teacher_Id == Idt);
                    TempData["QestionTOFId"] = id;
                    TempData.Keep();
                    return View(CurrTOF);
                }
            }
        }
        // زرار حذف سؤال
        [HttpPost]
        public ActionResult DeleteTrueFalse(tbl_QuestionTrueFalse DelTof)
        {
            if (Session["Teacher_Id"] == null || Session["Teacher_Name"] == null)
            {
                return RedirectToAction("TeacherLogin", "Home");
            }
            else
            {
                int id = (int)TempData["QestionTOFId"];
                tbl_QuestionTrueFalse tf = db.tbl_QuestionTrueFalse.Find(id);
                db.tbl_QuestionTrueFalse.Remove(tf);
                db.SaveChanges();
                return RedirectToAction("TrueFalse");
            }
        }
        // عرض الامتحان
        public ActionResult ViewExam(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            else
            {
                int Idt = Convert.ToInt32(Session["Teacher_Id"]);
                var McqT = db.tbl_QuestionMCQ.Where(a => a.Exam_Id == id && a.Teacher_Id == Idt);
                var TFT = db.tbl_QuestionTrueFalse.Where(a => a.Exam_Id == id && a.Teacher_Id == Idt);
                var ExamT = db.tbl_Exam.Single(a => a.Exam_Id == id && a.Teacher_Id == Idt);

                int DegMCq = McqT.Sum(a => (int?)a.QuestionMCQ_Mark) ?? 0;
                int DegTF = TFT.Sum(a => (int?)a.QuestionTrueFalse_Mark) ?? 0;


                int Total = DegMCq + DegTF;
                int TotalExam = ExamT.Exam_TotalMark;

                List<tbl_QuestionTrueFalse> Ques_TOF = db.tbl_QuestionTrueFalse.Where(a => a.Exam_Id == id && a.Teacher_Id == Idt).ToList();
                List<tbl_QuestionMCQ> Ques_Mcq = db.tbl_QuestionMCQ.Where(a => a.Exam_Id == id && a.Teacher_Id == Idt).ToList();

                var Exam = db.tbl_Exam.SingleOrDefault(a => a.Exam_Id == id && a.Teacher_Id == Idt);

                ViewBag.Name = Exam.Exam_Name.ToString();
                ViewBag.Degre = Exam.Exam_TotalMark.ToString();
                ViewBag.Mint = Exam.Exam_MuniteDuration.ToString();
                ViewBag.Start = Exam.Exam_StartOn.ToString();
                ViewBag.End = Exam.Exam_EndOn.ToString();
                if (Total == TotalExam)
                {

                    ViewBag.Statue = "الامتحان كامل وجاهز";
                }
                else
                {
                    ViewBag.Statue = "الامتحان غير كامل";
                }

                QustionAll All = new QustionAll();
                All.Mcq = Ques_Mcq;
                All.TF = Ques_TOF;
                return View(All);

            }


        }
    }
}
