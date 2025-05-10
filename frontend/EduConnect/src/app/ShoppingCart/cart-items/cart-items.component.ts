import { Component, inject, OnInit, NgZone, PLATFORM_ID, Inject } from '@angular/core';
import { Router } from '@angular/router';
import { PaymentService } from '../../services/Shopping-Cart/payment.service';
import { NgForOf } from '@angular/common';
import { ShoppingCartService } from '../../services/Shopping-Cart/shopping.service';
import { SnackboxService } from '../../services/shared/snackbox.service';
import { isPlatformBrowser } from '@angular/common';

/**
 * Stripe declaration for TypeScript compatibility
 * Ensures the global Stripe object is recognized by TypeScript
 */
declare var Stripe: any;

@Component({
  selector: 'app-cart-items',
  standalone: true,
  imports: [
    NgForOf
  ],
  templateUrl: './cart-items.component.html',
  styleUrls: ['./cart-items.component.css']
})
export class CartItemsComponent implements OnInit {
  /** Stores loaded courses in the shopping cart */
  kursevi: any;

  /** Holds the total price of items in cart */
  totalprice: number = 0;

  /** Router instance for navigation */
  private router = inject(Router);

  /** Stripe instance reference */
  private stripe: any = null;

  /** Flag to track if Stripe is properly loaded */
  private isStripeLoaded: boolean = false;

  /** Loading state for checkout button */
  isProcessingCheckout: boolean = false;

  /**
   * Component constructor
   * @param shoppingCartService - Service for shopping cart operations
   * @param snackboxService - Service for displaying notifications
   * @param paymentService - Service for payment processing
   * @param ngZone - Angular zone service for handling external callbacks
   */
  constructor(
    public shoppingCartService: ShoppingCartService,
    public snackboxService: SnackboxService,
    private paymentService: PaymentService,
    private ngZone: NgZone,
    @Inject(PLATFORM_ID) private platformId: Object
  ) {}

  /**
   * Lifecycle hook that initializes the component
   * Loads shopping cart data and initializes Stripe
   */
  ngOnInit(): void {
    // Load cart items and calculate total
    this.LoadShoppingCrt();
    this.UcitajTotal();

    // Initialize Stripe safely only in browser context
    if (isPlatformBrowser(this.platformId)) {
      // Use setTimeout to ensure DOM is fully loaded
      setTimeout(() => {
        this.initializeStripe();
      }, 500);
    } else {
      console.log('Not initializing Stripe in server-side rendering');
    }
  }

  /**
   * Initializes the Stripe payment processor
   * Handles errors gracefully with detailed logging
   * Uses dynamic script loading if needed
   */
  private initializeStripe(): void {
    try {
      // First check if Stripe is already available
      if (typeof Stripe !== 'undefined') {
        this.configureStripe();
        return;
      }

      // If not available, check if script is already in the DOM but not yet executed
      const existingScript = document.querySelector('script[src*="stripe.com/v3"]');
      if (existingScript) {
        console.log('Stripe script found in DOM but not loaded yet. Waiting for load...');
        existingScript.addEventListener('load', () => {
          console.log('Stripe script loaded from existing script tag');
          this.ngZone.run(() => {
            this.configureStripe();
          });
        });
        return;
      }

      // If script is not in the DOM, add it dynamically
      console.log('Stripe script not found, adding dynamically');
      const script = document.createElement('script');
      script.src = 'https://js.stripe.com/v3/';
      script.async = true;
      script.onload = () => {
        console.log('Stripe script loaded dynamically');
        this.ngZone.run(() => {
          this.configureStripe();
        });
      };
      script.onerror = (error) => {
        this.ngZone.run(() => {
          this.handleStripeInitError('Script loading error', 'Failed to load Stripe.js dynamically');
        });
      };
      document.head.appendChild(script);
    } catch (error) {
      // Handle initialization errors
      this.handleStripeInitError('Stripe initialization error', error);
    }
  }

  /**
   * Configures the Stripe instance once the script is loaded
   * Separated from initialization to support async loading
   */
  private configureStripe(): void {
    try {
      if (typeof Stripe !== 'undefined') {
        // Initialize with your publishable key
        this.stripe = Stripe("pk_test_51QfCdARqiD42JnqA55ry377aEzFx4zUp5fbq1GHqGOAhtl6BcC2vt8EcOkihvaBhKR95XIwFEmhR7MwfVJlUnV6R00gMAhR2hp");
        this.isStripeLoaded = true;
        console.log('Stripe successfully initialized');
      } else {
        this.handleStripeInitError('Stripe configuration error', 'Stripe object not available after script load');
      }
    } catch (error) {
      this.handleStripeInitError('Stripe configuration error', error);
    }
  }

