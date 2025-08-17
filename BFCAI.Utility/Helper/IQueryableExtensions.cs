using BFCAI.Models.ViewModels;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BFCAI.Utility.Helper
{
    public static class IQueryableExtensions
    {
        public static async Task<PagedListViewModel<T>> ToPagedListAsync<T>(this IQueryable<T> source, int pageNumber, int pageSize)
        {
            var totalCount = await source.CountAsync();
            var totalPages = (int)Math.Ceiling(totalCount / (double)pageSize);
            var items = await source.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToListAsync();
            return new PagedListViewModel<T>
            {
                Items = items,
                PageNumber = pageNumber,
                TotalPages = totalPages,
                PageSize = pageSize,
                TotalCount = totalCount
            };
        }



    }
}
