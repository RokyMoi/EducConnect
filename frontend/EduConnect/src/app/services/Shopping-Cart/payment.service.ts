import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import {catchError, Observable, tap, throwError} from 'rxjs';
import { AccountService } from '../account.service';
import { SnackboxService } from '../shared/snackbox.service';

// Definicija odgovora za checkout
interface CheckoutResponse {
  sessionId: string;
}

@Injectable({
  providedIn: 'root'
})
export class PaymentService {
  // Osnovni URL za API
  private apiUrl = 'http://localhost:5177/api/payment';

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

  // Kreiranje checkout sesije
  createCheckoutSession(cartId?: string): Observable<any> {
    const payload = cartId ? { cartId } : {};

    console.log('Creating checkout session with payload:', payload);

    return this.http.post<any>(`${this.apiUrl}/create-checkout-session`, payload ,{
      headers: this.getAuthHeaders()
    })
      .pipe(
        tap(response => console.log('Checkout session created:', response)),
        catchError(error => {
          console.error('Error creating checkout session:', error);
          return throwError(() => new Error(error.error?.message || 'Failed to create checkout session'));
        })
      );
  }

  /**
   * Checks the status of a payment by session ID
   * @param sessionId - The Stripe session ID to check
   * @returns Observable with payment status information
   */
  checkPaymentStatus(sessionId: string): Observable<any> {
    return this.http.get<any>(`${this.apiUrl}/payment-status/${sessionId}`,{
      headers: this.getAuthHeaders()
    })
      .pipe(
        tap(response => console.log('Payment status:', response)),
        catchError(error => {
          console.error('Error checking payment status:', error);
          return throwError(() => new Error(error.error?.message || 'Failed to check payment status'));
        })
      );
  }
}