  /**
   * Handles errors during Stripe initialization
   * @param errorType - Type of error that occurred
   * @param errorDetails - Detailed error information
   */
  private handleStripeInitError(errorType: string, errorDetails: any): void {
    this.isStripeLoaded = false;
    console.error(`${errorType}:`, errorDetails);
    // We don't show error to user immediately as they might not use checkout
  }

  /**
   * Special initialization method that proceeds to checkout after initializing
   * Used when checkout is attempted before Stripe is ready
   */
  private initializeStripeAndProceed(): void {
    // Set up a retry mechanism with timeout
    let retryCount = 0;
    const maxRetries = 3;

    const attemptInit = () => {
      try {
        if (typeof Stripe !== 'undefined') {
          this.configureStripe();

          // If initialization succeeded, proceed with checkout
          if (this.isStripeLoaded && this.stripe) {
            console.log('Stripe initialized successfully, proceeding with checkout');
            // Short delay to ensure Stripe is fully ready
            setTimeout(() => {
              this.ngZone.run(() => {
                this.proceedWithCheckout();
              });
            }, 300);
          } else {
            retryOrFail();
          }
        } else {
          retryOrFail();
        }
      } catch (error) {
        retryOrFail();
      }
    };

    const retryOrFail = () => {
      retryCount++;
      if (retryCount < maxRetries) {
        console.log(`Retrying Stripe initialization (${retryCount}/${maxRetries})...`);
        setTimeout(attemptInit, 1000); // Wait 1 second between retries
      } else {
        console.error('Failed to initialize Stripe after multiple attempts');
        this.ngZone.run(() => {
          this.snackboxService.showSnackbox("Payment system unavailable. Please try again later.");
        });
      }
    };

    // Start the initialization process
    console.log('Starting Stripe initialization for checkout');
    this.initializeStripe();
    setTimeout(attemptInit, 500);
  }

  /**
   * Executes the actual checkout process after verifying Stripe is ready
   * Separated from onCheckout to support retry logic
   */
  private proceedWithCheckout(): void {
    // Set processing state
    this.isProcessingCheckout = true;
    this.snackboxService.showSnackbox("Preparing your payment...");
    console.log('Creating checkout session');

    // Call payment service to create checkout session
    this.paymentService.createCheckoutSession().subscribe({
      next: (response: any) => {
        if (!response || !response.sessionId) {
          this.handleCheckoutError('Invalid session response', 'Missing session ID in response');
          return;
        }

        console.log('Checkout session created successfully:', response.sessionId);

        // Store Stripe publishable key if provided by backend
        const publishableKey = response.publishableKey || "pk_test_51QfCdARqiD42JnqA55ry377aEzFx4zUp5fbq1GHqGOAhtl6BcC2vt8EcOkihvaBhKR95XIwFEmhR7MwfVJlUnV6R00gMAhR2hp";

        // Ensure Stripe is initialized with correct key
        try {
          if (!this.stripe) {
            this.stripe = Stripe(publishableKey);
            this.isStripeLoaded = true;
            console.log('Stripe initialized with publishable key from backend');
          }
        } catch (error) {
          console.error('Failed to initialize Stripe with backend key:', error);
        }

        // Navigate to intermediate page before redirecting to Stripe
        this.router.navigateByUrl("/cart-dwe").then(() => {
          console.log('Redirecting to Stripe checkout');

          // Redirect to Stripe checkout within Angular zone to ensure proper handling
          this.ngZone.run(() => {
            this.stripe.redirectToCheckout({ sessionId: response.sessionId })
              .then((result: any) => {
                if (result.error) {
                  this.handleCheckoutError('Stripe redirect error', result.error.message);
                }
              })
              .catch((error: any) => {
                this.handleCheckoutError('Stripe checkout exception', error);
              })
              .finally(() => {
                this.isProcessingCheckout = false;
              });
          });
        }).catch(navError => {
          this.handleCheckoutError('Navigation error', navError);
        });
      },
      error: (error: any) => {
        this.handleCheckoutError('Checkout session creation failed', error);
      }
    });
  }

