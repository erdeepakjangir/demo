using Survey.Business.Entities.Survey;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Survey.Business.Services.Contracts
{
    public interface IQuestionBankService
    {
        Task<IEnumerable<QuestionbankDto>> GetQuestion();
        Task<bool> AddQuetion(QuestionbankDto newObj);
    }
}
