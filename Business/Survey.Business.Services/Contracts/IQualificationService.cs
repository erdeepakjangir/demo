using Survey.Business.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Survey.Business.Services.Contracts
{
    public interface IQualificationService
    {         

        #region Public Methods

        Task<List<int>> GetYears();


        Task<QualificationDataListDto> GetQualifications();

        Task<List<SubjectDto>> GetSubjects(string qualificationCode);

        Task<List<ResultDto>> GetResults(string qualificationCode);

        Task<List<SittingDto>> GetSittings();

        Task<List<FacultyDto>> GetFaculties();

        Task<string[]> GetNamesByCode(string qualificationCode, string subjectCode, string result,char sittingCode);

        Task<string> GetFacultyCode(string facultyName);
        #endregion
    }
}
