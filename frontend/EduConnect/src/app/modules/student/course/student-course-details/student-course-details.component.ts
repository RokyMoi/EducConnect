import { CommonModule } from '@angular/common';
import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { CourseStudentControllerService } from '../../../../services/course/course-student-controller.service';
import { GetCoursesByQueryResponse } from '../../../../models/course/course-student-controller/get-courses-by-query-response';
import { SnackboxService } from '../../../../services/shared/snackbox.service';
import {EnrollmentService} from '../../../../services/Shopping-Cart/enrollment.service';
import {ShoppingCartService} from '../../../../services/Shopping-Cart/shopping.service';

@Component({
  selector: 'app-student-course-details',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './student-course-details.component.html',
  styleUrl: './student-course-details.component.css',
})
export class StudentCourseDetailsComponent implements OnInit {
  courseId: string = '';
  priceCourse: any;
  buttonCourseLabel:any;
  course: GetCoursesByQueryResponse | null = null;
  constructor(
    private router: Router,
    private route: ActivatedRoute,
    private courseStudentControllerService: CourseStudentControllerService,
    private snackboxService: SnackboxService,
    private enrollmentService: EnrollmentService,
    private shoppingService: ShoppingCartService,
  ) {
    this.courseId = this.route.snapshot.paramMap.get('courseId') || '';
    console.log('Course ID:', this.courseId);
  }

  ngOnInit(): void {
    this.UcitajKursInfo();
    this.getCourse();
    this.courseStudentControllerService
      .setEnteredOnCourseViewershipData()
      .subscribe({
        next: (response) => {
          console.log(response);
        },
        error: (error) => {
          console.log(error);
        },
      });
  }
  goBack() {
    this.courseStudentControllerService
      .setLeftOnCourseViewershipData()
      .subscribe({
        next: (response) => {
          console.log(response);
          this.router.navigate(['/student/course/search']);
        },
        error: (error) => {
          console.log(error);
          this.router.navigate(['/student/course/search']);
        },
      });
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

  private UcitajKursInfo() {
    this.enrollmentService.getEnrollmentStatus(this.courseId).subscribe({
      next: (response) => {
        console.log("Enrollment status je upravo ucitan (price): ",response.price);
        console.log("Enrollment status je upravo ucitan (buttonLabel): ",response.buttonLabel);
        this.buttonCourseLabel = response.buttonLabel;
        this.priceCourse = response.price;
      }
    })
  }

  EnrollujCourse() {
if(this.priceCourse === 0) {
  this.enrollmentService.enrollCourse(this.courseId).subscribe({
    next: (response) => {
      this.snackboxService.showSnackbox("Uspjesno ste pristupili ovom kursu");
    },
    error: err => {
      console.log("Error je pri dodavanju kursa", err.error);
    }
  })
  /*
  Ako se desiiiii greska zbog guida ~!! GUBIM KOSUUUUUUUUUUUUUUUUU.
   */
  this.enrollmentService.debugEnrollment(this.courseId).subscribe({
    next: (response) => {
      console.log('Debug response:', response);

    },
    error: (error) => {
      console.error('Debug request failed:', error);
    }
  });
}else{
this.shoppingService.addCourseToCart(this.courseId).subscribe({
  next: (response) => {
    this.snackboxService.showSnackbox("Uspjesno je dodat u cart kurs");
  }
})
}

  }
}
