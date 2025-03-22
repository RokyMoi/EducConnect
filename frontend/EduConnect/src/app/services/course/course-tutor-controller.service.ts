import { HttpClient, HttpEvent, HttpEventType } from '@angular/common/http';
import { Injectable } from '@angular/core';
import ApiLinks from '../../../assets/api/link.api';
import { CreateCourseRequest } from '../../models/course/course-tutor-controller/create-course-request';
import { DefaultServerResponse } from '../../models/shared/default-server-response';
import { CheckCourseTitleExistsEmitGivenCourseRequest } from '../../models/course/course-tutor-controller/check-course-title-exists-emit-given-course-request';
import { buildHttpParams } from '../../helpers/build-http-params.helper';
import { UpdateCourseBasicsRequest } from '../../models/course/course-tutor-controller/update-course-basics-request';
import { UploadCourseThumbnailRequest } from '../../models/course/course-tutor-controller/upload-course-thumbnail-request';
import { map } from 'rxjs';

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
}
