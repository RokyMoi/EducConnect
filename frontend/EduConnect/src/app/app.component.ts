import { Component, inject, OnInit } from '@angular/core';
import { RouterOutlet } from '@angular/router';
import { HeaderComponent } from './landingPage/header/header.component';
import { HttpClient } from '@angular/common/http';
import { BodyComponent } from "./landingPage/body/body.component";
@Component({
  selector: 'app-root',
  standalone: true,
  imports: [RouterOutlet, HeaderComponent, BodyComponent],
  templateUrl: './app.component.html',
  styleUrl: './app.component.css',
})
export class AppComponent implements OnInit {
  title = 'EduConnect';
  http = inject(HttpClient);
  users: any;

  ngOnInit(): void {
    console.log('AppComponent initialized');
    this.http.get('http://localhost:8080/api/v1/users/1').subscribe(
      (response) => {
        this.users = response;
        console.log('User:', this.users);
      },
      (error) => {
        console.error('Error fetching user:', error);
      }
    );
  }
}
