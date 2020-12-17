using System;
using System.Collections.Generic;
using System.Text;

namespace Cicero.Data.Entities.SimpleTransfer
{
    public class LexisNexis
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        public string LexisNexisId { get; set; }
        public string Ikey { get; set; }
        public string EquifaxUsername { get; set; }
        public string Reference { get; set; }
        public string ScoreCard { get; set; }
        public string ResultText { get; set; }
        public string ProfileUrl { get; set; }
        public int Credits { get; set; }
        public string UKLexIdField { get; set; }
        public bool SanctionMatch { get; set; }
        public bool PepMatch { get; set; }
        public DateTime CreatedDate { get; set; }
        public string CreatedBy { get; set; }
        public DateTime UpdatedDate { get; set; }
        public string UpdatedBy { get; set; }
    }
}
