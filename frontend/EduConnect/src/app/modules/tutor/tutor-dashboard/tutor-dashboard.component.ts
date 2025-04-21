import { Component, inject, OnInit } from '@angular/core';
import { AccountService } from '../../../services/account.service';
import ITutorRegistrationStatus from '../../../models/Tutor/tutorRegistrationStatus.tutor';
import { TutorRegistrationStatusService } from '../../../services/tutor/tutor-status/tutor-status.service';
import { CreateCourseComponent } from '../course/create-course/create-course.component';
import { SubmitButtonComponent } from '../../../common/button/submit-button/submit-button.component';
import { CommonModule } from '@angular/common';
import { ActivatedRoute, Router } from '@angular/router';
import { PersonControllerService } from '../../../services/shared/person-controller.service';
import { GetDashboardUserInfoResponse } from '../../../models/person/person-controller/get-dashboard-user-info-response';

@Component({
  standalone: true,
  selector: 'app-tutor-dashboard',
  imports: [CommonModule],

  templateUrl: './tutor-dashboard.component.html',
  styleUrl: './tutor-dashboard.component.css',
})
export class TutorDashboardComponent implements OnInit {
  dashboardItems: {
    title: string;
    description: string;
    dataInfo: string;
    buttonText: string;
    link: string;
  }[] = [];

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
    this.dashboardItems = [
      {
        title: 'New Course',
        description: 'Create a new course',
        dataInfo: '',
        buttonText: 'Create',
        link: '/tutor/course/create',
      },
      {
        title: 'My Courses',
        description: 'View and manage all your courses',
        dataInfo: '',
        buttonText: 'View My Courses',
        link: '/tutor/course',
      },
      {
        title: 'Collaboration Document',
        description: 'Manage and work with other on a shared document',
        dataInfo: '',
        buttonText: 'View',
        link: '/tutor/collaboration',
      },
    ];
  }

  goTo(link: string) {
    this.router.navigate([link]);
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
