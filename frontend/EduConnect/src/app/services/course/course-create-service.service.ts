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
      price: courseToCreate.coursePrice,
    };
    console.log('Request body for creating course', requestBody);
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

  public addSupportedLanguage(courseId: string, languageId: string) {
    const authorization = this.accountService.getAccessToken();
    const requestBody = {
      courseId: courseId,
      languageId: languageId,
    };
    return this.http
      .post(ApiLinks.addLanguageSupportToCourse, requestBody, {
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
            statusCode: (response as any).status,
          };
        }),
        catchError((error) => {
          console.log(error);
          return of({
            success: (error as any).error.success,
            data: (error as any).error.data,
            message: (error as any).error.message,
            statusCode: (error as any).status,
          });
        })
      );
  }

  getCourseBasicInformation(courseId: string) {
    const authorization = this.accountService.getAccessToken();

    return this.http
      .get(`${ApiLinks.getCourseBasicInformation}/${courseId}`, {
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

  getLanguagesSupportedByCourse(courseId: string) {
    const authorization = this.accountService.getAccessToken();
    return this.http
      .get(`${ApiLinks.getCourseSupportedLanguages}${courseId}`, {
        headers: {
          Authorization: `Bearer ${authorization}`,
        },
      })
      .pipe(
        map((response) => {
          return {
            success: (response as any).success,
            data: (response as any).data.supportedLanguages,
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

  removeSupportedLanguage(courseId: string, languageId: string) {
    const authorization = this.accountService.getAccessToken();
    const requestBody = {
      courseId: courseId,
      languageId: languageId,
    };
    return this.http
      .delete(ApiLinks.deleteLanguageSupportFromCourse, {
        headers: {
          Authorization: `Bearer ${authorization}`,
        },
        body: requestBody,
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

  public uploadFileAsCourseMainMaterial(
    courseId: string,
    fileName: string,
    dateTimePointOfFileCreation: string,
    fileToUpload: File
  ) {
    const authorization = this.accountService.getAccessToken();
    const formData = new FormData();
    formData.append('FileToUpload', fileToUpload);
    formData.append('FileName', fileName);
    formData.append('DateTimePointOfFileCreation', dateTimePointOfFileCreation);
    formData.append('CourseId', courseId);

    return this.http
      .post(ApiLinks.uploadCourseMainMaterial, formData, {
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

  public getCourseMainMaterials(courseId: string) {
    const authorization = this.accountService.getAccessToken();
    return this.http
      .get(ApiLinks.getCourseMainMaterials + '/' + courseId, {
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
