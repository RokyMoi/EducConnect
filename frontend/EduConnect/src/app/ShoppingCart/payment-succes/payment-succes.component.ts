import { CommonModule } from '@angular/common';
import { Component, OnInit, NgZone } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { PaymentService } from '../../services/Shopping-Cart/payment.service';
import { SnackboxService } from '../../services/shared/snackbox.service';

@Component({
  selector: 'app-payment-success',
  standalone: true,
  imports: [
    CommonModule
  ],
  templateUrl: './payment-succes.component.html',
  styleUrls: ['./payment-succes.component.css']
})
export class PaymentSuccessComponent implements OnInit {
  // Payment status tracking
  isLoading = true;
  paymentSuccessful = false;
  errorMessage: string | null = null;

  // Payment information
  sessionId: string | null = null;
  paymentDetails: any = null;

  constructor(
    private route: ActivatedRoute,
    private router: Router,
    private paymentService: PaymentService,
    private snackboxService: SnackboxService,
    private ngZone: NgZone
  ) {}

  /**
   * Initialize component and verify payment status
   */
  ngOnInit(): void {
    // Extract the session ID from URL query parameters
    this.route.queryParams.subscribe(params => {
      this.sessionId = params['session_id'];

      if (this.sessionId) {
        this.verifyPaymentStatus(this.sessionId);
      } else {
        this.handleError('No session ID found. Invalid payment confirmation.');
      }
    });
  }

  /**
   * Verify payment status with backend
   * @param sessionId Stripe session ID
   */
  private verifyPaymentStatus(sessionId: string): void {
    this.isLoading = true;

    this.paymentService.checkPaymentStatus(sessionId).subscribe({
      next: (response) => {
        console.log('Payment verification response:', response);

        // Process based on payment status
        if (response.paymentStatus === 'paid') {
          this.handlePaymentSuccess(response);
        } else if (response.paymentStatus === 'unpaid') {
          this.handleError('Payment was not completed. Please try again or contact support.');
        } else {
          this.handleError('Payment status is pending or unknown. We will notify you when confirmed.');
        }
      },
      error: (error) => {
        console.error('Payment verification error:', error);
        this.handleError('Error verifying payment status. Please contact support.');
      }
    });
  }

  /**
   * Handle successful payment
   * @param paymentDetails Payment details from API
   */
  private handlePaymentSuccess(paymentDetails: any): void {
    this.ngZone.run(() => {
      this.isLoading = false;
      this.paymentSuccessful = true;
      this.paymentDetails = paymentDetails;

      // Show success message
      this.snackboxService.showSnackbox('Payment completed successfully!');
    });
  }

  /**
   * Handle payment errors
   * @param message Error message to display
   */
  private handleError(message: string): void {
    this.ngZone.run(() => {
      this.isLoading = false;
      this.paymentSuccessful = false;
      this.errorMessage = message;

      // Show error notification
      this.snackboxService.showSnackbox(message);
    });
  }

  /**
   * Navigate to user's courses
   */
  goToCourses(): void {
    this.router.navigate(['/viewOfAllCourses']);
  }

  /**
   * Navigate to shopping cart
   */
  goToCart(): void {
    this.router.navigate(['/cart-dwe']);
  }

  /**
   * Navigate to home
   */
  goToHome(): void {
    this.router.navigate(['/student-dashboard']);
  }
}
