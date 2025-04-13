import { HttpClient, HttpEvent, HttpEventType } from '@angular/common/http';
import { Injectable } from '@angular/core';
import ApiLinks from '../../../assets/api/link.api';
import { CreateCourseRequest } from '../../models/course/course-tutor-controller/create-course-request';
import { DefaultServerResponse } from '../../models/shared/default-server-response';
import { CheckCourseTitleExistsEmitGivenCourseRequest } from '../../models/course/course-tutor-controller/check-course-title-exists-emit-given-course-request';
import { buildHttpParams } from '../../helpers/build-http-params.helper';
import { UpdateCourseBasicsRequest } from '../../models/course/course-tutor-controller/update-course-basics-request';
import { UploadCourseThumbnailRequest } from '../../models/course/course-tutor-controller/upload-course-thumbnail-request';
import { map, Observable } from 'rxjs';
import { UploadCourseTeachingResourceRequest } from '../../models/course/course-tutor-controller/upload-course-teaching-resource-request';
import { CreateCourseLessonRequest } from '../../models/course/course-tutor-controller/create-course-lesson-request';
import { ChangeCourseLessonPublishedStatusRequest } from '../../models/course/course-tutor-controller/change-course-lesson-published-status-request';
import { GetCourseLessonResourceByIdResponse } from '../../models/course/course-tutor-controller/get-course-lesson-resource-by-id-response';
import { UploadCourseLessonResourceRequest } from '../../models/course/course-tutor-controller/upload-course-lesson-resource-request';
import { GetPromotionImagesResponse } from '../../models/course/course-tutor-controller/get-promotion-images-response';
import { GetCoursePromotionImageMetadataByIdResponse } from '../../models/course/course-tutor-controller/get-course-promotion-image-metadata-by-id-response';
import { UploadCoursePromotionImageRequest } from '../../models/course/course-tutor-controller/upload-course-promotion-image-request';
import { GetCourseAnalyticsHistoryResponse } from '../../models/course/course-tutor-controller/get-course-analytics-history-response';

@Injectable({
  providedIn: 'root',
})
export class CourseTutorControllerService {
  apiUrl = ApiLinks.CourseTutorControllerUrl;
  constructor(private httpClient: HttpClient) {}

  createCourse(request: CreateCourseRequest) {
    const token = localStorage.getItem('Authorization');
    return this.httpClient.post<DefaultServerResponse>(
      `${this.apiUrl}/create`,
      request,
      {
        headers: {
          Authorization: `Bearer ${token}`,
        },
      }
    );
  }

  checkCourseTitle(title: string) {
    const token = localStorage.getItem('Authorization');
    return this.httpClient.get<DefaultServerResponse>(
      `${this.apiUrl}/check-title?title=${title}`,
      {
        headers: {
          Authorization: `Bearer ${token}`,
        },
      }
    );
  }

  getAllCourses() {
    const token = localStorage.getItem('Authorization');
    return this.httpClient.get<DefaultServerResponse>(`${this.apiUrl}/all`, {
      headers: {
        Authorization: `Bearer ${token}`,
      },
    });
  }

  getCourseManagementDashboardInfo(courseId: string) {
    var token = localStorage.getItem('Authorization');
    return this.httpClient.get<DefaultServerResponse>(
      `${this.apiUrl}/info?courseId=${courseId}`,
      {
        headers: {
          Authorization: `Bearer ${token}`,
        },
      }
    );
  }

  getCourseBasics(courseId: string) {
    var token = localStorage.getItem('Authorization');
    return this.httpClient.get<DefaultServerResponse>(
      `${this.apiUrl}/basics?courseId=${courseId}`,
      {
        headers: {
          Authorization: `Bearer ${token}`,
        },
      }
    );
  }

  checkCourseTitleExistEmitGivenCourse(
    request: CheckCourseTitleExistsEmitGivenCourseRequest
  ) {
    var token = localStorage.getItem('Authorization');
    var params = buildHttpParams(request);
    return this.httpClient.get<DefaultServerResponse>(
      `${this.apiUrl}/check-title-except`,
      {
        headers: {
          Authorization: `Bearer ${token}`,
        },
        params: params,
      }
    );
  }

