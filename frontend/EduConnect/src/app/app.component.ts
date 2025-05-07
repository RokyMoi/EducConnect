import { Component, inject, OnInit } from '@angular/core';
import { Router, RouterOutlet } from '@angular/router';
import { HttpClient } from '@angular/common/http';
import { BodyComponent } from './landingPage/body/body.component';
import { ServerHealthCheckService } from './services/server-health-check.service';
import { CommonModule, NgIf } from '@angular/common';
import { ServerOfflineComponent } from './error/server-offline/server-offline.component';
import { RouterModule } from '@angular/router';
import { AccountService } from './services/account.service';
import { HeaderTemplateComponent } from './common/header/header-template/header-template.component';

import { SnackboxComponent } from './modules/shared/snackbox/snackbox.component';
import { HeaderComponent } from './landingPage/header/header.component';
import { VoiceInjectorComponent } from "./modules/shared/voice-injector/voice-injector.component";

@Component({
  selector: 'app-root',
  imports: [
    RouterOutlet,
    RouterModule,
    CommonModule,
    SnackboxComponent,
    HeaderComponent,
    VoiceInjectorComponent
],
  standalone: true,
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css'],
})
export class AppComponent implements OnInit {
  title = 'EduConnect';
  users: any;
  isServerRunning = false;
  private AccountService = inject(AccountService);
  router = inject(Router);
  constructor(private serverHealthCheckService: ServerHealthCheckService) {}

  ngOnInit(): void {
    this.setCurrentUser();
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
  isThreadComponent(): boolean {
    return this.router.url.includes('/studentMessageThread');
  }

  setCurrentUser() {
    const userString = localStorage.getItem('user');
    if (userString) {
      // Check if userString is not null
      const user = JSON.parse(userString);
      this.AccountService.CurrentUser.set(user);
    } else {
      console.log('No user found in localStorage');
    }
  }
}
