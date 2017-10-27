using Survey.Business.Services.Contracts;
using Survey.Data.DataAccess.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Survey.Business.Entities.Survey;
using Survey.Core.Contracts;
using Survey.Data.DataAccess.Repositories;
using AutoMapper;

namespace Survey.Business.Services.Implementation
{
    public class QuestionBankService : BaseService, IQuestionBankService
    {
        public QuestionBankService(IUnitOfWork unitOfWork, IExceptionReporter exceptionReporter) : base(unitOfWork, exceptionReporter)
        {
        }

        public async Task<IEnumerable<QuestionbankDto>> GetQuestion()
        {
           

            return Mapper.Map<IEnumerable<QuestionbankDto>>(await UnitOfWork.QuestionbankRepository.GetAllAsync());
        }


        public async Task<bool> AddQuetion(QuestionbankDto newObj)
        {


            QuestionBank que = new QuestionBank();
            que.QueText = newObj.QueText;
            que.OptionType = newObj.OptionType;
             UnitOfWork.QuestionbankRepository.Add(que);
            await SaveAsync();
            return true;
            
        }
    }
}
