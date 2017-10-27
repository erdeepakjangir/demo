using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Survey.Business.Entities
{
    public class UserSummaryDto
    {
        public string FacultyCode { get; set; }
        public string UserName { get; set; }
        public long Total { get; set; }
        public long Approved { get; set; }
        public long Rejected { get; set; }
        public long Pending { get; set; }
    }
}
