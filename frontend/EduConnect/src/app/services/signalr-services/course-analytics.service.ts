import { Injectable } from '@angular/core';
import * as signalR from '@microsoft/signalr';
import { Subject } from 'rxjs';
import { CourseViewershipUpdate } from '../../models/course/course-analytics-hub/course-viewership-update';
import { CourseStatistics } from '../../models/course/course-analytics-hub/course-statistics';
import ApiLinks from '../../../assets/api/link.api';

@Injectable({
  providedIn: 'root',
})
export class CourseAnalyticsService {
  private connection!: signalR.HubConnection;
  private isConnectionReady: boolean = false;
  private pendingSubscriptions: number[] = [];

  /**
   *
   */
  constructor() {
    this.startConnection();
  }
  public startConnection = () => {
    this.connection = new signalR.HubConnectionBuilder()
      .withUrl(`http://localhost:5177/course-analytics-hub`)
      .configureLogging(signalR.LogLevel.Information)
      .build();

    this.connection
      .start()
      .then(() => {
        console.log('Connection started');
        // Invoke the server-side method after the connection is started
      })
      .catch((err) => {
        console.log('Error while starting connection: ' + err);
      });
  };
  public requestAnalyticsData = (courseId: string) => {
    if (this.connection.state === signalR.HubConnectionState.Connected) {
      this.connection
        .invoke('SendAnalyticsData', courseId) // Call the server-side method
        .catch((err) => {
          console.error('Error invoking SendAnalyticsData', err);
        });
    } else {
      console.warn('SignalR connection not established yet.');
    }
  };
  public getAnalyticsData = (callback: (message: string) => void) => {
    this.connection.on('GetAnalyticsData', (data) => {
      callback(data);
    });
  };
}
