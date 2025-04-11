import { Injectable } from '@angular/core';
import * as signalR from '@microsoft/signalr';
import { Subject } from 'rxjs';
import { CourseViewershipUpdate } from '../../models/course/course-analytics-hub/course-viewership-update';
import { CourseStatistics } from '../../models/course/course-analytics-hub/course-statistics';

@Injectable({
  providedIn: 'root',
})
export class CourseAnalyticsService {
  public connection: signalR.HubConnection;
  private _updates = new Subject<CourseViewershipUpdate>();

  public get updates$() {
    return this._updates.asObservable();
  }

  constructor() {
    this.connection = new signalR.HubConnectionBuilder()
      .withUrl('http://localhost:5177/tutor/course/analytics/hub')
      .withAutomaticReconnect()
      .build();
  }

  public startConnection(): Promise<void> {
    return this.connection
      .start()
      .then(() => {
        console.log('Course Analytics SignalR Hub Connected');
        this.registerListeners();
      })
      .catch((error) =>
        console.error('Course Analytics SignalR Hub Connection Error:', error)
      );
  }

  private registerListeners(): void {
    this.connection.on('UpdateViewership', (update: CourseViewershipUpdate) => {
      this._updates.next(update);
    });

    this.connection.on('BatchUpdate', (updates: CourseViewershipUpdate[]) => {
      updates.forEach((update) => this._updates.next(update));
    });
  }

  public subscribeToCourse(courseId: string): Promise<void> {
    return this.connection.invoke('SubscribeToCourse', courseId);
  }

  public get currentState(): Promise<CourseStatistics> {
    return this.connection.invoke('GetCurrentState');
  }
}
