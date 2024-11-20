import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
@Injectable({
  providedIn: 'root',
})
export class ServerHealthCheckService {
  private serverHealthStatusUrl = 'http://localhost:5177/server/status';
  constructor(private http: HttpClient) {}

  getServerHealth(): Observable<any> {
    return this.http.get(this.serverHealthStatusUrl);
  }
}
