using System;
using System.Collections.Generic;


namespace Survey.Business.Entities
{
    public class QualificationDataDto
    {
        #region Properties   
        public string Title { get; set; }
        public string Code { get; set; }
        public List<SubjectDto> Subjects;
        public List<ResultDto> Results;
        #endregion
    }
}
