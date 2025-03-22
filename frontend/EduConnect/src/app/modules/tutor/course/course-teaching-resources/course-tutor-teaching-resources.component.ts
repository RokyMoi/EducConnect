import { Component } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';

@Component({
  selector: 'app-course-teaching-resources',
  standalone: true,
  imports: [],
  templateUrl: './course-teaching-resources.component.html',
  styleUrl: './course-teaching-resources.component.css',
})
export class CourseTutorTeachingResourcesComponent {
  courseId: string | null = null;
  constructor(private router: Router, private route: ActivatedRoute) {
    this.courseId = this.route.snapshot.paramMap.get('courseId');
  }
  goBack() {
    this.router.navigate(['/tutor/course/' + this.courseId]);
  }

  onAddNewResource() {
    this.router.navigate([
      '/tutor/course/teaching-resources/new/' + this.courseId,
    ]);
  }
}
