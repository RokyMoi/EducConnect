import { Component } from '@angular/core';
import {
  Router,
  RouterLink,
  RouterModule,
  RouterOutlet,
  RouterLinkActive,
} from '@angular/router';
@Component({
  selector: 'app-landing-page-body',
  imports: [RouterLink, RouterOutlet, RouterLinkActive],
  templateUrl: './body.component.html',
  styleUrl: './body.component.css',
  standalone: true,
})
export class BodyComponent {
  constructor(private router: Router) {}
}
