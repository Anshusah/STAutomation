using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Cicero.Service.Models
{
    public class WorkFlowViewModel
    {

        public List<SelectListItem> CaseFormList { get; set; }

        public int CaseFormId { get; set; }

        public List<StateViewModel> StateList { get; set; }

        public List<QueueViewModel> QueueList { get; set; }

        public List<SelectListItem> RoleList { get; set; }

        public List<ActionsViewModel> ActionList { get; set; }
    }

}
