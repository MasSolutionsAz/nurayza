using System;
using System.Collections.Generic;

namespace ILoveBaku.Domain.Entities
{
    public partial class ErrorLogs
    {
        public int Id { get; set; }
        public string LogText { get; set; }
        public string Url { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime? CreatedIp { get; set; }
    }
}
