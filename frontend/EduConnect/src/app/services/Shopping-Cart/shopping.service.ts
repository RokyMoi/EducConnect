import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Observable } from 'rxjs';
import { AccountService } from '../account.service';
import { SnackboxService } from '../shared/snackbox.service';

@Injectable({
  providedIn: 'root'
})
export class ShoppingCartService {
  private apiUrl = 'http://localhost:5177/api/shoppingcart';

  constructor(
    private http: HttpClient,
    private accountService: AccountService,
    private snackBarService: SnackboxService
  ) {}

  private getAuthHeaders(): HttpHeaders {
    const token = this.accountService.getAccessToken();
    return new HttpHeaders({
      'Authorization': `Bearer ${token}`,
      'Content-Type': 'application/json'
    });
  }

  getShoppingCart(): Observable<any> {
    return this.http.get(this.apiUrl, {
      headers: this.getAuthHeaders()
    });
  }

  createShoppingCart(): Observable<any> {
    return this.http.post(`${this.apiUrl}/create`, {}, {
      headers: this.getAuthHeaders()
    });
  }

  addCourseToCart(courseId: string): Observable<boolean> {
    return this.http.post<boolean>(`${this.apiUrl}/add/${courseId}`, {}, {
      headers: this.getAuthHeaders()
    });
  }

  loadShoppingCart()   {
    return this.http.get(`http://localhost:5177/loadShopping`, {
      headers: this.getAuthHeaders()
    });
  }

  removeCourseFromCart(courseId: string): Observable<boolean> {
    return this.http.delete<boolean>(`${this.apiUrl}/remove/${courseId}`, {
      headers: this.getAuthHeaders()
    });
  }

  moveCourseToWishlist(courseId: string): Observable<any> {
    return this.http.post<any>(`${this.apiUrl}/move-to-wishlist/${courseId}`, {}, {
      headers: this.getAuthHeaders()
    });
  }

  clearShoppingCart(): Observable<boolean> {
    return this.http.delete<boolean>(`${this.apiUrl}/clear`, {
      headers: this.getAuthHeaders()
    });
  }

  getTotalPrice(): Observable<number> {
    return this.http.get<number>(`${this.apiUrl}/total-price`, {
      headers: this.getAuthHeaders()
    });
  }

  getItemCount(): Observable<number> {
    return this.http.get<number>(`${this.apiUrl}/count`, {
      headers: this.getAuthHeaders()
    });
  }

  checkIfCourseInCart(courseId: string): Observable<boolean> {
    return this.http.get<boolean>(`${this.apiUrl}/contains/${courseId}`, {
      headers: this.getAuthHeaders()
    });
  }
}
