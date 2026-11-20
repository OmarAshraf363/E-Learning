using BFCAI.Application.Common.Models;

namespace BFCAI.Application.Features.Tracks;

public record TrackListItemDto(int Id, string Name, string Description, string? Cover, int CoursesCount);

public record TrackDto(int Id, string Name, string Description, string? Cover);

public record TrackCourseDto(int Id, string Name, string? Cover, string? InstructorName, decimal? Price, string Description);

/// <summary>Admin track details (legacy Views/Shared/Details.cshtml, admin branch).</summary>
public record TrackDetailsDto(int Id, string Name, string Description, string? Cover, List<TrackCourseDto> Courses);

/// <summary>Create / edit. Cover is required on create.</summary>
public record TrackRequest(string Name, string Description, FileUpload? Cover);
