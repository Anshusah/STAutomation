using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Cicero.Service.Models
{
    public class QueueListViewModel
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public int Count { get; set; }

        public int OrderPosition { get; set; }
    }
}
