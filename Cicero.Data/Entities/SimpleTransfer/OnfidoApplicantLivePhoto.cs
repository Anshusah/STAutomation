using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Cicero.Data.Entities.SimpleTransfer
{
    public class OnfidoApplicantLivePhoto
    {
        [Key]
        [StringLength(450)]
        public string id { get; set; }

        public string PhotoId { get; set; }

        public DateTime created_at { get; set; } = DateTime.Now;

        public DateTime? updated_at { get; set; }

        public string file_name { get; set; }

        public string file_type { get; set; }

        public string Url { get; set; }

        public string applicant_id { get; set; }

        [StringLength(450)]
        public string CreatedBy { get; set; }

        [StringLength(450)]
        public string UpdatedBy { get; set; }
    }
}
