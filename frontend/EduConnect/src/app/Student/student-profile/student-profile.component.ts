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
  LogoutFromProfile(){
    this.AccService.logout();
    this.ruter.navigate(['/']);
  }
  GetStudentInformations() {
    console.log(this.AccService.CurrentUser()?.Email);
    const baseLink = `http://localhost:5177/api/Student/student/${this.AccService.CurrentUser()?.Email}`;
    this.http.get(baseLink).subscribe({
      next: (response) => {
        this.student = response;
        console.log("Fetched student data:", this.student);
      },
      error: (err) => {
        console.error("Error fetching student data:", err);
      }
    })
    
  }
}
