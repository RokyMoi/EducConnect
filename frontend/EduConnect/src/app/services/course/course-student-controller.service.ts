import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { GetCoursesByQueryRequest } from '../../models/course/course-student-controller/get-courses-by-query-request';
import { buildHttpParams } from '../../helpers/build-http-params.helper';
import { DefaultServerResponse } from '../../models/shared/default-server-response';
import { GetCoursesByQueryResponse } from '../../models/course/course-student-controller/get-courses-by-query-response';
import ApiLinks from '../../../assets/api/link.api';
import { DefaultServerPaginatedResponse } from '../../models/shared/default-server-paginated-response';
import { AddCourseViewershipDataRequest } from '../../models/course/course-student-controller/add-course-viewership-data-request';

@Injectable({
  providedIn: 'root',
})
export class CourseStudentControllerService {
  constructor(private http: HttpClient) {}

  //Search for course with query and filters with pagination
  public getCoursesByQuery(request: GetCoursesByQueryRequest) {
    const token = localStorage.getItem('Authorization');

    const params = buildHttpParams(request);
    return this.http.get<
      DefaultServerPaginatedResponse<GetCoursesByQueryResponse[]>
    >(`${ApiLinks.CourseStudentControllerUrl}/search`, {
      headers: {
        Authorization: `Bearer ${token}`,
      },
      params: params,
    });
  }

  //Get course student view by course id
  public GetCourseById(courseId: string) {
    const token = localStorage.getItem('Authorization');
    const params = buildHttpParams({ courseId: courseId });
    return this.http.get<DefaultServerResponse<GetCoursesByQueryResponse>>(
      `${ApiLinks.CourseStudentControllerUrl}`,
      {
        headers: {
          Authorization: `Bearer ${token}`,
        },
        params: params,
      }
    );
  }

  public addCourseViewershipData(request: AddCourseViewershipDataRequest) {
    const token = localStorage.getItem('Authorization');
    console.log('Adding course viewership data:', request);
    return this.http.post<DefaultServerResponse>(
      `${ApiLinks.CourseStudentControllerUrl}/analytics`,
      request,
      {
        headers: {
          Authorization: `Bearer ${token}`,
        },
      }
    );
  }

  public setEnteredOnCourseViewershipData() {
    const token = localStorage.getItem('Authorization');
    const viewId = localStorage.getItem('viewId');

    return this.http.patch<DefaultServerResponse>(
      `${ApiLinks.CourseStudentControllerUrl}/analytics/entered?courseViewershipDataId=${viewId}`,
      {},
      {
        headers: {
          Authorization: `Bearer ${token}`,
        },
      }
    );
  }

  public setLeftOnCourseViewershipData() {
    const token = localStorage.getItem('Authorization');
    const viewId = localStorage.getItem('viewId');

    return this.http.patch<DefaultServerResponse>(
      `${ApiLinks.CourseStudentControllerUrl}/analytics/exited?courseViewershipDataId=${viewId}`,
      {},
      {
        headers: {
          Authorization: `Bearer ${token}`,
        },
      }
    );
  }
}
