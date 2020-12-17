using Cicero.Data.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Cicero.Service.Models
{  
    public class CaseStateHistoryViewModel
    {
        public int Id { get; set; }

        public string UserId { get; set; }

        public int CaseId { get; set; }

        public DateTime UpdatedAt { get; set; }

        public string UpdatedBy { get; set; }

        public int? PreviousStateId { get; set; }

        public int? CurrentStateId { get; set; }

        public string Reason { get; set; }

        public string PreviousStateName { get; set; }

        public string CurrentStateName { get; set; }

        public string UpdatedByImg { get; set; }

        public CaseViewModel Case { get; set; }

        public UserViewModel User { get; set; }

        public StateViewModel PreviousState { get; set; }

        public StateViewModel CurrentState { get; set; }
    }    
}