using BFCAI.Application.Common.Models;
using Microsoft.EntityFrameworkCore;

namespace BFCAI.Infrastructure.Persistence.Repositories;

public static class QueryableExtensions
{
    /// <summary>Same behavior as the legacy ToPagedListAsync helper.</summary>
    public static async Task<PagedResult<T>> ToPagedResultAsync<T>(this IQueryable<T> query, PaginationParams pagination)
    {
        var total = await query.CountAsync();
        var items = await query
            .Skip((pagination.Page - 1) * pagination.PageSize)
            .Take(pagination.PageSize)
            .ToListAsync();

        return new PagedResult<T>
        {
            Items = items,
            Page = pagination.Page,
            PageSize = pagination.PageSize,
            TotalCount = total,
        };
    }
}
