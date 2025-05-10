import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders, HttpParams } from '@angular/common/http';
import { AccountService } from '../account.service';
import { Observable } from 'rxjs';

export interface PaginatedResponse<T> {
  totalCount: number;
  pageSize: number;
  currentPage: number;
  totalPages: number;
  data: T[];
}

export interface CourseItem {
  courseId: number;
  title: string;
  description: string;
  thumbnailUrl: string | null;
  category: string;
}

@Injectable({
  providedIn: 'root'
})
export class MyCoursesService {
  private apiLink = "http://localhost:5177/Info/my-courses";

  constructor(
    private http: HttpClient,
    private accountService: AccountService
  ) {}

  private getAuthHeaders(): HttpHeaders {
    const token = this.accountService.getAccessToken();
    return new HttpHeaders({
      'Authorization': `Bearer ${token}`,
      'Content-Type': 'application/json'
    });
  }

  getMyCourses(pageNumber: number = 1, pageSize: number = 5): Observable<PaginatedResponse<CourseItem>> {
    let params = new HttpParams()
      .set('pageNumber', pageNumber.toString())
      .set('pageSize', pageSize.toString());

    return this.http.get<PaginatedResponse<CourseItem>>(this.apiLink, {
      headers: this.getAuthHeaders(),
      params: params
    });
  }
}
