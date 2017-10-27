namespace Survey.Business.Services.Contracts
{
    using Entities;
    using Entities.Survey;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public interface ISurveyService
    {
        Task<IEnumerable<SurveyDto>> GetSurvey();
    }
}
