using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace SystemExamWebSite.Models
{
    public class LoginStudent
    {
        [Required(AllowEmptyStrings = false, ErrorMessage = "ادخل البريد الإلكتروني من فضلك")]
        [EmailAddress(ErrorMessage = "ادخل البريد الإلكتروني بطريقة صحيحة")]
        public string Student_Email { get; set; }
        [Required(AllowEmptyStrings = false, ErrorMessage = "ادخل كلمة المرور من فضلك")]
        [DataType(DataType.Password)]
        public string Password { get; set; }
    }
}