using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SystemExamWebSite.DataBase;

namespace SystemExamWebSite.Models
{
    public class QustionAll
    {
        public List<tbl_QuestionTrueFalse> TF { get; set; }
        public List<tbl_QuestionMCQ> Mcq { get; set; }
    }
}