  updateCourseBasics(request: UpdateCourseBasicsRequest) {
    var token = localStorage.getItem('Authorization');
    return this.httpClient.patch<DefaultServerResponse>(
      `${this.apiUrl}/update/basics`,
      request,
      {
        headers: {
          Authorization: `Bearer ${token}`,
        },
      }
    );
  }

  uploadCourseThumbnail(request: UploadCourseThumbnailRequest) {
    var token = localStorage.getItem('Authorization');

    const formData = new FormData();
    formData.append('courseId', request.courseId);
    formData.append('useAzureStorage', String(request.useAzureStorage));
    formData.append('thumbnailData', request.thumbnailData);

    return this.httpClient
      .post<DefaultServerResponse>(
        `${this.apiUrl}/thumbnail/upload`,
        formData,
        {
          headers: {
            Authorization: `Bearer ${token}`,
          },
          reportProgress: true,
          observe: 'events',
        }
      )
      .pipe(
        map((event: HttpEvent<DefaultServerResponse>) => {
          if (event.type === HttpEventType.UploadProgress) {
            const progress = event.total
              ? Math.round((100 * event.loaded) / event.total)
              : 0;
            return { progress, response: null };
          }
          if (event.type === HttpEventType.Response) {
            return { progress: 100, response: event.body };
          }
          return { progress: 0, response: null };
        })
      );
  }

  getCourseThumbnail(courseId: string) {
    var token = localStorage.getItem('Authorization');
    return this.httpClient.get(
      `${this.apiUrl}/thumbnail/get?courseId=${courseId}`,
      {
        headers: {
          Authorization: `Bearer ${token}`,
        },
        responseType: 'blob',
      }
    );
  }

  deleteCourseThumbnail(courseId: string) {
    var token = localStorage.getItem('Authorization');
    return this.httpClient.delete<DefaultServerResponse>(
      `${this.apiUrl}/thumbnail/delete?courseId=${courseId}`,
      {
        headers: {
          Authorization: `Bearer ${token}`,
        },
      }
    );
  }

  uploadCourseTeachingResource(request: UploadCourseTeachingResourceRequest) {
    var token = localStorage.getItem('Authorization');
    const formData = new FormData();
    if (request.courseTeachingResourceId) {
      formData.append(
        'courseTeachingResourceId',
        request.courseTeachingResourceId
      );
    }
    if (request.courseId) {
      formData.append('courseId', request.courseId);
    }
    formData.append('title', request.title);
    formData.append('description', request.description);
    if (request.courseTeachingResourceId) {
      formData.append(
        'courseTeachingResourceId',
        request.courseTeachingResourceId
      );
    }
    if (request.resourceUrl) {
      formData.append('resourceUrl', request.resourceUrl);
    }
    if (request.resourceFile) {
      formData.append('resourceFile', request.resourceFile);
    }

    return this.httpClient
      .post<DefaultServerResponse>(
        `${this.apiUrl}/teaching-resource/upload`,
        formData,
        {
          headers: {
            Authorization: `Bearer ${token}`,
          },
          reportProgress: true,
          observe: 'events',
        }
      )
      .pipe(
        map((event: HttpEvent<DefaultServerResponse>) => {
          if (event.type === HttpEventType.UploadProgress) {
            const progress = event.total
              ? Math.round((100 * event.loaded) / event.total)
              : 0;
            return { progress, response: null };
          }
          if (event.type === HttpEventType.Response) {
            return { progress: 100, response: event.body };
          }
          return { progress: 0, response: null };
        })
      );
  }

  getCourseTeachingResource(courseTeachingResourceId: string) {
    var token = localStorage.getItem('Authorization');
    return this.httpClient.get<DefaultServerResponse>(
      `${this.apiUrl}/teaching-resource/get?courseTeachingResourceId=${courseTeachingResourceId}`,
      {
        headers: {
          Authorization: `Bearer ${token}`,
        },
      }
    );
  }

  getAllCourseTeachingResourcesByCourseId(courseId: string) {
    const token = localStorage.getItem('Authorization');

    return this.httpClient.get<DefaultServerResponse>(
      `${this.apiUrl}/teaching-resource/all?courseId=${courseId}`,
      {
        headers: {
          Authorization: `Bearer ${token}`,
        },
      }
    );
  }

