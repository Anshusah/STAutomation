using System;
using System.Collections.Generic;
using System.Text;

namespace Cicero.Service.Models.API.LexisNexis
{
    public class SanctionPepPersonViewModel
    {
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

        public string CreatedDate { get; set; }
        public string CreatedBy { get; set; }
        public string UpdatedDate { get; set; }
        public string UpdatedBy { get; set; }
    }
}
