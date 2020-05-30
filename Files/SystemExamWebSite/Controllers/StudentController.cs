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
    public class StudentController : Controller
    {
        //للتعامل مع قاعة البياناتcre
        ExamSystemEntities db = new ExamSystemEntities();
        // GET: Student
        public ActionResult Index()
        {
            // شرط بيستخدم إن لو في حد حاول يدخل الرابط وهو مش مسجل دخول فبيرجعه لصفحة تسجيل الدخول
            if (Session["Studnet_Id"] == null || Session["Studnet_Name"] == null)
            {
                return RedirectToAction("StudentLogin", "Home");
            }
            else
            {
                // للحصول على اسم الالي الحالي
                string Name = Session["Studnet_Name"].ToString();
                // للحصول على رقم المعرف الطالب الحالي
                int Id = Convert.ToInt32(Session["Studnet_Id"]);
                int DeptId = Convert.ToInt32(Session["Department_Id"]);
                int LevelId = Convert.ToInt32(Session["Level_Id"]);
                // إرسال الطالب الحالي إلى صفحة العرض
                ViewBag.Name = Name;
                // عرض الإعلانات
                var Anonoun = db.tbl_Announcement.Where(a => a.IsDeleted == false && a.Level_Id == LevelId && a.Department_Id == DeptId).OrderByDescending(a => a.Announcement_Id).ToList();
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
        // صفحة تفاصيل الطالب الحالي من الاعدادات
        public ActionResult DetailsMe()
        {
            if (Session["Studnet_Id"] == null || Session["Studnet_Name"] == null)
            {
                return RedirectToAction("StudentLogin", "Home");
            }
            else
            {
                int Id = Convert.ToInt32(Session["Studnet_Id"]);
                var CurrentStu = db.tbl_Student.Single(a => a.Student_Id == Id);
                return View(CurrentStu);

            }
        }
        //صفحة تعديل الطالب الحالي من الاعدادات
        [HttpGet]
        public ActionResult EditMe()
        {
            if (Session["Studnet_Id"] == null || Session["Studnet_Name"] == null)
            {
                return RedirectToAction("StudentLogin", "Home");
            }
            else
            {
                int Id = Convert.ToInt32(Session["Studnet_Id"]);
                var CurrentStu = db.tbl_Student.Find(Id);
                TempData["StudentId"] = Id;
                TempData.Keep();
                return View(CurrentStu);
            }

        }
        // زرار تعديل الطالب الحالي
        [HttpPost]
        public ActionResult EditMe([Bind(Exclude = ("Teacher_CreatedOn,Level_Id,Department_Id,IsDeleted"))]tbl_Student EdStu)
        {
            if (Session["Studnet_Id"] == null || Session["Studnet_Name"] == null)
            {
                return RedirectToAction("StudentLogin", "Home");
            }
            else
            {
                if (ModelState.IsValid)
                {

                    int id = (int)TempData["StudentId"];
                    var Stu = db.tbl_Student.Where(a => a.Student_Id == id).FirstOrDefault();
                    Stu.Student_Name = EdStu.Student_Name;
                    Stu.Student_Email = EdStu.Student_Email;
                    Stu.Student_Password = EdStu.Student_Password;
                    Stu.Student_Phone = EdStu.Student_Phone;
                    db.Entry(Stu).State = EntityState.Modified;
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
                return View();
            }
        }
        // صفحة حذف الطالب الحالي
        [HttpGet]
        public ActionResult DeleteMe()
        {
            if (Session["Studnet_Id"] == null || Session["Studnet_Name"] == null)
            {
                return RedirectToAction("StudentLogin", "Home");
            }
            else
            {
                int Id = Convert.ToInt32(Session["Studnet_Id"]);
                var CurrentStu = db.tbl_Student.Find(Id);
                TempData["StudentId"] = Id;
                TempData.Keep();
                return View(CurrentStu);
            }
        }
        // زرار حذف الطالب الحالي ويتم توجيه بعدها إلى الصفحة الرئيسية
        [HttpPost]
        public ActionResult DeleteMe([Bind(Exclude = ("Student_Name,Student_Email,Student_Password,Student_Phone,Level_Id,Department_Id"))]tbl_Student EdStu)
        {
            if (Session["Studnet_Id"] == null || Session["Studnet_Name"] == null)
            {
                return RedirectToAction("StudentLogin", "Home");
            }
            else
            {
                int id = (int)TempData["StudentId"];
                var Stu = db.tbl_Student.Where(a => a.Student_Id == id).FirstOrDefault();
                Stu.IsDeleted = true ;
                db.Entry(Stu).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index", "Home");
            }
        }
        // عرض النتايج
        public ActionResult ExamResult()
        {
            if (Session["Studnet_Id"] == null || Session["Studnet_Name"] == null)
            {
                return RedirectToAction("StudentLogin", "Home");
            }
            else
            {

                // للحصول على رقم المعرف الطالب الحالي
                int Id = Convert.ToInt32(Session["Studnet_Id"]);
                var CurrentSudent = db.tbl_Result.Where(a => a.Student_Id == Id && a.tbl_Student.IsDeleted == false).ToList();
                return View(CurrentSudent);
            }
        }
        // عرض الامتحانات المتاحة
        public ActionResult AvaliableExam()
        {
            if (Session["Studnet_Id"] == null || Session["Studnet_Name"] == null)
            {
                return RedirectToAction("StudentLogin", "Home");
            }
            else
            {
                int DeptId = Convert.ToInt32(Session["Department_Id"]);
                int LevelId = Convert.ToInt32(Session["Level_Id"]);
                return View(db.tbl_Exam.Where(a => a.tbl_Subject.Department_Id == DeptId && a.tbl_Subject.Level_Id == LevelId).ToList());
            }
        }

        // عرض الامتحان
        public ActionResult StartExam(int? id)
        {
            if (Session["Studnet_Id"] == null || Session["Studnet_Name"] == null)
            {
                return RedirectToAction("StudentLogin", "Home");
            }
            else
            {
                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                else
                {
                    int IdStu = Convert.ToInt32(Session["Studnet_Id"]);

                    if (db.tbl_Result.Where(a => a.Student_Id == IdStu && a.Exam_Id == id).FirstOrDefault() != null)
                    {
                        return RedirectToAction("ExamResult");
                    }
                    else
                    {
                        List<tbl_QuestionMCQ> Ques_Mcq = db.tbl_QuestionMCQ.Where(a => a.Exam_Id == id).ToList();
                        List<tbl_QuestionTrueFalse> Ques_TOF = db.tbl_QuestionTrueFalse.Where(a => a.Exam_Id == id).ToList();

                        int DeptId = Convert.ToInt32(Session["Department_Id"]);
                        int LevelId = Convert.ToInt32(Session["Level_Id"]);
                        var Exam = db.tbl_Exam.SingleOrDefault(a => a.Exam_Id == id && a.tbl_Subject.Department_Id == DeptId && a.tbl_Subject.Level_Id == LevelId);

                        ViewBag.Name = Exam.Exam_Name.ToString();
                        ViewBag.By = Exam.tbl_Teacher.Teacher_Name.ToString();
                        ViewBag.Degre = Exam.Exam_TotalMark.ToString();
                        ViewBag.Mint = Exam.Exam_MuniteDuration.ToString();

                        ViewBag.Time = Convert.ToInt32(Exam.Exam_MuniteDuration) * 60; 
                        ExamModel examModel = new ExamModel();
                        examModel.MCQ = Ques_Mcq;
                        examModel.TrueFalse = Ques_TOF;

                        return View(examModel);
                    }
                   
                }
            }
        }
    }
}