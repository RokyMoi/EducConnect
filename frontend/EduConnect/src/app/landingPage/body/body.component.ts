import { Component, inject } from '@angular/core';
import {
  Router,
  RouterLink,
  RouterModule,
  RouterOutlet,
  RouterLinkActive,
} from '@angular/router';
import { AccountService } from '../../services/account.service';
import { TutorDashboardComponent } from '../../tutor/tutor-dashboard/tutor-dashboard.component';
import { StudentDashboardComponent } from '../../Student/student-dashboard/student-dashboard.component';
@Component({
  selector: 'app-landing-page-body',
  imports: [RouterLink, RouterOutlet, RouterLinkActive,TutorDashboardComponent,StudentDashboardComponent],
  templateUrl: './body.component.html',
  styleUrl: './body.component.css',
  standalone: true,
})
export class BodyComponent {
  accService= inject(AccountService);
ruter = inject(Router);
  constructor(private router: Router) {}
}
