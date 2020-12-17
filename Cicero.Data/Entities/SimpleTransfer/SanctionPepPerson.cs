using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Cicero.Data.Entities.SimpleTransfer
{
    public class SanctionPepPerson
    {
        [Key]
        public int Id { get; set; }
        public int LexisNexisId { get; set; }

        public string NameField { get; set; }

        public string RecencyField { get; set; }

        public int MatchScoreField { get; set; }

        public string SourceField { get; set; }

        public string TypeField { get; set; }

        public string CountryField { get; set; }

        public string AddressesField { get; set; }

        public string AliasesField { get; set; }

        public string ExceptionsField { get; set; }

        public string PositionsField { get; set; }

        public string DOBField { get; set; }

        public DateTime CreatedDate { get; set; }
        public string CreatedBy { get; set; }
        public DateTime UpdatedDate { get; set; }
        public string UpdatedBy { get; set; }
    }
}
