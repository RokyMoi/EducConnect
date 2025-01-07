import { HttpClient } from '@angular/common/http';
import { inject, Injectable } from '@angular/core';
import { CreateCourseBasicInformation } from '../../_models/course/create-course/create-course.create-course.course.model';
import { AccountService } from '../account.service';
import { CreateCourseHttpRequestBody } from '../../_models/course/create-course/create-course.http-request-body';
import ApiLinks from '../../../assets/api/link.api';
import { catchError, map, of } from 'rxjs';

@Injectable({
  providedIn: 'root',
})
export class CourseCreateService {
  http = inject(HttpClient);
  accountService = inject(AccountService);
  constructor() {}

  public createCourseBasicInformation(
    courseToCreate: CreateCourseBasicInformation
  ) {
    const authorization = this.accountService.getAccessToken();
    const requestBody: CreateCourseHttpRequestBody = {
      courseName: courseToCreate.courseName,
      courseSubject: courseToCreate.courseSubject,
      courseDescription: courseToCreate.courseDescription,
      learningSubcategoryId: courseToCreate.learningSubcategoryId,
      learningDifficultyLevelId: courseToCreate.learningDifficultyLevelId,
      IsDraft: true,
      courseTypeId: courseToCreate.courseTypeId,
      coursePrice: courseToCreate.coursePrice,
    };

    return this.http
      .post(ApiLinks.addCourseBasicInformation, requestBody, {
        headers: {
          Authorization: `Bearer ${authorization}`,
        },
      })
      .pipe(
        map((response) => {
          return {
            success: (response as any).success,
            data: (response as any).data,
            message: (response as any).message,
            statusCode: (response as any).statusCode,
          };
        }),
        catchError((error) => {
          return of({
            success: (error as any).error.success,
            data: (error as any).error.data,
            message: (error as any).error.message,
            statusCode: (error as any).status,
          });
        })
      );
  }
}
