import { Component, inject, OnInit } from '@angular/core';
import { AccountService } from '../../services/account.service';
import { MatIconModule } from '@angular/material/icon';
import { RouterLink, RouterLinkActive, Router } from '@angular/router';
import { MatCardModule } from '@angular/material/card';
import { HttpClient } from '@angular/common/http';
import { PersonControllerService } from '../../services/shared/person-controller.service';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-landing-page-header',
  imports: [
    MatIconModule,
    RouterLink,
    RouterLinkActive,
    MatCardModule,
    CommonModule,
  ],
  templateUrl: './header.component.html',
  styleUrl: './header.component.css',
  standalone: true,
})
export class HeaderComponent implements OnInit {
  AccountService = inject(PersonControllerService);
  readonly currentUser = this.AccountService.CurrentUser;

  ngOnInit(): void {
    console.log('Header component initialized');
    this.GetProfilePicture();
    console.log('Current user:', this.AccountService.CurrentUser());
    
  }

  http = inject(HttpClient);
  profilePicture: any;
  profilePictureLink = `http://localhost:5177/Photo/GetCurrentUserProfilePicture`;

  title = 'EduConnect';
  GetProfilePicture() {
    const token = this.AccountService.getAccessToken();
    const headers = {
      Authorization: `Bearer ${token}`,
    };
    this.http
      .get<{ data: { url: string } }>(this.profilePictureLink, { headers })
      .subscribe({
        next: (response) => {
          this.profilePicture = response.data.url;
          console.log('Fetched profile picture URL: ' + this.profilePicture);
        },
        error: (err) => {
          console.error('Error fetching profile picture data: ' + err);
        },
      });
  }

  getUserRole() {
    return this.AccountService.CurrentUser()?.Role;
  }
}
