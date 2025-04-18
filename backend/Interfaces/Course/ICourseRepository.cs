using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EduConnect.DTOs;
using EduConnect.Entities.Course;
using Microsoft.AspNetCore.Mvc;

namespace EduConnect.Interfaces.Course
{
    public interface ICourseRepository
    {
        Task<bool> CourseExistsByTitle(string courseTitle);
        Task<bool> CourseCategoryExistsById(Guid courseCategoryId);

        Task<bool> CreateCourse(Entities.Course.Course course);

        Task<List<Entities.Course.Course>> GetAllCoursesByTutorId(Guid tutorId);

        Task<Entities.Course.Course?> GetCourseById(Guid courseId);

        Task<bool> UpdateCourseBasics(Entities.Course.Course course);

        Task<bool> CourseExistsByTitleExceptTheGivenCourseById(Guid courseId, string title);

        Task<bool> CreateCourseThumbnail(Entities.Course.CourseThumbnail courseThumbnail);

        Task<bool> CourseThumbnailExists(Guid courseId);

        Task<Entities.Course.CourseThumbnail?> GetCourseThumbnailByCourseId(Guid courseId);

        Task<bool> UpdateCourseThumbnail(Entities.Course.CourseThumbnail courseThumbnail);

        Task<bool> DeleteCourseThumbnail(Guid courseId);

        Task<bool> CourseExistsById(Guid courseId);

        Task<bool> CreateCourseTeachingResource(Entities.Course.CourseTeachingResource courseTeachingResource);

        Task<CourseTeachingResource?> GetCourseTeachingResourceById(Guid courseTeachingResourceId);

        Task<bool> UpdateCourseTeachingResource(CourseTeachingResource courseTeachingResource);

        Task<bool> CourseTeachingResourceExists(Guid courseTeachingResourceId);

        Task<List<GetCourseTeachingResourceResponse>> GetAllCourseTeachingResourcesWithoutFileDataByCourseId(Guid courseId);

        Task<GetCourseTeachingResourcesInformationByCourseIdResponseFromRepository?> GetCourseTeachingResourcesInformationByCourseId(Guid courseId);

        Task<GetCourseTeachingResourceByIdIncludeCourseExcludeFileDataIfFile?> GetCourseTeachingResourceByIdWithoutFileData(Guid courseTeachingResourceId);
        Task<bool> DeleteCourseTeachingResourceById(Guid courseTeachingResourceId);

        Task<CourseLesson?> GetCourseLessonById(Guid courseLessonId);

        Task<int> GetCourseLessonCountByCourseId(Guid courseId);

        Task<bool> CreateCourseLesson(CourseLesson courseLesson);

        Task<bool> RearrangeLessonSequenceAsync(Guid courseId, int currentPosition, int newPosition);

        Task<bool> UpdateCourseLesson(CourseLesson courseLesson);

        Task<bool> CreateCourseLessonContent(CourseLessonContent courseLessonContent);

        Task<List<CourseLesson>> GetCourseLessonsByCourseId(Guid courseId);

        Task<int> GetPublishedCourseLessonCountByCourseId(Guid courseId);

        Task<List<GetAllCourseLessonsResponse>> GetAllCourseLessons(Guid courseId);

        Task<GetCourseLessonByIdResponse?> GetCourseLessonByIdForTutorDashboard(Guid courseLessonId);

        Task<bool> RemoveLessonFromSequenceAsync(Guid courseId, int removedPosition);

        Task<bool> CourseLessonResourceExists(Guid courseLessonResourceId);

        Task<bool> CourseLessonExistsById(Guid courseLessonId);
        Task<CourseLessonResource?> GetCourseLessonResourceById(Guid courseLessonResourceId);

        Task<bool> UpdateCourseLessonResource(CourseLessonResource courseLessonResource);

        Task<bool> CreateCourseLessonResource(CourseLessonResource courseLessonResource);
        Task<List<GetAllCourseLessonResourcesResponse>> GetAllCourseLessonResourcesByCourseLessonId(Guid courseLessonId);

