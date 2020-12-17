using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;

namespace Cicero.Service.Models
{
    public class FileUploaderViewModel
    {
        public IFormFile UploadFile { get; set; }
        public string Type { get; set; }
        public int SupplierId { get; set; }
    }
}
