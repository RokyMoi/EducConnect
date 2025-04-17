using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EduConnect.Data;
using EduConnect.DTOs;
using EduConnect.Entities.Course;
using EduConnect.Enums;
using EduConnect.Interfaces.Course;
using EduConnect.Utilities;
using Microsoft.EntityFrameworkCore;

namespace EduConnect.Repositories.Course
{
    public class CourseRepository(DataContext _dataContext) : ICourseRepository
    {
        public async Task<bool> CheckCoursePromotionImageExists(Guid coursePromotionImageId)
        {
            return await _dataContext.CoursePromotionImage.Where(x => x.CoursePromotionImageId == coursePromotionImageId).AnyAsync();
        }

        public async Task<bool> CheckTagAssignedToCourse(Guid tagId, Guid courseId)
        {
            return await _dataContext.CourseTag
            .Where(x => x.CourseId == courseId && x.TagId == tagId).AnyAsync();
        }

        public async Task<bool> CourseCategoryExistsById(Guid courseCategoryId)
        {
            return await _dataContext.CourseCategory.Where(x => x.CourseCategoryId == courseCategoryId).AnyAsync();
        }

        public async Task<bool> CourseExistsById(Guid courseId)
        {
            return await _dataContext.Course
            .Where(x => x.CourseId == courseId).AnyAsync();
        }

        public async Task<bool> CourseExistsByTitle(string courseTitle)
        {
            return await _dataContext.Course.Where(x => x.Title.ToLower().Equals(courseTitle.ToLower())).AnyAsync();
        }

        public async Task<bool> CourseExistsByTitleExceptTheGivenCourseById(Guid courseId, string title)
        {
            return await _dataContext.Course
            .Where(
                x => x.CourseId != courseId && x.Title.ToLower().Equals(title.ToLower())
            )
            .AnyAsync();
        }

        public async Task<bool> CourseLessonExistsById(Guid courseLessonId)
        {
            return await _dataContext.CourseLesson.Where(
                x => x.CourseLessonId == courseLessonId
            ).AnyAsync();
        }

        public async Task<bool> CourseLessonResourceExists(Guid courseLessonResourceId)
        {
            return await _dataContext.CourseLessonResource.Where(
                x => x.CourseLessonResourceId == courseLessonResourceId
            ).AnyAsync();
        }

        public async Task<bool> CourseTeachingResourceExists(Guid courseTeachingResourceId)
        {
            return await _dataContext.CourseTeachingResource.Where(
                x => x.CourseTeachingResourceId == courseTeachingResourceId
            ).AnyAsync();
        }

        public async Task<bool> CourseThumbnailExists(Guid courseId)
        {
            return await _dataContext.CourseThumbnail.Where(x => x.CourseId == courseId).AnyAsync();
        }

        public async Task<bool> CreateCourse(Entities.Course.Course course)
        {
            try
            {
                await _dataContext.Course.AddAsync(course);
                await _dataContext.SaveChangesAsync();
                return true;
            }
            catch (System.Exception ex)
            {

                Console.WriteLine("Failed to create course " + course.Title);
                Console.WriteLine(ex);
                return false;
            }
        }

        public async Task<bool> CreateCourseLesson(CourseLesson courseLesson)
        {
            try
            {
                await _dataContext.CourseLesson.AddAsync(courseLesson);
                await _dataContext.SaveChangesAsync();
                return true;
            }
            catch (System.Exception ex)
            {

                Console.WriteLine("Failed to create course lesson " + courseLesson.CourseId);
                Console.WriteLine(ex);
                return false;
            }
        }

        public async Task<bool> CreateCourseLessonContent(CourseLessonContent courseLessonContent)
        {
            try
            {
                await _dataContext.CourseLessonContent.AddAsync(courseLessonContent);
                await _dataContext.SaveChangesAsync();
                return true;
            }
            catch (System.Exception ex)
            {

                Console.WriteLine("Failed to create course lesson content " + courseLessonContent.CourseLessonId);
                Console.WriteLine(ex);
                return false;

            }
        }

        public async Task<bool> CreateCourseLessonResource(CourseLessonResource courseLessonResource)
        {
            try
            {
                await _dataContext.CourseLessonResource.AddAsync(courseLessonResource);
                await _dataContext.SaveChangesAsync();
                return true;
            }
            catch (System.Exception ex)
            {

                Console.WriteLine("Failed to create course lesson resource " + courseLessonResource.CourseLessonId);
                Console.WriteLine(ex);
                return false;
            }
        }

        public async Task<bool> CreateCoursePromotionImage(CoursePromotionImage promotionImage)
        {
            try
            {
                await _dataContext.CoursePromotionImage.AddAsync(promotionImage);
                await _dataContext.SaveChangesAsync();
                return true;
            }
            catch (System.Exception ex)
            {

                Console.WriteLine("Failed to create course promotion image " + promotionImage.CourseId);
                Console.WriteLine(ex);
                return false;
            }
        }

        public async Task<bool> CreateCourseTag(CourseTag courseTag)
        {
            try
            {
                await _dataContext.CourseTag.AddAsync(courseTag);
                await _dataContext.SaveChangesAsync();
                return true;
            }
            catch (System.Exception ex)
            {

                Console.WriteLine("Failed to create course tag " + courseTag.CourseId);
                Console.WriteLine(ex);
                return false;
            }
        }

