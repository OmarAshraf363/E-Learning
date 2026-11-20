namespace BFCAI.Application.Common;

/// <summary>Redis keys used by the public home page (legacy HomeController.Index), 30 minutes TTL.</summary>
public static class CacheKeys
{
    public const string TopCourses = "top-courses";
    public const string TopDepartments = "top-departments";
    public const string TopTracks = "top-tracks";

    public static readonly TimeSpan HomeTtl = TimeSpan.FromMinutes(30);

    /// <summary>Everything that depends on courses / departments / tracks.</summary>
    public static readonly string[] Catalog = [TopCourses, TopDepartments, TopTracks];
}
