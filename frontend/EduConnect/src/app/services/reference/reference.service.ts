import { inject, Injectable } from '@angular/core';
import ApiLinks from '../../../assets/api/link.api';
import { HttpClient } from '@angular/common/http';
import { catchError, map, of } from 'rxjs';
import { DefaultServerResponse } from '../../models/shared/default-server-response';

@Injectable({
  providedIn: 'root',
})
export class ReferenceService {
  http = inject(HttpClient);
  constructor() {}

  getEmploymentTypes() {
    return this.http.get(ApiLinks.getAllEmploymentTypes).pipe(
      map((response) => {
        return {
          success: (response as any).success,
          data: (response as any).data.employmentType,
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

  GetIndustryClassifications() {
    return this.http.get(ApiLinks.getAllIndustryClassifications).pipe(
      map((response) => {
        return {
          success: (response as any).success,
          data: (response as any).data.industryClassification,
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

  GetWorkTypes() {
    return this.http.get(ApiLinks.getAllWorkTypes).pipe(
      map((response) => {
        return {
          success: (response as any).success,
          data: (response as any).data.workType,
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

  GetTutorTeachingStyleTypes() {
    return this.http.get(ApiLinks.getAllTutorTeachingStyleTypes).pipe(
      map((response) => {
        return {
          success: (response as any).success,
          data: (response as any).data.tutorTeachingStyleType,
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

  GetCommunicationTypes() {
    return this.http.get(ApiLinks.getAllCommunicationTypes).pipe(
      map((response) => {
        return {
          success: (response as any).success,
          data: (response as any).data.communicationType,
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

  GetEngagementMethods() {
    return this.http.get(ApiLinks.getAllEngagementMethods).pipe(
      map((response) => {
        return {
          success: (response as any).success,
          data: (response as any).data.engagementMethod,
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

  getLearningCategoriesAndSubcategories() {
    return this.http
      .get(ApiLinks.getAllLearningCategoriesAndSubcategories)
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

  getAllLearningDifficultyLevels() {
    return this.http.get(ApiLinks.getAllLearningDifficultyLevels).pipe(
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

  getAllCourseTypes() {
    return this.http.get(ApiLinks.getAllCourseTypes).pipe(
      map((response) => {
        return {
          success: (response as any).success,
          data: (response as any).data.courseType,
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

  getAllLanguages() {
    return this.http.get(ApiLinks.getAllLanguages).pipe(
      map((response) => {
        return {
          success: (response as any).success,
          data: (response as any).data.language,
          message: (response as any).message,
          statusCode: (response as any).status,
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

  getAllCourseCategories() {
    return this.http.get<DefaultServerResponse>(
      ApiLinks.ReferenceControllerUrl + '/course/category/all'
    );
  }
}