  /**
   * Loads shopping cart items from the service
   * Handles API responses and errors
   */
  LoadShoppingCrt(): void {
    this.shoppingCartService.loadShoppingCart().subscribe({
      next: (response) => {
        this.kursevi = response;
        console.log('Shopping cart loaded successfully:', this.kursevi.length, 'items');
      },
      error: (error) => {
        console.error('Failed to load shopping cart:', error);
        this.snackboxService.showSnackbox("Unable to load your cart items. Please try again later.");
      }
    });
  }

  /**
   * Navigates to the wishlist page
   */
  MoveToWishList(): void {
    console.log('Navigating to wishlist');
    this.router.navigateByUrl("/course-wishlist");
  }

  /**
   * Initiates the checkout process with Stripe
   * Handles the entire payment flow with proper error handling
   */
  onCheckout(): void {
    // Prevent multiple clicks
    if (this.isProcessingCheckout) {
      console.log('Checkout already in progress');
      return;
    }

    // Check if cart is empty
    if (!this.kursevi || this.kursevi.length === 0) {
      this.snackboxService.showSnackbox("Your cart is empty. Please add courses before checkout.");
      console.warn('Attempted checkout with empty cart');
      return;
    }

    // Verify Stripe is properly initialized
    if (!this.isStripeLoaded || !this.stripe) {
      console.log('Stripe not ready, attempting to initialize before checkout');
      this.snackboxService.showSnackbox("Preparing payment system...");

      // Initialize Stripe with callback to retry checkout once loaded
      this.initializeStripeAndProceed();
      return;
    }

    // Proceed with checkout - now delegated to separate method
    this.proceedWithCheckout();
  }

  /**
   * Centralized error handler for checkout process
   * @param errorType - The type/category of error
   * @param error - The error details
   */
  private handleCheckoutError(errorType: string, error: any): void {
    this.isProcessingCheckout = false;

    // Log detailed error information
    console.error(`Checkout Error (${errorType}):`, error);

    // Create user-friendly message
    let userMessage = "There was a problem with the payment process. Please try again later.";

    // Add more specific messaging based on error type
    if (errorType === 'Stripe redirect error') {
      userMessage = "Unable to redirect to the payment page. Please try again.";
    } else if (errorType === 'Checkout session creation failed') {
      userMessage = "Unable to create payment session. Please verify your payment details and try again.";
    }

    // Notify user
    this.ngZone.run(() => {
      this.snackboxService.showSnackbox(userMessage);
    });
  }

  /**
   * Fetches and updates the total price of items in cart
   */
  private UcitajTotal(): void {
    this.shoppingCartService.getTotalPrice().subscribe({
      next: (response) => {
        this.totalprice = response;
        console.log('Total price updated:', this.totalprice);
      },
      error: (error) => {
        console.error('Failed to load total price:', error);
      }
    });
  }

  /**
   * Moves a course to wishlist
   * @param courseId - ID of the course to be moved
   * TODO: Implement this method
   */
  PrebaciNaWshlistu(courseId: any): void {
    console.log('Moving course to wishlist:', courseId);
    // Implementation needed - could integrate with a wishlist service
  }

  /**
   * Removes a course from the shopping cart
   * @param courseId - ID of the course to be removed
   */
  ObrisiKurs(courseId: string): void {
    console.log('Removing course from cart:', courseId);

    this.shoppingCartService.removeCourseFromCart(courseId).subscribe({
      next: response => {
        console.log('Course successfully removed from cart');
        this.snackboxService.showSnackbox("Course removed from cart");

        // Refresh cart data
        this.LoadShoppingCrt();
        this.UcitajTotal();
      },
      error: error => {
        console.error('Failed to remove course from cart:', error);
        this.snackboxService.showSnackbox("Failed to remove course. Please try again.");
      }
    });
  }

  /**
   * Empties the entire shopping cart
   */
  ClearCart(): void {
    console.log('Clearing shopping cart');

    this.shoppingCartService.clearShoppingCart().subscribe({
      next: response => {
        console.log('Shopping cart cleared successfully');
        this.snackboxService.showSnackbox("You successfully cleared your cart");

        // Refresh cart data
        this.LoadShoppingCrt();
        this.UcitajTotal();
      },
      error: error => {
        console.error('Failed to clear shopping cart:', error);
        this.snackboxService.showSnackbox("Failed to clear cart. Please try again.");
      }
    });
  }

  /**
   * Navigates to course details page
   * @param courseId - ID of the course to view
   */
  OpenCourse(courseId: any): void {
    console.log('Opening course details:', courseId);
    this.router.navigate(['/student/course/details', courseId]);
  }
}
