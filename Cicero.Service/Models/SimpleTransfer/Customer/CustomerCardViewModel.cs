using Cicero.Service.Models.User;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Net.Http;
using System.Text;

namespace Cicero.Service.Models.SimpleTransfer.Customer
{
    public class CustomerCardDetailViewModel
    {

        [Key]
        public string Id { get; set; }
        public string UserId { get; set; }
        public string CustomerId { get; set; }
        public string Type { get; set; }
        public string Number { get; set; }
        public string ExpDate { get; set; }
        public bool Csv { get; set; }
        public string CreatedAt { get; set; }
        public string UpdatedAt { get; set; }
        public int TenantId { get; set; }
    }
}
