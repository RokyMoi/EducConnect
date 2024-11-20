import { Component, inject, OnInit } from '@angular/core';
import { RouterOutlet } from '@angular/router';
import { HeaderComponent } from './landingPage/header/header.component';
import { HttpClient } from '@angular/common/http';
import { BodyComponent } from './landingPage/body/body.component';
import { ServerHealthCheckService } from './services/server-health-check.service';
import { CommonModule, NgIf } from '@angular/common';
import { ServerOfflineComponent } from './error/server-offline/server-offline.component';
import { RouterModule } from '@angular/router';

@Component({
  selector: 'app-root',
  standalone: true,
  imports: [
    RouterOutlet,
    RouterModule,
    HeaderComponent,
    BodyComponent,
    CommonModule,
    NgIf,
    ServerOfflineComponent,
  ],
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css'],
})
export class AppComponent implements OnInit {
  title = 'EduConnect';
  users: any;
  isServerRunning = false;

  constructor(private serverHealthCheckService: ServerHealthCheckService) {}

  ngOnInit(): void {
    console.log('Checking server health...');
    this.serverHealthCheckService.getServerHealth().subscribe(
      (response) => {
        console.log('Server health: ', response.data.serverOnline);
        this.isServerRunning = response.data.serverOnline;
      },
      (error) => {
        console.log('Server health: Server offline');
        console.error('Error: ', error);
        this.isServerRunning = true;
      }
    );
  }
}
