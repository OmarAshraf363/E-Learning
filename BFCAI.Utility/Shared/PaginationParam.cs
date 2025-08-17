using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BFCAI.Utility.Shared
{
    public class PaginationParam
    {
        public int Page { get; set; } = 1;

        // Default page size can be changed globally here
        public int PageSize { get; set; } = 3;

        // Optional: Max page size enforcement
        private const int MaxPageSize = 20;

        public int ValidatedPageSize => PageSize > MaxPageSize ? MaxPageSize : PageSize;
    }
}
