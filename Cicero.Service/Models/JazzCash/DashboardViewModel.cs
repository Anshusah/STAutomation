using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace Cicero.Service.Models.JazzCash
{
    public class DashboardViewModel
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string FullName { get; set; }
        public string CustomerId { get; set; }
        public int NotificationCount { get; set; }
        public string LocalTime { get; set; }
        public string LastVisit { get; set; }
        public string Balance { get; set; }
        public string Currency { get; set; }
        public List<PaymentDetails> PaymentDetails { get; set; }
        public List<PaymentDetails> RemittanceDetails { get; set; }
    }

    public class PaymentDetails
    {
        public int Id { get; set; }
        public string Date { get; set; }
        public string Description { get; set; }
        public decimal Amount { get; set; }
        public string Currency { get; set; }
        public string Status { get; set; }
        public string ClassName { get; set; }
        public int Type { get; set; }
    }

    public enum PaymentReuqestType
    {
        [Description("Request")]
        Request = 1,
        [Description("Payment")]
        Payment = 2
    }

    public enum PaymentStatus
    {
        [Description("Payment Pending")]
        PaymentPending = 1,
        [Description("Payment InProgress")]
        PaymentInProgress = 2,
        [Description("Payment Received")]
        PaymentReceived = 3
    }

    public enum PaymentStatusClassName
    {
        [Description("info")]
        Info = 1,
        [Description("warning")]
        Warning = 2,
        [Description("success")]
        Success = 3
    }
}
