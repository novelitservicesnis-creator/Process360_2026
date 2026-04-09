using System;
using System.Collections.Generic;
using System.Text;

namespace Process360.Repository.ViewModel
{
    public class ApiResponse<T>
    {
        public bool Success { get; set; }
        public string Message { get; set; } = string.Empty;
        public T? Data { get; set; }
        public List<ApiError> Errors { get; set; } = new();
        public DateTime Timestamp { get; set; }
    }
}
