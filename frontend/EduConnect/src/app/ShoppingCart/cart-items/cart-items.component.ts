import { Component, inject } from '@angular/core';
import { Router } from '@angular/router';
import { take } from 'rxjs';
import {PaymentService} from '../../services/Shopping-Cart/payment.service';


declare var Stripe: any;

@Component({
  selector: 'app-cart-items',
  standalone: true,
  imports: [],
  templateUrl: './cart-items.component.html',
  styleUrls: ['./cart-items.component.css']
})
export class CartItemsComponent {
  private router = inject(Router);
  private stripe: any;

  constructor(private paymentService: PaymentService) {
    this.stripe = Stripe("primjerDokSeNePodeesiBegi");
  }

  MoveToWishList() {
    this.router.navigateByUrl("/course-wishlist");
  }

  onCheckout(): void {
    this.paymentService.createCheckoutSession().subscribe({
      next: (response:any) => {
        this.stripe.redirectToCheckout({ sessionId: response.sessionId })
          .then((result: any) => {
            if (result.error) {
              console.error(result.error.message);
            }
          });
      },
      error: (error:any) => {
        console.error('Error creating checkout session', error);
      }
    });
  }
}
