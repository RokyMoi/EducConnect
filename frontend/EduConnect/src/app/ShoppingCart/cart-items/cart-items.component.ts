import { Component, inject } from '@angular/core';
import { Router } from '@angular/router';

@Component({
  selector: 'app-cart-items',
  standalone:true,
  imports: [],
  templateUrl: './cart-items.component.html',
  styleUrl: './cart-items.component.css'
})
export class CartItemsComponent {
  router = inject(Router);
MoveToWishList() {
this.router.navigateByUrl("/course-wishlist");
}

}
