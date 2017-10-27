namespace Survey.Business.Services.ServiceUtilities
{
    using AutoMapper;
    using Entities;
    using Entities.Proxy;
    using Entities.Survey;

    public class AutoMapperServiceProfile : Profile
    {
        public AutoMapperServiceProfile()
        {
            CreateMap<Survey.Data.DataAccess.Entities.SurveyMaster, SurveyDto>().ReverseMap();
            CreateMap<Survey.Data.DataAccess.Entities.QuestionBank, QuestionbankDto>().ReverseMap();

            CreateMap<CUStudentDetail, UserDto>()
            .ForMember(dto => dto.StudentId, ent => ent.MapFrom(s => s.StudentId))

            .ForMember(dto => dto.StageId, ent => ent.MapFrom(s => s.CourseStage))
            // .ForMember(dto => dto.FacultyCode, ent => ent.MapFrom(s => s.Faculty))
            .ForMember(dto => dto.Mode, ent => ent.MapFrom(s => s.ModeOfStudy))
            .ForMember(dto => dto.QualAim, ent => ent.MapFrom(s => s.QualificationAim))
            .ForMember(dto => dto.FullName, ent => ent.MapFrom(s => s.Forename + " " + s.Surname))
            .ReverseMap();

            CreateMap<CUQualificationType, QualificationDataDto>()
            .ForMember(dto => dto.Code, ent => ent.MapFrom(s => s.QualificationTypeCode))
            .ForMember(dto => dto.Title, ent => ent.MapFrom(s => s.Description));

            CreateMap<CUSubject, SubjectDto>()
            .ForMember(dto => dto.SubjectTitle, ent => ent.MapFrom(s => s.HQualSubjectDescription));

            CreateMap<CUResult, ResultDto>();

            CreateMap<CUFaculty, FacultyDto>()
            .ForMember(dto => dto.FacultyName, ent => ent.MapFrom(s => s.Description));


            CreateMap<Qualification, QualificationDto>()
           .ForMember(dto => dto.QualificationTitle, ent => ent.MapFrom(s => s.QualificationTitle))
           .ForMember(dto => dto.SubjectTitle, ent => ent.MapFrom(s => s.SubjectName))
           .ForMember(dto => dto.SittingTitle, ent => ent.MapFrom(s => s.SittingName))
           .ForMember(dto => dto.SubmittedDate, ent => ent.MapFrom(s => s.CreatedDate))
           .ForMember(dto => dto.Result, ent => ent.MapFrom(s => s.Result));

            CreateMap<CUStudentQualification, QualificationDto>()
            .ForMember(dto => dto.QualificationTitle, ent => ent.MapFrom(s => s.Qualification))
            .ForMember(dto => dto.SubjectTitle, ent => ent.MapFrom(s => s.Subject))
            .ForMember(dto => dto.SittingTitle, ent => ent.MapFrom(s => s.Sitting))
             .ForMember(dto => dto.Result, ent => ent.MapFrom(s => s.Result));
            // .ForMember(dto => dto.SubmittedDate, ent => ent.MapFrom(s => s.CreatedDate));


            CreateMap<QualificationDto, Qualification>()
            .ForMember(dto => dto.QualificationTitle, ent => ent.MapFrom(s => s.QualificationTitle))
            .ForMember(dto => dto.SubjectName, ent => ent.MapFrom(s => s.SubjectTitle))
            .ForMember(dto => dto.SittingName, ent => ent.MapFrom(s => s.SittingTitle))
            .ForMember(dto => dto.Result, ent => ent.MapFrom(s => s.Result))
            .ForMember(dto => dto.CreatedDate, ent => ent.MapFrom(s => s.SubmittedDate));

            CreateMap<QualificationDto, CUStudentQualification>();



        }
    }
}