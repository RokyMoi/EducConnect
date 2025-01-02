import { Component, inject, OnInit } from '@angular/core';
import { AccountService } from '../../services/account.service';
import { MatIconModule } from '@angular/material/icon';
import {  RouterLink, RouterLinkActive,Router} from '@angular/router';
import { MatCardModule } from '@angular/material/card';
import { HttpClient } from '@angular/common/http';



@Component({
  selector: 'app-landing-page-header',
  imports: [MatIconModule,RouterLink,RouterLinkActive,MatCardModule],
  templateUrl: './header.component.html',
  styleUrl: './header.component.css',
  standalone: true,
})
export class HeaderComponent implements OnInit{
  ngOnInit(): void {
    this.GetProfilePicture();
  }
  AccountService = inject(AccountService);
  http = inject(HttpClient);
  profilePicture: any;
  profilePictureLink = `http://localhost:5177/Photo/GetCurrentUserProfilePicture`;
  
  title = 'EduConnect';
 GetProfilePicture() {
  const token = this.AccountService.getAccessToken();
  const headers={
    Authorization: `Bearer ${token}`,
  }
  this.http.get<{ data: { url: string } }>(this.profilePictureLink, { headers }).subscribe({
    next: (response) => {
      this.profilePicture = response.data.url; 
      console.log("Fetched profile picture URL: " + this.profilePicture);
    },
    error: (err) => {
      console.error("Error fetching profile picture data: " + err);
      
    },
  });
  }
}


