using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace SystemExamWebSite.Models.AdminController
{
    public class AdminMangementModel
    {
        [Key]
        public int Admin_Id { get; set; }
        [Required(AllowEmptyStrings = false, ErrorMessage = "ادخل اسم المسئول من فضلك")]
        [MinLength(5, ErrorMessage = "الاسم قصير للغاية")]
        public string Admin_Name { get; set; }
        [Required(AllowEmptyStrings = false, ErrorMessage = "ادخل البريد الإلكتروني من فضلك")]
        [EmailAddress(ErrorMessage = "ادخل البريد الإلكتروني بطريقة صحيحة")]
        public string Admin_Email { get; set; }
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
        public string Admin_Phone { get; set; }

        public DateTime Admin_CreatedOn { get; set; }
        public bool IsDeleted { get; set; }
    }
}