        public async Task<bool> CreateCourseTeachingResource(CourseTeachingResource courseTeachingResource)
        {
            try
            {
                await _dataContext.CourseTeachingResource.AddAsync(courseTeachingResource);
                await _dataContext.SaveChangesAsync();
                return true;
            }
            catch (System.Exception ex)
            {
                Console.WriteLine("Failed to create course teaching resource " + courseTeachingResource.CourseId);
                Console.WriteLine(ex);
                return false;
            }
        }

        public async Task<bool> CreateCourseThumbnail(CourseThumbnail courseThumbnail)
        {
            try
            {
                await _dataContext.CourseThumbnail.AddAsync(courseThumbnail);
                await _dataContext.SaveChangesAsync();
                return true;
            }
            catch (System.Exception ex)
            {

                Console.WriteLine("Failed to create course thumbnail " + courseThumbnail.CourseId);
                Console.WriteLine(ex);
                return false;
            }
        }

        public async Task<bool> CreateCourseViewershipData(CourseViewershipData courseViewershipData)
        {
            try
            {
                await _dataContext.CourseViewershipData.AddAsync(courseViewershipData);
                await _dataContext.SaveChangesAsync();
                return true;
            }
            catch (System.Exception ex)
            {

                Console.WriteLine("Failed to create course viewership data " + courseViewershipData.CourseId);
                Console.WriteLine(ex);
                return false;
            }
        }

        public async Task<bool> CreateTag(Tag tag)
        {
            try
            {
                await _dataContext.Tag.AddAsync(tag);
                await _dataContext.SaveChangesAsync();
                return true;
            }
            catch (System.Exception ex)
            {

                Console.WriteLine("Failed to create tag " + tag.Name);
                Console.WriteLine(ex);
                return false;
            }
        }

        public async Task<bool> DeleteCourseLessonResourceById(Guid courseLessonResourceId)
        {
            var courseLessonResource = await _dataContext.CourseLessonResource.Where(x => x.CourseLessonResourceId == courseLessonResourceId).FirstOrDefaultAsync();

            if (courseLessonResource == null)
            {
                return false;
            }

            try
            {
                _dataContext.CourseLessonResource.Remove(courseLessonResource);
                await _dataContext.SaveChangesAsync();
                return true;
            }
            catch (System.Exception ex)
            {

                Console.WriteLine("Failed to delete course lesson resource " + courseLessonResource.CourseLessonResourceId);
                Console.WriteLine(ex);
                return false;
            }
        }

        public async Task<bool> DeleteCoursePromotionImageById(Guid coursePromotionImageId)
        {
            var coursePromotionImage = await _dataContext.CoursePromotionImage.Where(x => x.CoursePromotionImageId == coursePromotionImageId).FirstOrDefaultAsync();
            if (coursePromotionImage == null)
            {
                return false;
            }
            try
            {
                _dataContext.CoursePromotionImage.Remove(coursePromotionImage);
                await _dataContext.SaveChangesAsync();
                return true;
            }
            catch (System.Exception ex)
            {

                Console.WriteLine("Failed to delete course promotion image " + coursePromotionImage.CoursePromotionImageId);
                Console.WriteLine(ex);
                return false;
            }
        }

        public async Task<bool> DeleteCourseTag(CourseTag courseTag)
        {
            try
            {
                _dataContext.CourseTag.Remove(courseTag);
                await _dataContext.SaveChangesAsync();
                return true;
            }
            catch (System.Exception ex)
            {

                Console.WriteLine("Failed to delete course tag " + courseTag.CourseId);
                Console.WriteLine(ex);
                return false;

            }
        }

        public async Task<bool> DeleteCourseTeachingResourceById(Guid courseTeachingResourceId)
        {
            var courseTeachingResource = await _dataContext.CourseTeachingResource.Where(x => x.CourseTeachingResourceId == courseTeachingResourceId).FirstOrDefaultAsync();

            if (courseTeachingResource == null)
            {
                return false;
            }

            try
            {
                _dataContext.CourseTeachingResource.Remove(courseTeachingResource);
                await _dataContext.SaveChangesAsync();
                return true;
            }
            catch (System.Exception ex)
            {

                Console.WriteLine("Failed to delete course teaching resource " + courseTeachingResourceId);
                Console.WriteLine(ex);
                return false;
            }
        }

        public async Task<bool> DeleteCourseThumbnail(Guid courseId)
        {
            try
            {
                var courseThumbnail = await _dataContext.CourseThumbnail.Where(x => x.CourseId == courseId).FirstOrDefaultAsync();
                _dataContext.CourseThumbnail.Remove(courseThumbnail);
                _dataContext.SaveChanges();
                return true;
            }
            catch (System.Exception ex)
            {

                Console.WriteLine("Failed to delete course thumbnail " + courseId);
                Console.WriteLine(ex);
                return false;
            }
        }

        public async Task<bool> DeleteTag(Tag tag)
        {
            try
            {
                _dataContext.Tag.Remove(tag);
                await _dataContext.SaveChangesAsync();
                return true;
            }
            catch (System.Exception ex)
            {

                Console.WriteLine("Failed to delete tag " + tag.Name);
                Console.WriteLine(ex);
                return false;
            }
        }

        public async Task<List<GetAllCourseLessonResourcesResponse>> GetAllCourseLessonResourcesByCourseLessonId(Guid courseLessonId)
        {
            return await _dataContext.CourseLessonResource
            .Where(
                x => x.CourseLessonId == courseLessonId
            )
            .Select(
                x => new GetAllCourseLessonResourcesResponse
                {
                    CourseLessonResourceId = x.CourseLessonResourceId,
                    CourseLessonId = x.CourseLessonId,
                    Title = x.Title,
                    Description = x.Description,
                    ResourceUrl = x.ResourceUrl,
                    FileName = x.FileName,
                    ContentType = x.ContentType,
                    FileSize = x.FileSize,
                    CreatedAt = DateTimeOffset.FromUnixTimeMilliseconds(x.CreatedAt).UtcDateTime
                }
            )
            .ToListAsync();
        }

