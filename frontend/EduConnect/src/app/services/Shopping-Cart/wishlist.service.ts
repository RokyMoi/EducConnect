import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Observable } from 'rxjs';
import { AccountService } from '../account.service';
import {SnackboxService} from '../shared/snackbox.service';

@Injectable({
  providedIn: 'root'
})
export class WishlistService {
  private baseUrl = 'http://localhost:5177/api/wishlist';

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
  CoursePrint():Observable<any>{
    return this.http.get<any>(`http://localhost:5177/UcitajKurseve`, {
      headers: this.getAuthHeaders()
    });
  }

  /**
   * Gets the wishlist for the current user
   */
  getWishlist(): Observable<any> {
    return this.http.get<any>(`${this.baseUrl}`, {
      headers: this.getAuthHeaders()
    });
  }


  addCourseToWishlist(courseId: string): Observable<any> {
    return this.http.post<any>(`${this.baseUrl}/courses/${courseId}`, {}, {
      headers: this.getAuthHeaders()
    });
  }


  removeCourseFromWishlist(courseId: string): Observable<any> {
    return this.http.delete<any>(`${this.baseUrl}/courses/${courseId}`, {
      headers: this.getAuthHeaders()
    });
  }

  /**
   * Checks if a course is in the wishlist
   */
  isCourseInWishlist(courseId: string): Observable<boolean> {
    return this.http.get<boolean>(`${this.baseUrl}/courses/${courseId}/exists`, {
      headers: this.getAuthHeaders()
    });
  }

  /**
   * Gets the number of items in the wishlist
   */
  getWishlistItemCount(): Observable<number> {
    return this.http.get<number>(`${this.baseUrl}/count`, {
      headers: this.getAuthHeaders()
    });
  }

  /**
   * Clears the entire wishlist
   */
  clearWishlist(): Observable<any> {
    return this.http.delete<any>(`${this.baseUrl}/clear`, {
      headers: this.getAuthHeaders()
    });
  }
}