  downloadCourseTeachingResource(resourceId: string) {
    var token = localStorage.getItem('Authorization');
    return this.httpClient.get(
      `${this.apiUrl}/teaching-resource/download?courseTeachingResourceId=${resourceId}`,
      {
        headers: {
          Authorization: `Bearer ${token}`,
        },
        responseType: 'blob',
        reportProgress: true,
        observe: 'events',
      }
    );
  }

  deleteCourseTeachingResource(courseTeachingResourceId: string) {
    var token = localStorage.getItem('Authorization');
    return this.httpClient.delete<DefaultServerResponse>(
      `${this.apiUrl}/teaching-resource/delete?courseTeachingResourceId=${courseTeachingResourceId}`,
      {
        headers: {
          Authorization: `Bearer ${token}`,
        },
      }
    );
  }

  createOrUpdateCourseLesson(request: CreateCourseLessonRequest) {
    const token = localStorage.getItem('Authorization');
    return this.httpClient.post<DefaultServerResponse>(
      `${this.apiUrl}/lesson/create`,
      request,
      {
        headers: {
          Authorization: `Bearer ${token}`,
        },
      }
    );
  }

  getAllCourseLessons(courseId: string) {
    const token = localStorage.getItem('Authorization');
    return this.httpClient.get<DefaultServerResponse>(
      `${this.apiUrl}/lesson/all?courseId=${courseId}`,
      {
        headers: {
          Authorization: `Bearer ${token}`,
        },
      }
    );
  }

  getCourseLessonById(courseLessonId: string) {
    const token = localStorage.getItem('Authorization');

    return this.httpClient.get<DefaultServerResponse>(
      `${this.apiUrl}/lesson?courseLessonId=${courseLessonId}`,
      {
        headers: {
          Authorization: `Bearer ${token}`,
        },
      }
    );
  }

  publishCourseLesson(request: ChangeCourseLessonPublishedStatusRequest) {
    const token = localStorage.getItem('Authorization');
    const params = buildHttpParams(request);
    return this.httpClient.patch<DefaultServerResponse>(
      `${this.apiUrl}/lesson/publish`,
      null,
      {
        headers: {
          Authorization: `Bearer ${token}`,
        },
        params: params,
      }
    );
  }

  archiveCourseLesson(courseLessonId: string) {
    const token = localStorage.getItem('Authorization');

    return this.httpClient.patch(
      `${this.apiUrl}/lesson/archive?courseLessonId=${courseLessonId}`,
      null,
      {
        headers: {
          Authorization: `Bearer ${token}`,
        },
      }
    );
  }

  getAllCourseLessonResources(courseLessonId: string) {
    const token = localStorage.getItem('Authorization');
    return this.httpClient.get<DefaultServerResponse>(
      `${this.apiUrl}/lesson/resource/all?courseLessonId=${courseLessonId}`,
      {
        headers: {
          Authorization: `Bearer ${token}`,
        },
      }
    );
  }

  getCourseLessonResourceById(
    courseLessonResourceId: string
  ): Observable<
    DefaultServerResponse<GetCourseLessonResourceByIdResponse | null>
  > {
    const token = localStorage.getItem('Authorization');
    return this.httpClient.get<
      DefaultServerResponse<GetCourseLessonResourceByIdResponse | null>
    >(
      `${this.apiUrl}/lesson/resource?courseLessonResourceId=${courseLessonResourceId}`,
      {
        headers: {
          Authorization: `Bearer ${token}`,
        },
      }
    );
  }

  downloadCourseLessonResource(resourceId: string) {
    var token = localStorage.getItem('Authorization');
    return this.httpClient.get(
      `${this.apiUrl}/lesson/resource/download?courseLessonResourceId=${resourceId}`,
      {
        headers: {
          Authorization: `Bearer ${token}`,
        },
        responseType: 'blob',
        reportProgress: true,
        observe: 'events',
      }
    );
  }

