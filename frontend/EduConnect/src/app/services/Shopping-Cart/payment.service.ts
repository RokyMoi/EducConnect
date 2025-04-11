import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';

interface CheckoutResponse {
  sessionId: string;
}

@Injectable({
  providedIn: 'root'
})
export class PaymentService {
  private baseUrl = 'https://your-backend-api.com/api/payment'; // Zameni pravim URL-om

  constructor(private http: HttpClient) { }

  createCheckoutSession(cartId?: string): Observable<CheckoutResponse> {
    const body = cartId ? { cartId } : {};
    return this.http.post<CheckoutResponse>(`${this.baseUrl}/create-checkout-session`, body);
  }
}
