using BFCAI.Application.Common;
using BFCAI.Application.Common.Exceptions;
using BFCAI.Application.Common.Interfaces;
using BFCAI.Application.Common.Models;
using BFCAI.Domain.Entities;
using FluentValidation;

namespace BFCAI.Application.Features.Tracks;

public class TrackRequestValidator : AbstractValidator<TrackRequest>
{
    public TrackRequestValidator()
    {
        RuleFor(x => x.Name).NotEmpty();
        RuleFor(x => x.Description).NotEmpty();
        RuleFor(x => x.Cover).MustBeImage();
    }
}

/// <summary>Legacy Admin/TrackController.</summary>
public interface ITrackService
{
    Task<List<TrackListItemDto>> GetAllAsync();
    Task<TrackDto> GetAsync(int id);
    Task<TrackDetailsDto> GetDetailsAsync(int id);
    Task<TrackDto> CreateAsync(TrackRequest request);
    Task<TrackDto> UpdateAsync(int id, TrackRequest request);
    Task DeleteAsync(int id);
}

public class TrackService(IUnitOfWork unitOfWork, IFileStorage files, ICacheService cache) : ITrackService
{
    private IGenericRepository<Track> Tracks => unitOfWork.Repository<Track>();

    public Task<List<TrackListItemDto>> GetAllAsync()
        => Tracks.SelectAsync(t => new TrackListItemDto(t.Id, t.Name, t.Description, FilePaths.Cover(t.Cover), t.Courses.Count));

    public async Task<TrackDto> GetAsync(int id)
        => await Tracks.SelectOneAsync(t => t.Id == id, t => new TrackDto(t.Id, t.Name, t.Description, FilePaths.Cover(t.Cover)))
           ?? throw new NotFoundException(nameof(Track), id);

    public async Task<TrackDetailsDto> GetDetailsAsync(int id)
        => await Tracks.SelectOneAsync(t => t.Id == id, t => new TrackDetailsDto(
               t.Id, t.Name, t.Description, FilePaths.Cover(t.Cover),
               t.Courses.Select(c => new TrackCourseDto(c.CourseID, c.CourseName, FilePaths.Cover(c.ImgCover),
                   c.Instructor != null ? c.Instructor.FullName : null, c.Price, c.Description)).ToList()))
           ?? throw new NotFoundException(nameof(Track), id);

    public async Task<TrackDto> CreateAsync(TrackRequest request)
    {
        if (request.Cover is null) Validation.Fail(nameof(request.Cover), "Please upload an image.");

        var track = new Track
        {
            Name = request.Name,
            Description = request.Description,
            Cover = await SaveCoverAsync(request.Cover!),
        };
        await Tracks.AddAsync(track);
        await unitOfWork.SaveChangesAsync();
        await cache.RemoveAsync(CacheKeys.Catalog);
        return await GetAsync(track.Id);
    }

    public async Task<TrackDto> UpdateAsync(int id, TrackRequest request)
    {
        var track = await Tracks.GetOneAsync(t => t.Id == id, tracked: true) ?? throw new NotFoundException(nameof(Track), id);

        track.Name = request.Name;
        track.Description = request.Description;
        if (request.Cover is not null)
        {
            var oldCover = track.Cover;
            track.Cover = await SaveCoverAsync(request.Cover);
            await files.DeleteAsync($"{FilePaths.CoversFolder}/{oldCover}");
        }

        await unitOfWork.SaveChangesAsync();
        await cache.RemoveAsync(CacheKeys.Catalog);
        return await GetAsync(id);
    }

    public async Task DeleteAsync(int id)
    {
        var track = await Tracks.GetOneAsync(t => t.Id == id, tracked: true) ?? throw new NotFoundException(nameof(Track), id);
        Tracks.Delete(track);
        await unitOfWork.SaveChangesAsync();
        await cache.RemoveAsync(CacheKeys.Catalog);
    }

    private Task<string> SaveCoverAsync(FileUpload cover)
        => files.SaveAsync(cover, FilePaths.CoversFolder, FilePaths.NewCoverName(cover.FileName));
}
