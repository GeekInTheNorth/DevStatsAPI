using System;
using System.Collections.Generic;
using DevStats.Domain.Logging;

namespace DevStats.Models.Audit
{
    public class ApiAuditModel
    {
        public DateTime FilterFrom { get; set; }

        public DateTime FilterTo { get; set; }

        public List<ApiLog> AuditItems { get; set; }
    }
}