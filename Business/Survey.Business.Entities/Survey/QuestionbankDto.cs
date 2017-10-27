using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Survey.Business.Entities.Survey
{
    public class QuestionbankDto: BaseDto
    {
        public string QueText { get; set; }
        public Nullable<int> OptionType { get; set; }
    }
}
