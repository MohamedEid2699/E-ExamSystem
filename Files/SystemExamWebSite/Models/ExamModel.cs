using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SystemExamWebSite.DataBase;

namespace SystemExamWebSite.Models
{
    public class ExamModel
    {
        public List<tbl_QuestionMCQ> MCQ { get; set; }
        public List<tbl_QuestionTrueFalse> TrueFalse { get; set; }
    }
}