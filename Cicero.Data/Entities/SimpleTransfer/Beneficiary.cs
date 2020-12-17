using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Cicero.Data.Entities.SimpleTransfer
{
    public class Beneficiary
    {   
        [Key]
        public int Id { get; set; }
        public string UserId { get; set; }
        [Column(TypeName = "varchar(200)")]

        public string FirstName { get; set; }
        [Column(TypeName = "varchar(200)")]

        public string MiddleName { get; set; }
        [Column(TypeName = "varchar(200)")]

        public string LastName { get; set; }
        [Column(TypeName = "varchar(200)")]

        public string ShortName { get; set; }
        [Column(TypeName = "varchar(500)")]

        public string AddressLine1 { get; set; }
        [Column(TypeName = "varchar(500)")]

        public string AddressLine2 { get; set; }
        [Column(TypeName = "varchar(200)")]

        public string Suburb { get; set; }
        [Column(TypeName = "varchar(10)")]

        public string PostalCode { get; set; }
        public int CityId { get; set; }
        public int CountryId { get; set; }
        [Column(TypeName = "varchar(200)")]

        public string EmailAddress { get; set; }
        [Column(TypeName = "varchar(20)")]

        public string MobileNo { get; set; }
        [Column(TypeName = "varchar(20)")]

        public string PhoneNo { get; set; }
        public int RelationshipToBeneId { get; set; }
        public int Gender { get; set; }
        [Column(TypeName = "varchar(500)")]

        public string Remark { get; set; }
        public bool Status { get; set; }
        [Column(TypeName = "datetime2(3)")]
        public DateTime UpdatedDate { get; set; } = DateTime.Now;
        [StringLength(450)]
        public string CreatedBy { get; set; }
        [Column(TypeName = "datetime2(3)")]
        public DateTime CreatedDate { get; set; }
        [StringLength(450)]
        public string UpdatedBy { get; set; }

    }  
}
