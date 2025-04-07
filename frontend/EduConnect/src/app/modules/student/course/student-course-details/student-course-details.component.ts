import { CommonModule } from '@angular/common';
import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { CourseStudentControllerService } from '../../../../services/course/course-student-controller.service';
import { GetCoursesByQueryResponse } from '../../../../models/course/course-student-controller/get-courses-by-query-response';
import { SnackboxService } from '../../../../services/shared/snackbox.service';

@Component({
  selector: 'app-student-course-details',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './student-course-details.component.html',
  styleUrl: './student-course-details.component.css',
})
export class StudentCourseDetailsComponent implements OnInit {
  courseId: string = '';
  course: GetCoursesByQueryResponse | null = null;
  constructor(
    private router: Router,
    private route: ActivatedRoute,
    private courseStudentControllerService: CourseStudentControllerService,
    private snackboxService: SnackboxService
  ) {
    this.courseId = this.route.snapshot.paramMap.get('courseId') || '';
    console.log('Course ID:', this.courseId);
  }

  ngOnInit(): void {
    this.getCourse();
  }
  goBack() {
    this.router.navigate(['/student/course/search']);
  }

  getCourse() {
    this.courseStudentControllerService.GetCourseById(this.courseId).subscribe({
      next: (response) => {
        console.log(response);
        this.course = response.data;
      },
      error: (error) => {
        console.log(error);
        this.snackboxService.showSnackbox(
          'Failed to load course information, returniing to course search page',
          'error'
        );
        this.router.navigate(['/student/course/search']);
      },
    });
  }
}
