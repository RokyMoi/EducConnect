import { Component, inject } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';

@Component({
  selector: 'app-forbidden-access',
  standalone: true,
  imports: [],
  templateUrl: './forbidden-access.component.html',
  styleUrl: './forbidden-access.component.css',
})
export class ForbiddenAccessComponent {
  router = inject(Router);

  goToHomePage() {
    this.router.navigate([`/user/dashboard`]);
  }
}
