namespace Survey.Business.Services.Implementation
{
    using System.Threading.Tasks;
    using Contracts;
    using Core.Contracts;

    using Data.DataAccess.Repositories;
    using System.Collections.Generic;
    using AutoMapper;
    using Entities.Survey;

    public class SurveyService : BaseService, ISurveyService
    {
        public SurveyService(IUnitOfWork unitOfWork, IExceptionReporter exceptionReporter)
              : base(unitOfWork, exceptionReporter)
        {
        }

        public async Task<IEnumerable<SurveyDto>> GetSurvey()
        {
            return Mapper.Map<IEnumerable<SurveyDto>>(await UnitOfWork.SurveyRepository.GetAllAsync());

        }
    }
}
