import { Component, inject, OnInit } from '@angular/core';
import { AccountService } from '../../services/account.service';
import { Router, RouterLink } from '@angular/router';
import { HttpClient } from '@angular/common/http';

@Component({
  selector: 'app-student-profile',
  standalone:true,
  imports:[RouterLink],
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

    const LinkProfile = 'http://localhost:5177/api/Student/getCurrentStudentWithPhoto';



    this.http.get(LinkProfile, { headers }).subscribe({
        next: (response) => {
            this.student = response;
            console.log("Fetched student data:", this.student);
        },
        error: (err) => {
            console.error("Error fetching student data:", err);
        }
    });

}
}

