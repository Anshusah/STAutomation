using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Cicero.Service.Helpers
{
    public class GenericResponse
    {
        [DataMember]
        public bool IsSuccess { get; set; }
    }
}
