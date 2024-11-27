import { Component, inject, OnInit } from '@angular/core';
import { RouterOutlet } from '@angular/router';
import { HeaderComponent } from './landingPage/header/header.component';
import { HttpClient } from '@angular/common/http';
import { BodyComponent } from './landingPage/body/body.component';
import { ServerHealthCheckService } from './services/server-health-check.service';
import { CommonModule, NgIf } from '@angular/common';
import { ServerOfflineComponent } from './error/server-offline/server-offline.component';
import { RouterModule } from '@angular/router';
import { AccountService } from './services/account.service';
import { HeaderTemplateComponent } from "./common/header/header-template/header-template.component";

@Component({
    selector: 'app-root',
    imports: [
        RouterOutlet,
        RouterModule,
        HeaderComponent,
        CommonModule,
        NgIf,
        ServerOfflineComponent,
        HeaderTemplateComponent
    ],
    templateUrl: './app.component.html',
    styleUrls: ['./app.component.css']
})
export class AppComponent implements OnInit {
  title = 'EduConnect';
  users: any;
  isServerRunning = false;
private AccountService = inject(AccountService);
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
  
  setCurrentUser() {
    const userString = localStorage.getItem('user');
    if (userString) {  // Check if userString is not null
      const user = JSON.parse(userString);
      this.AccountService.CurrentUser.set(user);
    } else {
      console.log('No user found in localStorage');
    }
  }
  
}
