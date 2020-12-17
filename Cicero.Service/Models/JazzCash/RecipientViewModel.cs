using System;
using System.Collections.Generic;
using System.Text;

namespace Cicero.Service.Models.JazzCash
{
    public class RecipientViewModel
    {
        public int Id { get; set; }

        public string UserId { get; set; }

        public string FirstName { get; set; }

        public string MiddleName { get; set; }

        public string LastName { get; set; }

        public string FullName { get; set; }

        public string ShortName { get; set; }

        public string AddressLine1 { get; set; }

        public string AddressLine2 { get; set; }

        public string Suburb { get; set; }

        public string PostalCode { get; set; }

        public int CityId { get; set; }

        public int CountryId { get; set; }

        public string EmailAddress { get; set; }

        public string MobileNo { get; set; }

        public string PhoneNo { get; set; }

        public int RelationshipToBeneId { get; set; }

        public int Gender { get; set; }

        public string Remark { get; set; }

        public bool Status { get; set; }

        public string CreatedDate { get; set; }

        public string UpdatedDate { get; set; }

        public string CreatedBy { get; set; }

        public string UpdatedBy { get; set; }
    }
}
