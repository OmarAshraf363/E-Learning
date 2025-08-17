using Banha_UniverCity.Data;
using BFCAI.Models;
using BFCAI.Models.ViewModels;
using BFCAI.Utility.Helper;
using DataAccess.Repository.IRepository;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Repository.ModelsRepository
{
    public class TrackRepository : GenralRepository<Track>, ITrackRepository
    {
        private readonly ApplicationDbContext _context;
        public TrackRepository(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<IReadOnlyList<TrackDetailsVIewModel>> GetTopTracksAsync()
        {
            var topEnrollments = await _context.Track
                .Select(e => new TrackDetailsVIewModel
                {
                    TrackId = e.Id,
                    Title = e.Name,
                    Description = e.Description,
                    Cover = e.Cover,
                    NumOfCourses = e.Courses.Count,
                    NUMOfEnrollments = e.Courses.SelectMany(c => c.Enrollments).Count(),
                })
                .OrderByDescending(e => e.NUMOfEnrollments)
                .Take(3)
                .ToListAsync();
            return topEnrollments;

        }

        public async Task<TrackDetailsVIewModel> TrackDetailsProjection(int trackId,string? studentId=null)
        {
            var track = await _context.Track.Where(e => e.Id == trackId)
                .Select(e => new TrackDetailsVIewModel
                {
                    Title = e.Name,
                    Description = e.Description,
                    Cover = e.Cover,
                    NumOfCourses = e.Courses.Count,
                    NUMOfEnrollments = e.Courses.SelectMany(c => c.Enrollments).Count(),
                   
                  
                }).FirstOrDefaultAsync();
            if (track == null)
                return new TrackDetailsVIewModel();
            if (!string.IsNullOrEmpty(studentId))
            {
            track.CoursesThatLoginStudentNotHave= await _context.Courses
                .Where(e => e.TrackID == trackId && !e.Enrollments.Any(en => en.StudentId == studentId))
                .PrepareCoursesToViewDetailAsync().ToListAsync();
            }
            track.Courses = _context.Courses.Where(e => e.TrackID == trackId).PrepareCoursesToViewDetailAsync().ToList();
            return track;
        }
        
    }
}
