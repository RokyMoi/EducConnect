import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Observable } from 'rxjs';
import { AccountService } from '../account.service';
import { SnackboxService } from '../shared/snackbox.service';

export interface EnrollmentStatus {
  buttonLabel: string;
  price: number;
}
export interface CourseEnrollRequest
{
  courseId:string;
}

@Injectable({
  providedIn: 'root'
})

export class EnrollmentService {
  private baseUrl = 'http://localhost:5177/api/Enrollment';

  constructor(
    private http: HttpClient,
    private accountService: AccountService,
    private snackBarService: SnackboxService
  ) {}

  // Private helper method to get headers with auth token
  private getAuthHeaders(): HttpHeaders {
    const token = this.accountService.getAccessToken();
    return new HttpHeaders({
      'Authorization': `Bearer ${token}`,
      'Content-Type': 'application/json'
    });
  }

  getEnrollmentStatus(courseId: string): Observable<EnrollmentStatus> {
    return this.http.get<EnrollmentStatus>(`http://localhost:5177/api/Enrollment/${courseId}/enrollment-status`, {
      headers: this.getAuthHeaders()
    });
  }

  enrollCourse(courseId: string): Observable<any> {
    //
    const requestBody = { courseId: courseId };

    // Use the same URL pattern as the working method
    return this.http.post<CourseEnrollRequest>(
      `http://localhost:5177/api/Enrollment/enroll`,
      requestBody,
      {
        headers: this.getAuthHeaders()
      }
    );
  }
  debugEnrollment(courseId: string): Observable<any> {
    const requestBody = { CourseId: courseId };

    return this.http.post(
      `http://localhost:5177/api/Enrollment/debug`,
      requestBody,
      {
        headers: this.getAuthHeaders()
      }
    );
  }
}