        public async Task<List<GetAllCourseLessonsResponse>> GetAllCourseLessons(Guid courseId)
        {
            return await _dataContext.CourseLesson
            .Where(x => x.CourseId == courseId)
            .Select(
                x => new GetAllCourseLessonsResponse
                {
                    CourseId = x.CourseId,
                    CourseLessonId = x.CourseLessonId,
                    Title = x.Title,
                    Topic = x.Topic,
                    ShortSummary = x.ShortSummary,
                    PublishedStatus = x.PublishedStatus,
                    LessonSequenceOrder = x.LessonSequenceOrder,
                    StatusChangedAt = x.StatusChangedAt,
                    CreatedAt = DateTimeOffset.FromUnixTimeMilliseconds(x.CreatedAt).UtcDateTime,
                    UpdatedAt = x.UpdatedAt.HasValue ? DateTimeOffset.FromUnixTimeMilliseconds(x.UpdatedAt.Value).UtcDateTime : null
                }
            ).ToListAsync();
        }

        public async Task<List<Entities.Course.Course>> GetAllCoursesByTutorId(Guid tutorId)
        {
            return await _dataContext.Course
            .Include(x => x.CourseCategory)
            .Include(x => x.LearningDifficultyLevel)
            .Include(x => x.CourseThumbnail)
            .Where(x => x.TutorId == tutorId).ToListAsync();
        }

        public async Task<List<GetAllCourseTagsByCourseId>> GetAllCourseTagsByCourseId(Guid courseId)
        {
            return await _dataContext.CourseTag
            .Include(x => x.Tag)
            .Where(x => x.CourseId == courseId && x.Tag != null)
            .Select(
                x => new GetAllCourseTagsByCourseId
                {
                    CourseTagId = x.CourseTagId,
                    TagId = x.TagId,
                    CourseId = x.CourseId,
                    Name = x.Tag.Name,
                }
            ).ToListAsync();
        }

        public Task<List<GetCourseTeachingResourceResponse>> GetAllCourseTeachingResourcesWithoutFileDataByCourseId(Guid courseId)
        {
            return _dataContext.CourseTeachingResource
            .Where(x => x.CourseId == courseId)
            .Select(
                x => new GetCourseTeachingResourceResponse
                {
                    CourseTeachingResourceId = x.CourseTeachingResourceId,
                    Title = x.Title,
                    Description = x.Description,
                    ResourceUrl = x.ResourceUrl,
                    FileName = x.FileName,
                    ContentType = x.ContentType,
                    FileSize = x.FileSize,
                    CreatedAt = DateTimeOffset.FromUnixTimeMilliseconds(x.CreatedAt).UtcDateTime
                }
            ).ToListAsync();
        }




        public async Task<List<GetAllTagsByTutorResponse>?> GetAllTagsByTutor(Guid tutorId)
        {
            var tutor = await _dataContext.Tutor.Where(x => x.TutorId == tutorId).FirstOrDefaultAsync();

            if (tutor == null)
            {
                return null;
            }
            return await _dataContext.Tag
            .Where(x => x.CreatedByPersonId == tutor.PersonId)
            .Select(
                x => new GetAllTagsByTutorResponse
                {
                    TagId = x.TagId,
                    Name = x.Name,
                    CourseCount = _dataContext.CourseTag.Count(ct => ct.TagId == x.TagId),
                    TutorId = tutor.TutorId,
                }
            ).ToListAsync();
        }



        public async Task<(int totalViews, int uniqueUsers)> GetCourseAnalyticsForCourseManagementDashboard(Guid courseId)
        {
            var totalViews = await _dataContext.CourseViewershipData.Where(x => x.CourseId == courseId).CountAsync();

            var uniqueUsers = await _dataContext.CourseViewershipData.Where(x => x.CourseId == courseId).Select(x => x.ViewedByPersonId).Distinct().CountAsync();

            return (totalViews, uniqueUsers);
        }

        public async Task<List<GetCourseAnalyticsHistoryResponse>> GetCourseAnalyticsHistory(Guid courseId)
        {
            return await _dataContext.CourseViewershipDataSnapshot
            .Where(x => x.CourseId == courseId)
            .Select(
                x => new GetCourseAnalyticsHistoryResponse
                {
                    CourseViewershipDataSnapshotId = x.CourseViewershipDataSnapshotId,
                    CourseId = x.CourseId,
                    TotalViews = x.TotalViews,
                    NumberOfUniqueVisitors = x.NumberOfUniqueVisitors,
                    CurrentlyViewing = x.CurrentlyViewing,
                    AverageViewDurationInMinutes = x.AverageViewDurationInMinutes,
                    CreatedAt = DateTimeOffset.FromUnixTimeMilliseconds(x.CreatedAt).UtcDateTime
                }
            ).ToListAsync();
        }

        public async Task<Entities.Course.Course?> GetCourseById(Guid courseId)
        {
            return await _dataContext.Course
            .Include(x => x.Tutor)
            .Include(x => x.CourseCategory)
            .Include(x => x.LearningDifficultyLevel)
            .Include(x => x.CourseThumbnail)
            .FirstOrDefaultAsync(x => x.CourseId == courseId);
        }

