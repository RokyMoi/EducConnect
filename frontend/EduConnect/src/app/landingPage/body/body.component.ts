import { Component, inject, OnInit } from '@angular/core';
import {
  Router,
  RouterLink,
  RouterModule,
  RouterOutlet,
  RouterLinkActive,
} from '@angular/router';
import { AccountService } from '../../services/account.service';
import { TutorDashboardComponent } from '../../modules/tutor/tutor-dashboard/tutor-dashboard.component';
import { StudentDashboardComponent } from '../../Student/student-dashboard/student-dashboard.component';
import { PersonControllerService } from '../../services/shared/person-controller.service';
@Component({
  selector: 'app-landing-page-body',
  imports: [
    RouterLink,
    RouterOutlet,
    RouterLinkActive,
    TutorDashboardComponent,
    StudentDashboardComponent,
  ],
  templateUrl: './body.component.html',
  styleUrl: './body.component.css',
  standalone: true,
})
export class BodyComponent implements OnInit {
  accService = inject(AccountService);
  constructor(
    private router: Router,
    private personControllerService: PersonControllerService
  ) {}
  ngOnInit(): void {
    
  }
}
