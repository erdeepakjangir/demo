using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Survey.Web.ViewModel
{
    public class QuestionBankViewModel
    {
        public string QueText { get; set; }
        public Nullable<int> OptionType { get; set; }
    }
}