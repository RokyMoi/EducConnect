import { Component, OnInit } from '@angular/core';
import { SnackboxService } from '../../../../services/shared/snackbox.service';
import { Router, ActivatedRoute } from '@angular/router';
import { CourseTutorControllerService } from '../../../../services/course/course-tutor-controller.service';
import { GetPromotionImagesResponse } from '../../../../models/course/course-tutor-controller/get-promotion-images-response';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-course-tutor-promotion-images',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './course-tutor-promotion-images.component.html',
  styleUrl: './course-tutor-promotion-images.component.css',
})
export class CourseTutorPromotionImagesComponent implements OnInit {
  courseId: string | null = null;

  coursePromotionImages: GetPromotionImagesResponse[] = [];

  constructor(
    private router: Router,
    private route: ActivatedRoute,
    private courseTutorControllerService: CourseTutorControllerService,
    private snackboxService: SnackboxService
  ) {
    this.courseId = this.route.snapshot.paramMap.get('courseId');
  }
  ngOnInit(): void {
    this.loadCoursePromotionImages();
  }
  goBack() {
    this.router.navigate(['/tutor/course', this.courseId]);
  }

  loadCoursePromotionImages() {
    this.courseTutorControllerService
      .getPromotionImages(this.courseId as string)
      .subscribe({
        next: (response) => {
          console.log(response);
          this.coursePromotionImages = response.data;
        },
        error: (error) => {
          console.log(error);
          this.snackboxService.showSnackbox(
            'Failed to load promotion images, please try again later',
            'error'
          );
        },
      });
  }

  getImageLink(imageId: string) {
    return `http://localhost:5177/public/course/promotion/image/${imageId}`;
  }
}
