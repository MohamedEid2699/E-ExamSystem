using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using SystemExamWebSite.DataBase;

namespace SystemExamWebSite.Models
{
    public class RegisterStudent
    {
        [Key]
        public int Student_Id { get; set; }
        [Required(AllowEmptyStrings = false, ErrorMessage = "ادخل اسم الطالب من فضلك")]
        [MinLength(5, ErrorMessage = "الاسم قصير للغاية")]
        public string Student_Name { get; set; }
        [Required(AllowEmptyStrings = false, ErrorMessage = "ادخل البريد الإلكتروني من فضلك")]
        [EmailAddress(ErrorMessage = "ادخل البريد الإلكتروني بطريقة صحيحة")]
        public string Student_Email { get; set; }
        [Required(AllowEmptyStrings = false, ErrorMessage = "ادخل كلمة المرور من فضلك")]
        [RegularExpression(@"^(?=.*\d)(?=.*[a-z])(?=.*[A-Z])(?=.*[^\da-zA-Z])(.{8,30})$", ErrorMessage = "كلمة السر ضعيفة ، ادخل كلمة سر قوية")]
        [DataType(DataType.Password)]
        public string Password { get; set; }
        [DataType(DataType.Password)]
        [System.ComponentModel.DataAnnotations.Compare("Password", ErrorMessage = "كلمة السر غير متطابقة")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "ادخل كلمة المرور من فضلك")]
        public string ConfirmPassword { get; set; }
        [Required(AllowEmptyStrings = false, ErrorMessage = "ادخل رقم التليفون من فضلك")]
        [RegularExpression(@"^[0-9]{11,11}$", ErrorMessage = "أرقام فقط")]
        [Phone (ErrorMessage = "أرقام فقط")]
        public string Student_Phone { get; set; }
        public DateTime Student_CreatedOn { get; set; }
        [Required(AllowEmptyStrings = false, ErrorMessage = "اختر المستوى من فضلك")]
        public int Level_Id { get; set; }
        [Required(AllowEmptyStrings = false, ErrorMessage = "اختر القسم من فضلك")]
        public int Department_ID { get; set; }
        public bool IsDeleted { get; set; }

        public virtual IEnumerable<tbl_Level> tbl_Level { get; set; }
        public virtual IEnumerable<tbl_Department> tbl_Department { get; set; }
        //public IEnumerable<SelectListItem> LevelsItems { get; set; }
        //public IEnumerable<SelectListItem> Deartmentitems { get; set; }
    }
}