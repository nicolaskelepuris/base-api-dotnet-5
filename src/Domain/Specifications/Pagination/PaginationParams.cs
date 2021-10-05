using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Domain.Specifications.Pagination
{
    public class PaginationParams
    {
        private const int MaxPageSize = 30;
        private const int MinPageSize = 1;
        public int PageIndex { get; set; } = 1;
        private int _pageSize = 10;
        public int PageSize
        {
            get { return _pageSize; }
            set
            {
                _pageSize = Math.Min(MaxPageSize, Math.Max(MinPageSize, value));
            }
        }        
    }
}