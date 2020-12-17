using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Cicero.Service.Models
{
    public class CaseViewModel
    {

        public int Id { get; set; }

        public int PaymentRequestId { get; set; }

        public string CaseGeneratedId { get; set; }

        [Display(Name = "Updated Date")]
        public DateTime UpdatedAt { get; set; }

        [Display(Name = "Created Date")]
        public DateTime CreatedAt { get; set; }

        public string UserId { get; set; }

       
        //[Display(Name ="Created By")]
        //public string CreatedBy { get; set; }

        public List<SelectListItem> CaseTasks { get; set; }

        public int StateId { get; set; }

        public string StateName { get; set; }

        public int QueueId { get; set; }

        public string QueueName { get; set; }

        public string QueueIcon { get; set; }

        public string QueueColor { get; set; }

        public bool DisplayPermission { get; set; }

        public int TenantId { get; set; }

        [Display(Name = "Insurance Policy Type")]
        [Required(ErrorMessage = "Please Select a policy type.")]
        public int CaseFormId { get; set; }

        [Display(Name = "Case Type")]
        public string CaseFormName { get; set; }

        public string CaseFormUrl { get; set; }

        public object FormBuilder { get; set; }

        public List<SelectListItem> StateList { get; set; }
        public string UserFullName { get; set; }

        public string AssignedTo { get; set; }
        public DateTime AssignedAt { get; set; }
        public DateTime DueDate { get; set; }
        public UserViewModel CaseOwner { get; set; }

        public List<VisibleInFooterViewModel> VisibleInFooterViewModel { get; set; }
    }

    public class VisibleInFooterViewModel
    {
        public string Text { get; set; }
        public string Type { get; set; }
        public string IconUrl { get; set; }
    }
}