        public async Task<GetCoursesByQueryResponse?> GetCourseByIdForStudent(Guid courseId, string requestScheme, string requestHost)
        {
            return await _dataContext.Course
            .Include(x => x.CourseCategory)
            .Include(x => x.Tutor)
                .ThenInclude(x => x.Person)
                    .ThenInclude(x => x.PersonEmail)
                        .ThenInclude(x => x.Person.PersonDetails)
            .Include(x => x.LearningDifficultyLevel)
            .Include(x => x.CourseThumbnail)
            .Where(x => x.CourseId == courseId)
            .Select(
                x => new GetCoursesByQueryResponse
                {
                    CourseId = x.CourseId,
                    Title = x.Title,
                    Description = x.Description,
                    CourseCategoryId = x.CourseCategoryId,
                    CourseCategoryName = x.CourseCategory.Name,
                    LearningDifficultyLevelId = x.LearningDifficultyLevelId,
                    LearningDifficultyLevelName = x.LearningDifficultyLevel.Name,
                    Price = x.Price,
                    MinNumberOfStudents = x.MinNumberOfStudents,
                    MaxNumberOfStudents = x.MaxNumberOfStudents,
                    CreatedAt = DateTimeOffset.FromUnixTimeMilliseconds(x.CreatedAt).UtcDateTime,
                    HasThumbnail = x.CourseThumbnail != null,
                    ThumbnailUrl = x.CourseThumbnail != null
                                ? (!string.IsNullOrEmpty(x.CourseThumbnail.ThumbnailUrl)
                                    ? x.CourseThumbnail.ThumbnailUrl
                                    : $"{requestScheme}://{requestHost}/public/course/thumbnail/get?courseId={x.CourseId}")
                                : null,
                    NumberOfStudents = 0,
                    TutorUsername = x.Tutor.Person.PersonDetails.Username,
                    TutorEmail = x.Tutor.Person.PersonEmail.Email,
                }
            ).FirstOrDefaultAsync();


        }

        public async Task<CourseLesson?> GetCourseLessonById(Guid courseLessonId)
        {
            return await _dataContext.CourseLesson.Include(x => x.CourseLessonContent).Include(x => x.Course).Where(x => x.CourseLessonId == courseLessonId).FirstOrDefaultAsync();

        }

        public Task<GetCourseLessonByIdResponse?> GetCourseLessonByIdForTutorDashboard(Guid courseLessonId)
        {
            return _dataContext.CourseLesson
            .Include(x => x.CourseLessonContent)
            .Where(x => x.CourseLessonId == courseLessonId)
            .Select(
                x => new GetCourseLessonByIdResponse
                {
                    CourseId = x.CourseId,
                    CourseLessonId = x.CourseLessonId,
                    Title = x.Title,
                    Topic = x.Topic,
                    ShortSummary = x.ShortSummary,
                    Description = x.Description,

                    PublishedStatus = x.PublishedStatus,
                    LessonSequenceOrder = x.LessonSequenceOrder,
                    StatusChangedAt = x.StatusChangedAt,
                    CreatedAt = DateTimeOffset.FromUnixTimeMilliseconds(x.CreatedAt).UtcDateTime,
                    UpdatedAt = x.UpdatedAt.HasValue ? DateTimeOffset.FromUnixTimeMilliseconds(x.UpdatedAt.Value).UtcDateTime : null,
                    CourseLessonContent = x.CourseLessonContent.Content
                }
            ).FirstOrDefaultAsync();
        }

        public async Task<int> GetCourseLessonCountByCourseId(Guid courseId)
        {
            return await _dataContext.CourseLesson
            .Where(x => x.CourseId == courseId)
            .CountAsync();
        }

        public async Task<CourseLessonResource?> GetCourseLessonResourceById(Guid courseLessonResourceId)
        {
            return await _dataContext.CourseLessonResource.Where(
                x => x.CourseLessonResourceId == courseLessonResourceId
            ).FirstOrDefaultAsync();
        }

        public async Task<GetCourseLessonResourceWithoutFileDataByIdResponse?> GetCourseLessonResourceByIdWithoutFileData(Guid courseLessonResourceId)
        {
            return await _dataContext.CourseLessonResource
            .Where(
                x => x.CourseLessonResourceId == courseLessonResourceId
            )
            .Select(
                x => new GetCourseLessonResourceWithoutFileDataByIdResponse
                {
                    CourseLessonResourceId = x.CourseLessonResourceId,
                    CourseLessonId = x.CourseLessonId,
                    Title = x.Title,
                    FileName = x.FileName,
                    ContentType = x.ContentType,
                    FileSize = x.FileSize,
                    ResourceUrl = x.ResourceUrl,
                    Description = x.Description,
                    CreatedAt = DateTimeOffset.FromUnixTimeMilliseconds(x.CreatedAt).UtcDateTime
                }
            )
            .FirstOrDefaultAsync();
        }

        public async Task<List<CourseLesson>> GetCourseLessonsByCourseId(Guid courseId)
        {
            return await _dataContext.CourseLesson
            .Include(x => x.CourseLessonContent)
            .Where(x => x.CourseId == courseId)
            .ToListAsync();
        }

        public Task<GetCourseLessonsCountFilteredByPublishedStatusRepositoryResponse?> GetCourseLessonsCountFilteredByPublishedStatus(Guid courseId)
        {
            return _dataContext.CourseLesson
            .Where(x => x.CourseId == courseId)
            .GroupBy(x => 1)
            .Select(
                x => new GetCourseLessonsCountFilteredByPublishedStatusRepositoryResponse
                {
                    TotalNumberOfLessons = x.Count(),
                    NumberOfPublishedLessons = x.Where(x => x.PublishedStatus == PublishedStatus.Published).Count(),
                    NumberOfDraftLessons = x.Where(x => x.PublishedStatus == PublishedStatus.Draft).Count(),
                    NumberOfArchivedLessons = x.Where(x => x.PublishedStatus == PublishedStatus.Archived).Count()
                }
            )
            .FirstOrDefaultAsync();
        }

