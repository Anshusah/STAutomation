using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Cicero.Service.Models
{
    public class JsonQueueViewModel
    {

        public FirstQueue First { get; set; }

        public bool IsActive { get; set; } = true;

        public LastState Last { get; set; }

        public int CaseFormId { get; set; }

    }

    public class FirstQueue
    {
        public int Queue { get; set; }

        public string QueueXPos { get; set; }

        public string QueueYPos { get; set; }

        public string LineXPos { get; set; }

        public string LineYPos { get; set; }

        public bool IsLock { get; set; } = true;

        //public bool IsQueue { get; set; }
    }

    public class LastState
    {
        public bool IsLock { get; set; } = true;

        public string StateXPos { get; set; }

        public string StateYPos { get; set; }

        public string LineXPos { get; set; }

        public string LineYPos { get; set; }

        public int State { get; set; }
    }

}
