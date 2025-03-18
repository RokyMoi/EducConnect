import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import ApiLinks from '../../../assets/api/link.api';
import { CreateCourseRequest } from '../../models/course/course-tutor-controller/create-course-request';
import { DefaultServerResponse } from '../../models/shared/default-server-response';

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
}
