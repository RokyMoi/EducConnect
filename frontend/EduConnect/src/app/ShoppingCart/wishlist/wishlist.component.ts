<<<<<<< HEAD
import { Component, inject } from '@angular/core';
import { Router } from '@angular/router';

@Component({
  selector: 'app-wishlist',
  standalone: true,
=======
import { Component } from '@angular/core';

@Component({
  selector: 'app-wishlist',
>>>>>>> d93e7ce8e2cd19478839a575de236d1244ad0fd8
  imports: [],
  templateUrl: './wishlist.component.html',
  styleUrl: './wishlist.component.css'
})
export class WishlistComponent {
<<<<<<< HEAD
  router = inject(Router);
MoveToShoppingCart() {
this.router.navigateByUrl("/Shopping-Cart");
}
=======
>>>>>>> d93e7ce8e2cd19478839a575de236d1244ad0fd8

}
