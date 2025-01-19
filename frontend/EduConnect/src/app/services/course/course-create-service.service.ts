import {
  HttpClient,
  HttpEvent,
  HttpEventType,
  HttpHeaders,
  HttpRequest,
} from '@angular/common/http';
import { inject, Injectable } from '@angular/core';
import { CreateCourseBasicInformation } from '../../_models/course/create-course/create-course.create-course.course.model';
import { AccountService } from '../account.service';
import { CreateCourseHttpRequestBody } from '../../_models/course/create-course/create-course.http-request-body';
import ApiLinks from '../../../assets/api/link.api';
import { catchError, map, of, tap } from 'rxjs';
import { EventType } from '@angular/router';

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

    //Create the HttpRequest object to send as the http post request to the server
    const requestToSend = new HttpRequest(
      'POST',
      ApiLinks.uploadCourseMainMaterial,
      formData,
      {
        headers: new HttpHeaders({
          Authorization: `Bearer ${authorization}`,
        }),
        reportProgress: true,
      }
    );

    //Send the http request to the server and return either the upload progress or the response from the server
    return this.http.request(requestToSend).pipe(
      map((event) => {
        //Handle the case where the response is still being sent to the server
        if (event.type === HttpEventType.UploadProgress && event.total) {
          const progress = Math.round((100 * event.loaded) / event.total);
          return progress;
        }
        //Handle the case where the response is received from the server
        if (event.type == HttpEventType.Response) {
          const response = event.body;
          return {
            success: (response as any).success,
            data: (response as any).data,
            message: (response as any).message,
            statusCode: event.status,
          };
        }

        //Fallback return
        return 0;
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

  public downloadCourseMainMaterialByCourseMainMaterialId(
    courseMainMaterialId: string
  ) {
    const authorization = this.accountService.getAccessToken();
    return this.http
      .get(ApiLinks.downloadCourseMainMaterial + '/' + courseMainMaterialId, {
        headers: {
          Authorization: `Bearer ${authorization}`,
        },
        responseType: 'blob', // Set the response type to 'blob' for file downloads
        reportProgress: true, // Enable progress reporting
        observe: 'events', // Observe the events to track progress of the file download
      })
      .pipe(
        map((event: HttpEvent<any>) => {
          //Check if the event type is HttpEventType.DownloadProgress
          if (event.type === HttpEventType.DownloadProgress) {
            //Calculate and return the progress percentage
            if (event.total) {
              return Math.round((event.loaded / event.total) * 100);
            }
            return 0; //Fallback return if the progress cannot be calculated
          }

          //Handle the case where the response is received from the server
          if (event.type === HttpEventType.Response) {
            return event.body;
          }

          //Fallback return
          return 0;
        }),
        tap((progressOrFile) => {
          if (typeof progressOrFile === 'number') {
            console.log('Download progress:', progressOrFile);
          }
          if (progressOrFile instanceof Blob) {
            console.log('Download completed:', progressOrFile);
          }
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

  public deleteCourseMainMaterialByCourseMainMaterialId(
    courseMainMaterialId: string
  ) {
    const authorization = this.accountService.getAccessToken();
    return this.http
      .delete(ApiLinks.deleteCourseMainMaterial + '/' + courseMainMaterialId, {
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

  public updateCourseTypeByCourseId(courseId: string, courseTypeId: number) {
    const authorization = this.accountService.getAccessToken();
    console.log('Authorization: ', authorization);

    return this.http
      .patch(
        ApiLinks.updateCourseTypeByCourseId +
          '/' +
          courseId +
          '/' +
          courseTypeId,
        {},
        {
          headers: {
            Authorization: `Bearer ${authorization}`,
          },
        }
      )
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

  public createCourseLesson(
    courseId: string,
    lessonTitle: string,
    lessonDescription: string,
    lessonSequenceOrder: number,
    lessonPrerequisites: string,
    lessonObjective: string,
    lessonCompletionTimeInMinutes: number,
    lessonTag: string
  ) {
    const authorization = this.accountService.getAccessToken();

    const requestBody = {
      courseId: courseId,
      lessonTitle: lessonTitle,
      lessonDescription: lessonDescription,
      lessonSequenceOrder: lessonSequenceOrder,
      lessonPrerequisites: lessonPrerequisites,
      lessonObjective: lessonObjective,
      lessonCompletionTimeInMinutes: lessonCompletionTimeInMinutes,
      lessonTag: lessonTag,
    };

    return this.http
      .post(ApiLinks.createCourseLesson, requestBody, {
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

  public createCourseLessonContent(
    courseLessonId: string,
    courseLessonContentTitle: string,
    courseLessonContentDescription: string,
    courseLessonContent: string
  ) {
    const authorization = this.accountService.getAccessToken();
    const requestBody = {
      courseLessonId,
      title: courseLessonContentTitle,
      description: courseLessonContentDescription,
      contentType: 'string',
      contentData: courseLessonContent,
    };

    return this.http
      .post(ApiLinks.createCourseLessonContent, requestBody, {
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

  public uploadFileAsCourseLessonSupplementaryMaterial(
    courseLessonId: string,
    fileName: string,
    dateTimePointOfFileCreation: string,
    fileToUpload: File
  ) {
    const authorization = this.accountService.getAccessToken();
    const formData = new FormData();
    formData.append('FileToUpload', fileToUpload);
    formData.append('FileName', fileName);
    formData.append('DateTimePointOfFileCreation', dateTimePointOfFileCreation);
    formData.append('courseLessonId', courseLessonId);

    //Create the HttpRequest object to send as the http post request to the server
    const requestToSend = new HttpRequest(
      'POST',
      ApiLinks.uploadCourseLessonSupplementaryMaterial,
      formData,
      {
        headers: new HttpHeaders({
          Authorization: `Bearer ${authorization}`,
        }),
        reportProgress: true,
      }
    );

    //Send the http request to the server and return either the upload progress or the response from the server
    return this.http.request(requestToSend).pipe(
      map((event) => {
        //Handle the case where the response is still being sent to the server
        if (event.type === HttpEventType.UploadProgress && event.total) {
          const progress = Math.round((100 * event.loaded) / event.total);
          return progress;
        }
        //Handle the case where the response is received from the server
        if (event.type == HttpEventType.Response) {
          const response = event.body;
          return {
            success: (response as any).success,
            data: (response as any).data,
            message: (response as any).message,
            statusCode: event.status,
          };
        }

        //Fallback return
        return 0;
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

  public getCourseLessonSupplementaryMaterialsByCourseLessonId(
    courseLessonId: string
  ) {
    const authorization = this.accountService.getAccessToken();
    return this.http
      .get(
        ApiLinks.getCourseLessonSupplementaryMaterial + '/' + courseLessonId,
        {
          headers: {
            Authorization: `Bearer ${authorization}`,
          },
        }
      )
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

  public downloadCourseLessonSupplementaryMaterialByCourseLessonSupplementaryMaterialId(
    courseLessonSupplementaryMaterialId: string
  ) {
    const authorization = this.accountService.getAccessToken();

    return this.http
      .get(
        ApiLinks.downloadCourseLessonSupplementaryMaterial +
          '/' +
          courseLessonSupplementaryMaterialId,
        {
          headers: {
            Authorization: `Bearer ${authorization}`,
          },
          responseType: 'blob',
          reportProgress: true,
          observe: 'events',
        }
      )
      .pipe(
        map((event: HttpEvent<any>) => {
          //Check if the event type is HttpEventType.DownloadProgress
          if (event.type === HttpEventType.DownloadProgress) {
            //Calculate and return the progress percentage
            if (event.total) {
              return Math.round((event.loaded / event.total) * 100);
            }
            return 0; //Fallback return if the progress cannot be calculated
          }

          //Handle the case where the response is received from the server
          if (event.type === HttpEventType.Response) {
            return event.body;
          }

          //Fallback return
          return 0;
        }),
        tap((progressOrFile) => {
          if (typeof progressOrFile === 'number') {
            console.log('Download progress:', progressOrFile);
          }
          if (progressOrFile instanceof Blob) {
            console.log('Download completed:', progressOrFile);
          }
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

  public deleteCourseLessonSupplementaryMaterialByCourseLessonSupplementaryMaterialId(
    courseLessonSupplementaryMaterialId: string
  ) {
    const authorization = this.accountService.getAccessToken();
    return this.http
      .delete(
        ApiLinks.deleteCourseLessonSupplementaryMaterial +
          '/' +
          courseLessonSupplementaryMaterialId,
        {
          headers: {
            Authorization: `Bearer ${authorization}`,
          },
        }
      )
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