        Task<GetCourseLessonResourceWithoutFileDataByIdResponse?> GetCourseLessonResourceByIdWithoutFileData(Guid courseLessonResourceId);

        Task<Guid?> GetCourseLessonTutorByCourseLessonId(Guid courseLessonId);

        Task<bool> DeleteCourseLessonResourceById(Guid courseLessonResourceId);
        Task<int> GetLessonCountByCourseId(Guid courseId);
        Task<GetCourseLessonsCountFilteredByPublishedStatusRepositoryResponse?> GetCourseLessonsCountFilteredByPublishedStatus(Guid courseId);

        Task<List<GetCourseLessonByIdResponse>> GetLatestCourseLessons(Guid courseId);

        Task<GetCourseRequirementsByCourseIdResponseFromRepository?> GetCourseRequirementsByCourseId(Guid courseId);

        Task<(List<GetCoursesByQueryResponse>, int TotalCount)> GetCoursesByQuery(string query, string requestScheme, string requestHost, int pageNumber = 1,
        int pageSize = 10);

        Task<GetCoursesByQueryResponse?> GetCourseByIdForStudent(Guid courseId, string requestScheme, string requestHost);

        Task<int> GetPromotionImageCountByCourseId(Guid courseId);

        Task<bool> CreateCoursePromotionImage(Entities.Course.CoursePromotionImage promotionImage);

        Task<bool> UpdateCoursePromotionImage(Entities.Course.CoursePromotionImage promotionImage);

        Task<bool> CheckCoursePromotionImageExists(Guid coursePromotionImageId);
        Task<CoursePromotionImage?> GetCoursePromotionImageById(Guid coursePromotionImageId);

        Task<bool> IsTutorCoursePromotionImageOwner(Guid imageId, Guid tutorId);

        Task<bool> DeleteCoursePromotionImageById(Guid coursePromotionImageId);

        Task<List<GetCoursePromotionImagesMetadataResponse>> GetCoursePromotionImagesMetadataByCourseId(Guid courseId);

        Task<GetCoursePromotionImageMetadataByIdResponse?> GetCoursePromotionImageMetadataById(Guid coursePromotionImageId);

        Task<(int, long?)> GetCoursePromotionImagesMetadataForCourseManagementDashboard(Guid courseId);

        Task<bool> CreateCourseViewershipData(CourseViewershipData courseViewershipData);
        Task<CourseViewershipData?> GetCourseViewershipDataById(Guid courseViewershipDataId);
        Task<bool> UpdateCourseViewershipData(CourseViewershipData courseViewershipData);

        Task<List<GetCourseAnalyticsHistoryResponse>> GetCourseAnalyticsHistory(Guid courseId);
        Task<(int usersCameFromFeedCount, int usersCameFromSearchCount)> GetCourseUsersCameFromCounts(Guid courseId);
        Task<(int totalViews, int uniqueUsers)> GetCourseAnalyticsForCourseManagementDashboard(Guid courseId);
        Task<bool> TagExistsById(Guid? tagId);
        Task<bool> TagExistsByName(string tagName);
        Task<Tag?> GetTagById(Guid? tagId);
        Task<bool> UpdateTag(Tag tag);
        Task<bool> CreateTag(Tag tag);

        Task<bool> CheckTagAssignedToCourse(Guid tagId, Guid courseId);
        Task<bool> CreateCourseTag(CourseTag courseTag);
        Task<bool> TagExistsByNameExcludingId(Guid tagId, string tagName);

        Task<CourseTag?> GetCourseTagByCourseIdAndTagId(Guid courseId, Guid tagId);
        Task<bool> DeleteCourseTag(CourseTag courseTag);
        Task<bool> DeleteTag(Tag tag);
        Task<List<GetAllCourseTagsByCourseId>> GetAllCourseTagsByCourseId(Guid courseId);
        Task<List<GetAllTagsByTutorResponse>?> GetAllTagsByTutor(Guid tutorId, Guid? assignedToCourseId);
        Task<PaginatedResponse<GetTagsBySearchResponse>> GetTagsBySearch(GetTagsBySearchPaginatedRequest request);
    }
}