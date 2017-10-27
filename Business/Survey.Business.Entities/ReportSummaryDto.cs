using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Survey.Business.Entities
{
    public class ReportSummaryDto
    {
        public long Total { get; set; }
        public long Approved { get; set; }
        public long Rejected { get; set; }
        public long Pending { get; set; }


        public List<FacultySummaryDto> FacultySummary;
        public List<QualificationSummaryDto> QualificationSummary;
        public List<UserSummaryDto> UserSummary;

    }
}
