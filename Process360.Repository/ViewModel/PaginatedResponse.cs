using System;
using System.Collections.Generic;
using System.Text;

namespace Process360.Repository.ViewModel
{
    public class PaginatedResponse<T> where T : class
    {
        public List<T> Items { get; set; } = new();
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public int TotalItems { get; set; }
        public int TotalPages { get; set; }
    }
}
