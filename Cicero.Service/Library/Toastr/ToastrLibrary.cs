﻿namespace Cicero.Service.Library.Toastr
{
    public class ToastrLibrary : ILibrary
    {
        public string VarName { get; } = "toastr";
        public string ScriptSrc { get; set; } = "https://cdnjs.cloudflare.com/ajax/libs/toastr.js/latest/js/toastr.min.js";
        public string StyleHref { get; set; } = "https://cdnjs.cloudflare.com/ajax/libs/toastr.js/latest/toastr.min.css";
        public ILibraryOptions Options { get; set; }
    }
}