  uploadCourseLessonResource(request: UploadCourseLessonResourceRequest) {
    const token = localStorage.getItem('Authorization');
    const formData = new FormData();
    if (request.courseLessonResourceId) {
      formData.append('courseLessonResourceId', request.courseLessonResourceId);
    }
    if (request.courseLessonId) {
      formData.append('courseLessonId', request.courseLessonId);
    }
    formData.append('title', request.title);
    formData.append('description', request.description);
    if (request.resourceUrl) {
      formData.append('resourceUrl', request.resourceUrl);
    }
    if (request.file) {
      formData.append('resourceFile', request.file);
    }
    return this.httpClient
      .post<DefaultServerResponse<null>>(
        `${this.apiUrl}/lesson/resource/upload`,
        formData,
        {
          headers: {
            Authorization: `Bearer ${token}`,
          },
          reportProgress: true,
          observe: 'events',
        }
      )
      .pipe(
        map((event: HttpEvent<DefaultServerResponse<null>>) => {
          if (event.type === HttpEventType.UploadProgress) {
            const progress = event.total
              ? Math.round((100 * event.loaded) / event.total)
              : 0;
            return { progress, response: null };
          }
          if (event.type === HttpEventType.Response) {
            return { progress: 100, response: event.body };
          }
          return { progress: 0, response: null };
        })
      );
  }

  deleteCourseLessonResourceById(courseLessonResourceId: string) {
    const token = localStorage.getItem('Authorization');
    return this.httpClient.delete(
      `${this.apiUrl}/lesson/resource?courseLessonResourceId=${courseLessonResourceId}`,
      {
        headers: {
          Authorization: `Bearer ${token}`,
        },
      }
    );
  }

  getPromotionImages(courseId: string) {
    const token = localStorage.getItem('Authorization');
    return this.httpClient.get<
      DefaultServerResponse<GetPromotionImagesResponse[]>
    >(`${this.apiUrl}/promotion/image/all?courseId=${courseId}`, {
      headers: {
        Authorization: `Bearer ${token}`,
      },
    });
  }

  getPromotionImage(coursePromotionImageId: string) {
    const token = localStorage.getItem('Authorization');
    return this.httpClient.get<
      DefaultServerResponse<GetCoursePromotionImageMetadataByIdResponse | null>
    >(`${this.apiUrl}/promotion/image/${coursePromotionImageId}`, {
      headers: {
        Authorization: `Bearer ${token}`,
      },
    });
  }

  uploadCoursePromotionImage(request: UploadCoursePromotionImageRequest) {
    var token = localStorage.getItem('Authorization');

    const formData = new FormData();

    formData.append('courseId', request.courseId as string);

    if (
      request.coursePromotionImageId &&
      request.coursePromotionImageId.trim().length > 0
    ) {
      formData.append('coursePromotionImageId', request.coursePromotionImageId);
    }

    formData.append('image', request.image);

    return this.httpClient
      .post<DefaultServerResponse>(
        `${this.apiUrl}/promotion/image/upload`,
        formData,
        {
          headers: {
            Authorization: `Bearer ${token}`,
          },
          reportProgress: true,
          observe: 'events',
        }
      )
      .pipe(
        map((event: HttpEvent<DefaultServerResponse>) => {
          if (event.type === HttpEventType.UploadProgress) {
            const progress = event.total
              ? Math.round((100 * event.loaded) / event.total)
              : 0;
            return { progress, response: null };
          }
          if (event.type === HttpEventType.Response) {
            return { progress: 100, response: event.body };
          }
          return { progress: 0, response: null };
        })
      );
  }

  deleteCoursePromotionImage(coursePromotionImageId: string) {
    const token = localStorage.getItem('Authorization');
    return this.httpClient.delete<DefaultServerResponse<null>>(
      `${this.apiUrl}/promotion/image/delete/${coursePromotionImageId}`,
      {
        headers: {
          Authorization: `Bearer ${token}`,
        },
      }
    );
  }

  getCourseAnalyticsHistory(courseId: string) {
    const token = localStorage.getItem('Authorization');
    return this.httpClient.get<
      DefaultServerResponse<GetCourseAnalyticsHistoryResponse[]>
    >(`${this.apiUrl}/analytics/history?courseId=${courseId}`, {
      headers: {
        Authorization: `Bearer ${token}`,
      },
    });
  }
}