        public async Task<Guid?> GetCourseLessonTutorByCourseLessonId(Guid courseLessonId)
        {
            return await _dataContext.CourseLesson.Where(
                x => x.CourseLessonId == courseLessonId
            )
            .Select(
                x => x.TutorId
            )
            .FirstOrDefaultAsync();
        }

        public async Task<CoursePromotionImage?> GetCoursePromotionImageById(Guid coursePromotionImageId)
        {
            return await _dataContext.CoursePromotionImage
            .Include(x => x.Course)
            .Where(x => x.CoursePromotionImageId == coursePromotionImageId)
            .FirstOrDefaultAsync();
        }

        public async Task<GetCoursePromotionImageMetadataByIdResponse?> GetCoursePromotionImageMetadataById(Guid coursePromotionImageId)
        {
            return await _dataContext
            .CoursePromotionImage
            .Include(x => x.Course)
            .Where(
                x => x.CoursePromotionImageId == coursePromotionImageId
            )
            .Select(
                x => new GetCoursePromotionImageMetadataByIdResponse
                {
                    CoursePromotionImageId = x.CoursePromotionImageId,
                    CourseId = x.CourseId,
                    CourseTitle = x.Course.Title,
                    UploadedAt = x.UpdatedAt.HasValue ? DateTimeOffset.FromUnixTimeMilliseconds(x.UpdatedAt.Value).UtcDateTime : DateTimeOffset.FromUnixTimeMilliseconds(x.CreatedAt).UtcDateTime
                }
            )
            .FirstOrDefaultAsync();
        }

        public async Task<List<GetCoursePromotionImagesMetadataResponse>> GetCoursePromotionImagesMetadataByCourseId(Guid courseId)
        {
            return await _dataContext
            .CoursePromotionImage
            .Where(
                x => x.CourseId == courseId
            )
            .Select(
                x => new GetCoursePromotionImagesMetadataResponse
                {
                    CoursePromotionImageId = x.CoursePromotionImageId,
                    CourseId = x.CourseId,
                    UploadedAt = x.UpdatedAt.HasValue ? DateTimeOffset.FromUnixTimeMilliseconds(x.UpdatedAt.Value).UtcDateTime : DateTimeOffset.FromUnixTimeMilliseconds(x.CreatedAt).UtcDateTime
                }
            ).ToListAsync();
        }

        public async Task<(int, long?)> GetCoursePromotionImagesMetadataForCourseManagementDashboard(Guid courseId)
        {
            var query = _dataContext.CoursePromotionImage
            .Where(x => x.CourseId == courseId);

            var result = await query
            .GroupBy(x => 1)
            .Select(group => new
            {
                TotalCount = group.Count(),
                LatestUploadedAt = group.Max(x => x.UpdatedAt ?? x.CreatedAt)
            })
            .FirstOrDefaultAsync();

            return (result?.TotalCount ?? 0, result?.LatestUploadedAt ?? null);
        }

        public async Task<GetCourseRequirementsByCourseIdResponseFromRepository?> GetCourseRequirementsByCourseId(Guid courseId)
        {
            return await _dataContext.Course
            .Where(x => x.CourseId == courseId)
            .Select(
                x => new GetCourseRequirementsByCourseIdResponseFromRepository
                {
                    CourseId = x.CourseId,
                    Price = x.Price,
                    MaxNumberOfStudents = x.MaxNumberOfStudents,
                    PublishedStatus = x.PublishedStatus

                }
            ).FirstOrDefaultAsync();
        }

        public async Task<(List<GetCoursesByQueryResponse>, int TotalCount)> GetCoursesByQuery(string query, string requestScheme, string requestHost, int pageNumber = 1,
        int pageSize = 10)
        {

            if (pageNumber < 1) pageNumber = 1;
            if (pageSize < 1) pageSize = 10;

            var dbQuery = _dataContext.Course
       .Include(x => x.CourseCategory)
       .Include(x => x.Tutor)
       .Include(x => x.Tutor.Person.PersonEmail)
       .Include(x => x.Tutor.Person.PersonDetails)
       .Include(x => x.LearningDifficultyLevel)
       .Include(x => x.CourseThumbnail)
       .AsQueryable();

            if (!string.IsNullOrWhiteSpace(query))
            {
                var lowerQuery = query.Trim().ToLower();

                dbQuery = dbQuery.Where(x =>
                        // x.PublishedStatus == PublishedStatus.Published &&
                        // (
                        x.Title.ToLower().Contains(lowerQuery) ||
                        x.Tutor.Person.PersonEmail.Email.ToLower().Contains(lowerQuery) ||
                        x.Tutor.Person.PersonDetails.FirstName.ToLower().Contains(lowerQuery) ||
                        x.Tutor.Person.PersonDetails.LastName.ToLower().Contains(lowerQuery) ||
                        x.LearningDifficultyLevel.Name.ToLower().Contains(lowerQuery) ||
                        x.CourseCategory.Name.ToLower().Contains(lowerQuery)
                // )
                );
            }
            // else
            // {
            //     dbQuery = dbQuery.Where(x => x.PublishedStatus == PublishedStatus.Published);
            // }

            var totalCount = await dbQuery.CountAsync();


            var pagedCourses = await dbQuery
                .Select(x => new GetCoursesByQueryResponse
                {
                    CourseId = x.CourseId,
                    Title = x.Title,
                    Description = x.Description,
                    CourseCategoryId = x.CourseCategoryId,
                    CourseCategoryName = x.CourseCategory.Name,
                    LearningDifficultyLevelId = x.LearningDifficultyLevelId,
                    LearningDifficultyLevelName = x.LearningDifficultyLevel.Name,
                    Price = x.Price,
                    MinNumberOfStudents = x.MinNumberOfStudents,
                    MaxNumberOfStudents = x.MaxNumberOfStudents,
                    CreatedAt = DateTimeOffset.FromUnixTimeMilliseconds(x.CreatedAt).UtcDateTime,
                    HasThumbnail = x.CourseThumbnail != null,
                    ThumbnailUrl = x.CourseThumbnail != null
                        ? (!string.IsNullOrEmpty(x.CourseThumbnail.ThumbnailUrl)
                            ? x.CourseThumbnail.ThumbnailUrl
                            : $"{requestScheme}://{requestHost}/public/course/thumbnail/get?courseId={x.CourseId}")
                        : null,
                    NumberOfStudents = 0,
                    TutorUsername = x.Tutor.Person.PersonDetails.Username,
                    TutorEmail = x.Tutor.Person.PersonEmail.Email,
                })
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();
            return (pagedCourses, totalCount);
        }

