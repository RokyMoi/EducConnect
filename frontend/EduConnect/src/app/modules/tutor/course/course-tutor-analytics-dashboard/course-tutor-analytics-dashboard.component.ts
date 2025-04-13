import { CommonModule } from '@angular/common';
import { Component, OnDestroy, OnInit } from '@angular/core';
import { Subject, Subscription, take, takeUntil } from 'rxjs';
import { FontAwesomeModule } from '@fortawesome/angular-fontawesome';
import { Chart, Legend, registerables } from 'chart.js';
import { CourseStatistics } from '../../../../models/course/course-analytics-hub/course-statistics';
import { CourseAnalyticsService } from '../../../../services/signalr-services/course-analytics.service';
import { CourseViewershipUpdate } from '../../../../models/course/course-analytics-hub/course-viewership-update';
import { ActivatedRoute, Router } from '@angular/router';
import { CourseTutorControllerService } from '../../../../services/course/course-tutor-controller.service';
import { GetCourseAnalyticsHistoryResponse } from '../../../../models/course/course-tutor-controller/get-course-analytics-history-response';
import { CourseAnalyticsHistory } from '../../../../models/course/course-tutor-controller/course-analytics-history';

Chart.register(...registerables);

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

  analyticsHistory: CourseAnalyticsHistory[] = [];
  usersCameFromFeedCount: number = 0;
  usersCameFromSearchCount: number = 0;

  private subscription!: Subscription;

  lineCtx: any;
  barCtx: any;
  pieCtx: any;
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
        this.usersCameFromFeedCount = data.data.usersCameFromFeedCount;
        this.usersCameFromSearchCount = data.data.usersCameFromSearchCount;
        this.analyticsHistory = data.data.courseAnalyticsHistory;
        const cleanedChartData = this.analyticsHistory
          .filter(
            (entry, index, self) =>
              index ===
              self.findIndex(
                (e) =>
                  e.createdAt === entry.createdAt &&
                  e.totalViews == entry.totalViews
              )
          )
          .sort(
            (a, b) =>
              new Date(a.createdAt).getTime() - new Date(b.createdAt).getTime()
          );

        const lineChartLabels = cleanedChartData.map((entry) =>
          new Date(entry.createdAt).toLocaleString()
        );

        const totalViews = cleanedChartData.map((entry) => entry.totalViews);
        const numberOfUniqueVisitors = cleanedChartData.map(
          (entry) => entry.numberOfUniqueVisitors
        );
        const currentlyViewing = cleanedChartData.map(
          (entry) => entry.currentlyViewing
        );
        const averageViewDurationInMinutes = cleanedChartData.map(
          (entry) => entry.averageViewDurationInMinutes
        );
        const barChartLabels = cleanedChartData.map((entry) =>
          new Date(entry.createdAt).toLocaleString([], {
            hour: '2-digit',
            minute: '2-digit',
          })
        );

        const pieChartLabels = ['From Feed', 'From Search'];

        this.lineCtx = new Chart('lineChart', {
          type: 'line',
          data: {
            labels: lineChartLabels,
            datasets: [
              {
                label: 'Total Views',
                data: totalViews,
                borderColor: 'rgba(54, 162, 235, 1)',
                backgroundColor: 'rgba(54, 162, 235, 0.2)',
                fill: false,
                tension: 0.2,
              },
              {
                label: 'Unique Visitors',
                data: numberOfUniqueVisitors,
                borderColor: 'rgba(255, 99, 132, 1)',
                backgroundColor: 'rgba(255, 99, 132, 0.2)',
                fill: false,
                tension: 0.2,
              },
              {
                label: 'Currently Viewing',
                data: currentlyViewing,
                borderColor: 'rgba(255, 206, 86, 1)',
                backgroundColor: 'rgba(255, 206, 86, 0.2)',
                fill: false,
                tension: 0.2,
              },
            ],
          },
          options: {
            responsive: true,
            scales: {
              x: {
                title: {
                  display: true,
                  text: 'Date',
                },
                ticks: {
                  autoSkip: false,
                  maxRotation: 90,
                  minRotation: 45,
                },
              },
              y: {
                title: {
                  display: true,
                  text: 'Count',
                },
                beginAtZero: true,
              },
            },
          },
        });

        this.barCtx = new Chart('barChart', {
          type: 'bar',
          data: {
            labels: barChartLabels,
            datasets: [
              {
                label: 'Average View Duration (minutes)',
                data: averageViewDurationInMinutes,
                backgroundColor: 'rgba(153, 102, 255, 0.6)',
                borderColor: 'rgba(153, 102, 255, 1)',
                borderWidth: 1,
              },
            ],
          },
          options: {
            responsive: true,
            plugins: {
              title: {
                display: true,
                text: 'Average View Duration Per Time Slot',
                font: {
                  size: 18,
                },
              },
              tooltip: {
                mode: 'index',
                intersect: false,
              },
              legend: {
                display: true,
                position: 'top',
              },
            },
            scales: {
              x: {
                title: {
                  display: true,
                  text: 'Time',
                },
              },
              y: {
                title: {
                  display: true,
                  text: 'Minutes',
                },
                beginAtZero: true,
              },
            },
          },
        });

        this.pieCtx = new Chart('pieChart', {
          type: 'pie',
          data: {
            labels: ['Feed', 'Search'],
            datasets: [
              {
                data: [
                  this.usersCameFromFeedCount,
                  this.usersCameFromSearchCount,
                ],
                backgroundColor: ['red', 'blue'],
                borderColor: ['white', 'black'],
                borderWidth: 1,
              },
            ],
          },
          options: {
            responsive: true,
            plugins: {
              legend: {
                position: 'top',
              },
              title: {
                display: true,
                text: 'Traffic Source',
              },
              tooltip: {
                callbacks: {
                  label: (tooltipItem) => {
                    const label =
                      ['Feed', 'Search'][tooltipItem.dataIndex] || '';
                    const value = [
                      this.usersCameFromFeedCount,
                      this.usersCameFromSearchCount,
                    ][tooltipItem.dataIndex];
                    return `${label}: ${value}`;
                  },
                },
              },
            },
          },
        });
      });
  }
}
