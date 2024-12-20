import { Component, inject, OnInit } from '@angular/core';
import { AccountService } from '../../services/account.service';
import { Router } from '@angular/router';
import { HttpClient } from '@angular/common/http';

@Component({
  selector: 'app-student-profile',
  templateUrl: './student-profile.component.html',
  styleUrl: './student-profile.component.css'
})
export class StudentProfileComponent implements OnInit{
  ngOnInit(): void {
    this.GetStudentInformations();
  }
  AccService = inject(AccountService);
  http = inject(HttpClient);
student:any;
  ruter = inject(Router);
  profilePicture:any;
  LogoutFromProfile(){
    this.AccService.logout();
    this.ruter.navigate(['/']);
  }
  GetStudentInformations() {
    
    const token = this.AccService.getAccessToken();

   
    const headers = {
        'Authorization': `Bearer ${token}`,
    };

    const LinkProfile = 'http://localhost:5177/api/Student/getCurrentStudentForProfile';
    const ProfilePictureLink =`http://localhost:5177/Photo/GetCurrentUserProfilePicture`;

   
    this.http.get(LinkProfile, { headers }).subscribe({
        next: (response) => {
            this.student = response;
            console.log("Fetched student data:", this.student);
        },
        error: (err) => {
            console.error("Error fetching student data:", err);
        }
    });
    this.http.get<{data: {url:string}}>(ProfilePictureLink,{headers}).subscribe({
      next: (picture)=>{
         this.profilePicture = picture.data.url;
         console.log("Fetched data from PhotosPerson" + this.profilePicture);
      },
      error: err=> {
        console.log("Error fetching profile picture data: " + err);
      }

    });
}
}