        public async Task<CourseTag?> GetCourseTagByCourseIdAndTagId(Guid courseId, Guid tagId)
        {
            return await _dataContext.CourseTag
            .Include(x => x.Course)
            .Include(x => x.Tag)
            .Where(x => x.CourseId == courseId && x.TagId == tagId)
            .FirstOrDefaultAsync();
        }

        public async Task<CourseTeachingResource?> GetCourseTeachingResourceById(Guid courseTeachingResourceId)
        {
            return await _dataContext.CourseTeachingResource
            .Where(x => x.CourseTeachingResourceId == courseTeachingResourceId)
            .FirstOrDefaultAsync();
        }

        public async Task<GetCourseTeachingResourceByIdIncludeCourseExcludeFileDataIfFile?> GetCourseTeachingResourceByIdWithoutFileData(Guid courseTeachingResourceId)
        {
            return await _dataContext.CourseTeachingResource
            .Include(x => x.Course)
            .Where(
                x => x.CourseTeachingResourceId == courseTeachingResourceId
            )
            .Select(
                x => new GetCourseTeachingResourceByIdIncludeCourseExcludeFileDataIfFile
                {
                    CourseTeachingResourceId = x.CourseTeachingResourceId,
                    Title = x.Title,
                    Description = x.Description,
                    ResourceUrl = x.ResourceUrl,
                    FileName = x.FileName,
                    ContentType = x.ContentType,
                    FileSize = x.FileSize,
                    CreatedAt = x.CreatedAt,
                    UpdatedAt = x.UpdatedAt,
                    Course = x.Course
                }
            ).FirstOrDefaultAsync();
        }

        public async Task<GetCourseTeachingResourcesInformationByCourseIdResponseFromRepository?> GetCourseTeachingResourcesInformationByCourseId(Guid courseId)
        {
            var query = _dataContext.CourseTeachingResource.Where(x => x.CourseId == courseId);

            var result = await query
            .GroupBy(x => 1)
            .Select(
                x => new GetCourseTeachingResourcesInformationByCourseIdResponseFromRepository
                {
                    TotalNumberOfTeachingResources = x.Count(),
                    NumberOfFiles = x.Count(x => x.ResourceUrl == null),
                    NumberOfURLs = x.Count(x => x.ResourceUrl != null),
                    TotalSizeOfFilesInBytes = x.Sum(x => x.FileSize ?? 0),
                    TwoLatestAddedTeachingResources = x.OrderByDescending(
                        x => x.UpdatedAt ?? x.CreatedAt
                    )
                    .Take(2)
                    .Select(y => new GetCourseTeachingResourceResponse
                    {
                        CourseTeachingResourceId = y.CourseTeachingResourceId,
                        Title = y.Title,
                        FileName = y.FileName,
                        ContentType = y.ContentType,
                        FileSize = y.FileSize,
                        ResourceUrl = y.ResourceUrl,
                        Description = y.Description,
                        CreatedAt = DateTimeOffset.FromUnixTimeMilliseconds(y.CreatedAt).UtcDateTime,
                    }).ToList()


                }
            ).FirstOrDefaultAsync();

            return result ?? null;
        }

        public async Task<CourseThumbnail?> GetCourseThumbnailByCourseId(Guid courseId)
        {
            return await _dataContext.CourseThumbnail.Include(x => x.Course).Where(x => x.CourseId == courseId).FirstOrDefaultAsync();
        }

        public async Task<(int usersCameFromFeedCount, int usersCameFromSearchCount)> GetCourseUsersCameFromCounts(Guid courseId)
        {

            var data = await _dataContext.CourseViewershipData
            .Where(x => x.CourseId == courseId)
            .GroupBy(x => x.UserCameFrom)
            .Select(g => new
            {
                Source = g.Key,
                Count = g.Count()
            })
            .ToListAsync();

            int usersCameFromFeedCount = data.FirstOrDefault(x => x.Source == UserCourseSourceType.Feed)?.Count ?? 0;
            int usersCameFromSearchCount = data.FirstOrDefault(x => x.Source == UserCourseSourceType.Search)?.Count ?? 0;

            return (usersCameFromFeedCount, usersCameFromSearchCount);
        }

