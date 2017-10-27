using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Survey.Business.Entities
{
    public class CertificateDto : BaseDto
    {
        #region Properties     
        public string FileName { get; set; }
        public byte[] FileContent{ get; set; }
        public string ContentType { get; set; }
        #endregion
    }
}
