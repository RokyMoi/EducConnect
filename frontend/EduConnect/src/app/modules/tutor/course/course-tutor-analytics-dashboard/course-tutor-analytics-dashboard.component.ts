import { CommonModule } from '@angular/common';
import { Component, OnDestroy, OnInit } from '@angular/core';
import { Subject, Subscription, take, takeUntil } from 'rxjs';
import { CourseStatistics } from '../../../../models/course/course-analytics-hub/course-statistics';
import { CourseAnalyticsService } from '../../../../services/signalr-services/course-analytics.service';
import { CourseViewershipUpdate } from '../../../../models/course/course-analytics-hub/course-viewership-update';
import { ActivatedRoute, Router } from '@angular/router';

@Component({
  selector: 'app-course-tutor-analytics-dashboard',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './course-tutor-analytics-dashboard.component.html',
  styleUrl: './course-tutor-analytics-dashboard.component.css',
})
export class CourseTutorAnalyticsDashboardComponent
  implements OnInit, OnDestroy
{
  courseId: string = '';
  courseStatistics: CourseStatistics = {
    totalViews: 0,
    activeViewers: 0,
    averageViewDurationInMinutes: 0,
  };
  private subscription!: Subscription;

  constructor(
    private analyticsService: CourseAnalyticsService,
    private route: ActivatedRoute,
    private router: Router
  ) {
    this.courseId = this.route.snapshot.params['courseId'];
  }

  ngOnInit() {
    this.analyticsService.startConnection(this.courseId);
    this.subscription = this.analyticsService.courseAnalytics$.subscribe(
      (data) => (this.courseStatistics = data as CourseStatistics)
    );
  }
  ngOnDestroy(): void {
    this.analyticsService.stopConnection();
    if (this.subscription) {
      this.subscription.unsubscribe();
    }
  }
  goBack() {
    this.router.navigate(['/tutor/course', this.courseId]);
  }
}
