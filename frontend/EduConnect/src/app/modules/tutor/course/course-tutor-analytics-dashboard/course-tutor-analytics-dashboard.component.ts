import { CommonModule } from '@angular/common';
import { Component, OnDestroy, OnInit } from '@angular/core';
import { Subject, takeUntil } from 'rxjs';
import { CourseStatistics } from '../../../../models/course/course-analytics-hub/course-statistics';
import { CourseAnalyticsService } from '../../../../services/signalr-services/course-analytics.service';
import { CourseViewershipUpdate } from '../../../../models/course/course-analytics-hub/course-viewership-update';
import { ActivatedRoute } from '@angular/router';

@Component({
  selector: 'app-course-tutor-analytics-dashboard',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './course-tutor-analytics-dashboard.component.html',
  styleUrl: './course-tutor-analytics-dashboard.component.css',
})
export class CourseTutorAnalyticsDashboardComponent implements OnInit {
  courseId: string = '';
  constructor(
    private analyticsService: CourseAnalyticsService,
    private route: ActivatedRoute
  ) {
    this.courseId = this.route.snapshot.params['courseId'];
  }

  ngOnInit() {
    this.analyticsService.getAnalyticsData((message) => {
      console.log('Received message:', message);
    });

    this.analyticsService.requestAnalyticsData(this.courseId);
  }
}
