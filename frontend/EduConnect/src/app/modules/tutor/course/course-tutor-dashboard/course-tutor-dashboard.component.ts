import { Component, OnInit } from '@angular/core';
import { CourseTutorControllerService } from '../../../../services/course/course-tutor-controller.service';
import { GetAllCoursesResponse } from '../../../../models/course/course-tutor-controller/get-all-courses-response';
import { CommonModule } from '@angular/common';
import { SnackboxService } from '../../../../services/shared/snackbox.service';
import { Router } from '@angular/router';

@Component({
  selector: 'app-course-tutor-dashboard',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './course-tutor-dashboard.component.html',
  styleUrl: './course-tutor-dashboard.component.css',
})
export class CourseTutorDashboardComponent implements OnInit {
  courses: GetAllCoursesResponse[] = [];
  constructor(
    private snackboxService: SnackboxService,
    private courseTutorControllerService: CourseTutorControllerService,
    private router: Router
  ) {}
  ngOnInit(): void {
    this.loadCourses();
  }

  loadCourses() {
    this.courseTutorControllerService.getAllCourses().subscribe({
      next: (response) => {
        console.log(response);
        this.courses = response.data;
      },
      error: (error) => {
        console.log(error);
        this.snackboxService.showSnackbox(
          'Failed to load courses, please try again later',
          'error'
        );
      },
    });
  }

  goBack() {
    this.router.navigate(['/tutor/dashboard']);
  }

  onCreateCourse() {
    this.router.navigate(['/tutor/course/create']);
  }

  onViewCourse(courseId: string) {
    this.router.navigate(['tutor/course/', courseId]);
  }
}
