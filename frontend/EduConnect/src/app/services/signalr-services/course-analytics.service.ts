import { Injectable } from '@angular/core';
import * as signalR from '@microsoft/signalr';
import { BehaviorSubject, Observable, single, Subject } from 'rxjs';
import { CourseViewershipUpdate } from '../../models/course/course-analytics-hub/course-viewership-update';
import { CourseStatistics } from '../../models/course/course-analytics-hub/course-statistics';
import ApiLinks from '../../../assets/api/link.api';

@Injectable({
  providedIn: 'root',
})
export class CourseAnalyticsService {
  private connection!: signalR.HubConnection;
  private courseAnalyticsDataSource =
    new BehaviorSubject<CourseStatistics | null>(null);
  courseAnalytics$ = this.courseAnalyticsDataSource.asObservable();

  startConnection(courseId: string): void {
    this.connection = new signalR.HubConnectionBuilder()
      .withUrl('http://localhost:5177/course-analytics-hub')
      .build();

    this.connection
      .start()
      .then(() => {
        this.connection.invoke('SubscribeToCourse', courseId);
      })
      .catch((error) => {
        console.error('Error starting SignalR connection:', error);
      });

    this.connection.on('GetAnalyticsData', (data) => {
      this.courseAnalyticsDataSource.next(data);
    });
  }
  stopConnection(): void {
    if (
      this.connection &&
      this.connection.state === signalR.HubConnectionState.Connected
    ) {
      this.connection.stop();
    }
  }
}
