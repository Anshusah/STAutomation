using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Cicero.Data.Entities.SimpleTransfer
{
    public class SmsCodeCustomerRegistraion
    {
        [Key]
        public int Id { get; set; }
        public int TenantId { get; set; }
        public string MobileNumber { get; set; }
        public string Email { get; set; }       
        public bool SmsSuccess { get; set; }
        public bool CustomerSuccess { get; set; }
        public int RetryCount { get; set; }
        public int SmsCode { get; set; }

        public bool Status { get; set; }
        public int ExpiryMinute { get; set; }
        [Column(TypeName = "datetime2(3)")]
        public DateTime RequestedDateTime { get; set; } = DateTime.Now;
        [Column(TypeName = "datetime2(3)")]
        public DateTime ExpiryDateTime { get; set; }      

    }
    public class SmsLog
    {
        [Key]
        public int Id { get; set; }
        public int TenantId { get; set; }
        public string ErrorCode { get; set; }
        public string StatusMessage { get; set; }
        public string MobileNumber { get; set; }
        public string Email { get; set; }
        public string Message { get; set; }
        public string ResponseMessage { get; set; }
        public string CreatedBy { get; set; }
        [Column(TypeName = "datetime2(3)")]
        public DateTime CreatedDate { get; set; } = DateTime.Now;
    }
    public class SmsCodeCustomerRegistraionLog
    {
        [Key]
        public int Id { get; set; }
        public int TenantId { get; set; }
        public string MobileNumber { get; set; }
        public int SmsCode { get; set; }
        [Column(TypeName = "datetime2(3)")]
        public DateTime CreatedDate { get; set; } = DateTime.Now;
        public bool SmsSuccess { get; set; }
    }
}
