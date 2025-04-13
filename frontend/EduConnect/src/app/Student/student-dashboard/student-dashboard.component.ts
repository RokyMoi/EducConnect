import { Component, inject, OnInit } from '@angular/core';

import { ForYouPageComponent } from '../for-you-page/for-you-page.component';
import { CommonModule } from '@angular/common';
import { PersonControllerService } from '../../services/shared/person-controller.service';
import { ActivatedRoute, Router } from '@angular/router';
import { GetDashboardUserInfoResponse } from '../../models/person/person-controller/get-dashboard-user-info-response';
import { AccountService } from '../../services/account.service';
import { User } from '../../models/User';

@Component({
  selector: 'app-student-dashboard',
  standalone: true,
  imports: [ForYouPageComponent, CommonModule],
  templateUrl: './student-dashboard.component.html',
  styleUrl: './student-dashboard.component.css',
})
export class StudentDashboardComponent implements OnInit {
  dashboardUserInfo!: GetDashboardUserInfoResponse;
  accountService = inject(PersonControllerService);
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

    console.log('Current user in dashboard:', localStorage.getItem('user'));
    const userString = localStorage.getItem('user');
    const user: User | null = userString ? JSON.parse(userString) : null;
    this.accountService.CurrentUser.set(user);
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
