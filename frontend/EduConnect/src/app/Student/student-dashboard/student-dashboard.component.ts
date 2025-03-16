import { Component, OnInit } from '@angular/core';

import { ForYouPageComponent } from '../for-you-page/for-you-page.component';
import { CommonModule } from '@angular/common';
import { PersonControllerService } from '../../services/shared/person-controller.service';
import { ActivatedRoute, Router } from '@angular/router';
import { GetDashboardUserInfoResponse } from '../../models/person/person-controller/get-dashboard-user-info-response';

@Component({
  selector: 'app-student-dashboard',
  standalone: true,
  imports: [ForYouPageComponent, CommonModule],
  templateUrl: './student-dashboard.component.html',
  styleUrl: './student-dashboard.component.css',
})
export class StudentDashboardComponent implements OnInit {
  dashboardUserInfo!: GetDashboardUserInfoResponse;

  constructor(
    private router: Router,
    private route: ActivatedRoute,
    private personControllerService: PersonControllerService
  ) {}
  ngOnInit(): void {
    this.personControllerService.getDashboardUserInfo().subscribe({
      next: (response) => {
        console.log(response);
        this.dashboardUserInfo = response.data;
      },
      error: (error) => {
        console.log(error);
      },
    });
  }

  logout() {
    this.personControllerService.logout().subscribe({
      next: (response) => {
        console.log(response);
        localStorage.removeItem('Authorization');
        this.router.navigate(['/login']);
      },
      error: (error) => {
        console.log(error);
      },
    });
  }
}
