import { Component, OnInit } from '@angular/core';
import { CourseTutorControllerService } from '../../../../services/course/course-tutor-controller.service';
import { GetAllCoursesResponse } from '../../../../models/course/course-tutor-controller/get-all-courses-response';
import { CommonModule } from '@angular/common';
import { SnackboxService } from '../../../../services/shared/snackbox.service';

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
    private courseTutorControllerService: CourseTutorControllerService
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
}
