namespace Survey.Web.WebUtilities
{
    using AutoMapper;
    using Business.Entities;
    using Business.Entities.Survey;
    using ViewModel;

    public class AutoMapperViewModelProfile : Profile
    {
        public AutoMapperViewModelProfile()
        {

            CreateMap<SurveyDto, SurveyViewModel>().ReverseMap();
            CreateMap<QuestionbankDto, QuestionBankViewModel>().ReverseMap();
        
        }
    }
}