using Banha_UniverCity.Repository.IRepository;
using BFCAI.Models;
using BFCAI.Models.ViewModels;
using DataAccess.Repository.IRepository.Service;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Repository.ModelsRepository.Service
{
    public class PostService : IPostService
    {
        private readonly IUnitOfWork _unitOfWork;

        public PostService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<IReadOnlyList<PostVM>> GetRelatedPostsByKeyWordsAsync(string studentId)
        {
            var studentCourses=await _unitOfWork.enrollmentRepository.GetStudentCourses(studentId);
            var coursesIdes=studentCourses.Select(e=>e.CourseID).ToList();
            var keys=_unitOfWork.keyWordRepository.Get(e=>coursesIdes.Contains((int)e.CourseId)).Select(e=>e.Name).ToList();
            //var words = studentCourses.SelectMany(e => e.Keywords).Select(k => k.Name.ToLower()).Distinct().ToList();
            var posts =  await _unitOfWork.keyWordRepository.Get(e=>keys.Contains(e.Name.ToLower())&&e.PostId!=null).Select(e=>new PostVM
            {
                Content=e.Post.Content,
                Attachment=e.Post.Attachment??null,
                CreatedAt=e.Post.CreatedAt,
                UserName=e.Post.User.UserName,
                UserProfilePic=e.Post.User.Picture,
                CommunityId=e.Post.CommunityId,

            }).ToListAsync();
            return posts;
        }
    }
}
