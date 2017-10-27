using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Survey.Business.Entities
{
    public class StudentSummaryDto
    {
        public string FacultyCode { get; set; }
        public string StudentName { get; set; }
        public long Total { get; set; }
        public long Approved { get; set; }
        public long Rejected { get; set; }
        public long Pending { get; set; }
    }
}
