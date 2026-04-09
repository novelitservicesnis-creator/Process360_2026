using System;
using System.Collections.Generic;
using System.Text;

namespace Process360.Repository.ViewModel
{
    public class ApiError
    {
        public string? Field { get; set; }
        public string Message { get; set; } = string.Empty;
        public string? Code { get; set; }
    }
}
