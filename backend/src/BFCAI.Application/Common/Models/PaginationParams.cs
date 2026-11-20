namespace BFCAI.Application.Common.Models;

/// <summary>Same defaults as the legacy PaginationParam: page 1, size 3, max 20.</summary>
public class PaginationParams
{
    public const int MaxPageSize = 20;

    private int _page = 1;
    private int _pageSize = 3;

    public int Page { get => _page; set => _page = value < 1 ? 1 : value; }
    public int PageSize { get => _pageSize; set => _pageSize = value < 1 ? 3 : Math.Min(value, MaxPageSize); }
}
