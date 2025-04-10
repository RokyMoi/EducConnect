import { CommonModule } from '@angular/common';
import { Component, OnDestroy, OnInit } from '@angular/core';
import { Subject, takeUntil } from 'rxjs';
import { CourseStatistics } from '../../../../models/course/course-analytics-hub/course-statistics';
import { CourseAnalyticsService } from '../../../../services/signalr-services/course-analytics.service';
import { CourseViewershipUpdate } from '../../../../models/course/course-analytics-hub/course-viewership-update';

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
  private destroy$ = new Subject<void>();
  currentStats?: CourseStatistics;

  constructor(private analytics: CourseAnalyticsService) {}

  ngOnInit() {
    this.analytics.startConnection();
    this.analytics.subscribeToCourse('COURSE_UUID');

    this.analytics.updates$
      .pipe(takeUntil(this.destroy$))
      .subscribe((update) => {
        this.handleUpdate(update);
      });

    this.analytics.connection.on(
      'ReceiveInitialData',
      (stats: CourseStatistics) => {
        this.currentStats = stats;
      }
    );
  }

  private handleUpdate(update: CourseViewershipUpdate) {
    if (this.currentStats) {
      this.currentStats = {
        totalViews: this.currentStats.totalViews + update.newViews,
        activeNow: this.currentStats.activeNow + update.activeViewersChange,
        avgTimeSpent: update.updatedAverageTime,
      };
    }
  }

  ngOnDestroy() {
    this.destroy$.next();
    this.destroy$.complete();
  }
}
