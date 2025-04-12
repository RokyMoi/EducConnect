import { CommonModule } from '@angular/common';
import { Component, OnDestroy, OnInit } from '@angular/core';
import { Subject, Subscription, take, takeUntil } from 'rxjs';
import { FontAwesomeModule } from '@fortawesome/angular-fontawesome';
import {
  faArrowUp,
  faArrowDown,
  faMinus,
} from '@fortawesome/free-solid-svg-icons';
import { Chart } from 'chart.js';
import { ChartConfiguration, ChartOptions } from 'chart.js';
import { CourseStatistics } from '../../../../models/course/course-analytics-hub/course-statistics';
import { CourseAnalyticsService } from '../../../../services/signalr-services/course-analytics.service';
import { CourseViewershipUpdate } from '../../../../models/course/course-analytics-hub/course-viewership-update';
import { ActivatedRoute, Router } from '@angular/router';
import { CourseTutorControllerService } from '../../../../services/course/course-tutor-controller.service';

@Component({
  selector: 'app-course-tutor-analytics-dashboard',
  standalone: true,
  imports: [CommonModule, FontAwesomeModule],
  templateUrl: './course-tutor-analytics-dashboard.component.html',
  styleUrl: './course-tutor-analytics-dashboard.component.css',
})
export class CourseTutorAnalyticsDashboardComponent
  implements OnInit, OnDestroy
{
  courseId: string = '';
  courseStatistics: CourseStatistics = {
    courseId: this.courseId,
    numberOfUniqueVisitors: 0,
    totalViews: 0,
    currentlyViewing: 0,
    averageViewDurationInMinutes: 0,
  };

  formerCourseStatistics: CourseStatistics = { ...this.courseStatistics };
  lastUpdated: Date | null = null;
  faArrowUp = faArrowUp;
  faArrowDown = faArrowDown;
  faMinus = faMinus;

  

  private subscription!: Subscription;

  constructor(
    private analyticsService: CourseAnalyticsService,
    private route: ActivatedRoute,
    private router: Router,
    private courseTutorControllerService: CourseTutorControllerService
  ) {
    this.courseId = this.route.snapshot.params['courseId'];
  }

  ngOnInit() {
    this.loadAnalyticsHistory();
    this.analyticsService.startConnection(this.courseId);
    this.subscription = this.analyticsService.courseAnalytics$.subscribe(
      (data) => {
        this.formerCourseStatistics = { ...this.courseStatistics };
        this.courseStatistics = data as CourseStatistics;
        this.lastUpdated = new Date();
      }
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

  loadAnalyticsHistory() {
    this.courseTutorControllerService
      .getCourseAnalyticsHistory(this.courseId)
      .subscribe((data) => {
        console.log(data);
      });
  }
}
