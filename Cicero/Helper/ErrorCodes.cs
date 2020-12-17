using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Cicero.Helper
{
    public class ErrorResponse
    {
        public ErrorResponse() { }

        public ErrorResponse(ValidationFailureError error)
        {
            Errors.Add(error);
        }

        public List<ValidationFailureError> Errors { get; set; } = new List<ValidationFailureError>();


    }
    [DataContract]
    public class Error
    {
        [DataMember]
        public string ErrorDescription { get; set; }

        [DataMember]
        public string Message { get; set; }
    }

    [DataContract]
    public class ValidationFailureError: GenericResponse
    {
        [DataMember]
        public int ErrorCode { get; set; }

        [DataMember]
        public string ErrorDescription { get; set; }

        [DataMember]
        public string Message { get; set; }
    }

    public class ValidationFailureErrorList
    {
        private List<ValidationFailureError> errorList;

        public List<ValidationFailureError> ErrorList
        {
            get
            {
                if (errorList == null)
                {
                    errorList = new List<ValidationFailureError>();
                }
                return errorList;
            }
            set { errorList = value; }
        }

    }
}