        public async Task<CourseViewershipData?> GetCourseViewershipDataById(Guid courseViewershipDataId)
        {
            return await _dataContext.CourseViewershipData.Where(x => x.CourseViewershipDataId == courseViewershipDataId).FirstOrDefaultAsync();
        }

        public async Task<List<GetCourseLessonByIdResponse>> GetLatestCourseLessons(Guid courseId)
        {
            return await _dataContext
            .CourseLesson
            .Include(x => x.CourseLessonContent)
            .Where(
                x => x.CourseId == courseId
            )
            .OrderByDescending(x => x.CreatedAt)
            .Take(2)
            .Select(
                x => new GetCourseLessonByIdResponse
                {
                    CourseId = x.CourseId,
                    CourseLessonId = x.CourseLessonId,
                    Title = x.Title,
                    ShortSummary = x.ShortSummary,
                    Description = x.Description,
                    Topic = x.Topic,
                    LessonSequenceOrder = x.LessonSequenceOrder,
                    PublishedStatus = x.PublishedStatus,
                    StatusChangedAt = x.StatusChangedAt,
                    CreatedAt = DateTimeOffset.FromUnixTimeMilliseconds(x.CreatedAt).UtcDateTime,
                    UpdatedAt = x.UpdatedAt != null ? DateTimeOffset.FromUnixTimeMilliseconds(x.UpdatedAt.Value).UtcDateTime : null,
                    CourseLessonContent = x.CourseLessonContent != null ? x.CourseLessonContent.Content : ""
                }
            )
            .ToListAsync();
        }

        public async Task<int> GetLessonCountByCourseId(Guid courseId)
        {
            return await _dataContext.CourseLesson.Where(x => x.CourseId == courseId).CountAsync();
        }

        public async Task<int> GetPromotionImageCountByCourseId(Guid courseId)
        {
            return await _dataContext.CoursePromotionImage.Where(x => x.CourseId == courseId).CountAsync();
        }

        public async Task<int> GetPublishedCourseLessonCountByCourseId(Guid courseId)
        {
            return await _dataContext.CourseLesson
            .Where(
                x => x.CourseId == courseId &&
                x.PublishedStatus == PublishedStatus.Published
            )
            .CountAsync();
        }

        public async Task<Tag?> GetTagById(Guid? tagId)
        {
            return await _dataContext.Tag.Where(x => x.TagId == tagId).FirstOrDefaultAsync();
        }

        public Task<bool> IsTutorCoursePromotionImageOwner(Guid imageId, Guid tutorId)
        {
            return _dataContext.CoursePromotionImage
            .Include(x => x.Course)
            .Where(x => x.CoursePromotionImageId == imageId && x.Course.TutorId == tutorId)
            .AnyAsync();
        }

        public async Task<bool> RearrangeLessonSequenceAsync(Guid courseId, int currentPosition, int newPosition)
        {
            if (currentPosition == newPosition) return true;

            using var transaction = await _dataContext.Database.BeginTransactionAsync();

            try
            {
                var lessons = await _dataContext.CourseLesson
                    .Where(l => l.CourseId == courseId && l.LessonSequenceOrder.HasValue)
                    .OrderBy(l => l.LessonSequenceOrder)
                    .ToListAsync();

                var lessonToMove = lessons.FirstOrDefault(l => l.LessonSequenceOrder == currentPosition);
                if (lessonToMove == null) return false;

                // Determine movement direction
                bool movingRight = newPosition > currentPosition;

                foreach (var lesson in lessons)
                {
                    if (movingRight)
                    {
                        if (lesson.LessonSequenceOrder > currentPosition && lesson.LessonSequenceOrder <= newPosition)
                        {
                            lesson.LessonSequenceOrder--;
                        }
                    }
                    else
                    {
                        if (lesson.LessonSequenceOrder < currentPosition && lesson.LessonSequenceOrder >= newPosition)
                        {
                            lesson.LessonSequenceOrder++;
                        }
                    }
                }

                // Set the new position
                lessonToMove.LessonSequenceOrder = newPosition;

                await _dataContext.SaveChangesAsync();
                await transaction.CommitAsync();
                return true;
            }
            catch
            {
                await transaction.RollbackAsync();
                return false;
            }
        }

        public async Task<bool> RemoveLessonFromSequenceAsync(Guid courseId, int removedPosition)
        {
            using var transaction = await _dataContext.Database.BeginTransactionAsync();

            try
            {
                var lessons = await _dataContext.CourseLesson
                    .Where(l => l.CourseId == courseId && l.LessonSequenceOrder.HasValue)
                    .OrderBy(l => l.LessonSequenceOrder)
                    .ToListAsync();

                var lessonToRemove = lessons.FirstOrDefault(l => l.LessonSequenceOrder == removedPosition);
                if (lessonToRemove == null) return true;

                // Set the removed lesson's sequence to null
                lessonToRemove.LessonSequenceOrder = null;

                // Shift all lessons after the removed position one step down
                foreach (var lesson in lessons.Where(l => l.LessonSequenceOrder > removedPosition))
                {
                    lesson.LessonSequenceOrder--;
                }

                await _dataContext.SaveChangesAsync();
                await transaction.CommitAsync();
                return true;
            }
            catch
            {
                await transaction.RollbackAsync();
                return false;
            }
        }


        public async Task<bool> TagExistsById(Guid? tagId)
        {
            return await _dataContext.Tag.Where(x => x.TagId == tagId).AnyAsync();
        }

