using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Survey.Business.Entities
{
    public class UserDto : BaseDto
    {
        #region Properties
        public long StudentId { get; set; }
        public string FacultyCode { get; set; }
        public string Faculty{ get; set; }
        public string QualAim { get; set; }
        public int StageId { get; set; }
        public string Mode { get; set; }
        public string CourseCode { get; set; }
        public string CourseTitle { get; set; }
        public string FullName { get; set; }
        
        #endregion

    }
}