        public async Task<bool> TagExistsByName(string tagName)
        {
            return await _dataContext.Tag.Where(x => x.Name == tagName).AnyAsync();
        }

        public Task<bool> TagExistsByNameExcludingId(Guid tagId, string tagName)
        {
            return _dataContext.Tag.Where(x => x.TagId != tagId && x.Name == tagName).AnyAsync();
        }

        public async Task<bool> UpdateCourseBasics(Entities.Course.Course course)
        {
            try
            {
                _dataContext.Course.Update(course);
                await _dataContext.SaveChangesAsync();
                return true;
            }
            catch (System.Exception ex)
            {

                Console.WriteLine("Failed to update course " + course.Title);
                Console.WriteLine(ex);
                return false;
            }
        }

        public async Task<bool> UpdateCourseLesson(CourseLesson courseLesson)
        {
            try
            {
                _dataContext.CourseLesson.Update(courseLesson);
                await _dataContext.SaveChangesAsync();
                return true;
            }
            catch (System.Exception ex)
            {

                Console.WriteLine("Failed to update course lesson " + courseLesson.Title);
                Console.WriteLine(ex);
                return false;
            }
        }

        public async Task<bool> UpdateCourseLessonResource(CourseLessonResource courseLessonResource)
        {
            try
            {
                _dataContext.CourseLessonResource.Update(courseLessonResource);
                await _dataContext.SaveChangesAsync();
                return true;
            }
            catch (System.Exception ex)
            {
                Console.WriteLine("Failed to update course lesson resource " + courseLessonResource.Title);
                Console.WriteLine(ex);
                return false;
            }
        }

        public async Task<bool> UpdateCoursePromotionImage(CoursePromotionImage promotionImage)
        {
            try
            {
                _dataContext.CoursePromotionImage.Update(promotionImage);
                await _dataContext.SaveChangesAsync();
                return true;
            }
            catch (System.Exception ex)
            {

                Console.WriteLine("Failed to update course promotion image " + promotionImage.CoursePromotionImageId);
                Console.WriteLine(ex);
                return false;

            }
        }

        public async Task<bool> UpdateCourseTeachingResource(CourseTeachingResource courseTeachingResource)
        {
            try
            {
                _dataContext.CourseTeachingResource.Update(courseTeachingResource);
                await _dataContext.SaveChangesAsync();
                return true;

            }
            catch (System.Exception ex)
            {
                Console.WriteLine("Failed to update course teaching resource " + courseTeachingResource.CourseId);
                Console.WriteLine(ex);
                return false;
            }
        }

        public async Task<bool> UpdateCourseThumbnail(CourseThumbnail courseThumbnail)
        {
            try
            {
                _dataContext.CourseThumbnail.Update(courseThumbnail);
                await _dataContext.SaveChangesAsync();
                return true;
            }
            catch (System.Exception ex)
            {

                Console.WriteLine("Failed to update course thumbnail " + courseThumbnail.CourseId);
                Console.WriteLine(ex);
                return false;
            }
        }

        public async Task<bool> UpdateCourseViewershipData(CourseViewershipData courseViewershipData)
        {
            try
            {
                _dataContext.CourseViewershipData.Update(courseViewershipData);
                await _dataContext.SaveChangesAsync();
                return true;
            }
            catch (System.Exception ex)
            {

                Console.WriteLine("Failed to update course viewership data " + courseViewershipData.CourseId);
                Console.WriteLine(ex);
                return false;
            }
        }

        public async Task<bool> UpdateTag(Tag tag)
        {
            try
            {
                _dataContext.Tag.Update(tag);
                await _dataContext.SaveChangesAsync();
                return true;
            }
            catch (System.Exception ex)
            {

                Console.WriteLine("Failed to update tag " + tag.Name);
                Console.WriteLine(ex);
                return false;

            }
        }

        public async Task<PaginatedResponse<GetTagsBySearchResponse>> GetTagsBySearch(GetTagsBySearchPaginatedRequest request)
        {
            var query = _dataContext.Tag
                        .Include(x => x.CourseTags)
                        .ThenInclude(x => x.Course)
                        .ThenInclude(x => x.Tutor)
                        .ThenInclude(x => x.Person)
                        .ThenInclude(x => x.PersonEmail)
                        .AsQueryable();

            if (!string.IsNullOrEmpty(request.SearchQuery.Trim()))
            {
                query = query.Where(x => x.Name.Trim().Contains(request.SearchQuery.Trim().ToLower()));
            }

            var items = await query
            .OrderBy(x => x.Name)
            .Skip((request.PageNumber - 1) * request.PageSize)
            .Take(request.PageSize)
            .Select(
                x => new GetTagsBySearchResponse
                {
                    TagId = x.TagId,
                    Name = x.Name,
                    CourseCount = _dataContext.CourseTag.Where(y => y.TagId == x.TagId).Count(),
                    CreatedBy = x.Person != null && x.Person.PersonEmail != null ? x.Person.PersonEmail.Email : "",
                    CreatedAt = DateTimeOffset.FromUnixTimeMilliseconds(x.CreatedAt).UtcDateTime,
                    IsAssigned = request.ContainsTagCourseId.HasValue && _dataContext.CourseTag.Where(y => y.CourseId == request.ContainsTagCourseId.Value && x.TagId == y.TagId).Any(),
                }
            ).ToListAsync();




            var totalCount = await _dataContext.Tag.CountAsync();
            return new PaginatedResponse<GetTagsBySearchResponse>
            {
                PageNumber = request.PageNumber,
                PageSize = request.PageSize,
                TotalCount = totalCount,
                Items = items,
            };

        }
    }
}