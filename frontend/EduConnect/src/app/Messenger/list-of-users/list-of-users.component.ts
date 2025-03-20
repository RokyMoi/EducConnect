import { Component, inject, OnInit } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { NgFor } from '@angular/common';
import { AccountService } from '../../services/account.service';
import { Router } from '@angular/router';

@Component({
  selector: 'app-list-of-users',
  standalone: true,
  imports: [NgFor],
  templateUrl: './list-of-users.component.html',
  styleUrls: ['./list-of-users.component.css'] // Ispravljeno u "styleUrls"
})
export class ListOfUsersComponent implements OnInit {
NavigateCurrentUser() {
this.router.navigateByUrl("/direct-message");
}
  router = inject(Router);
OpenThread(Recipientemail:string) {
  var id = Math.floor(Math.random() * 1000000000);
  this.router.navigate(['/studentMessageThread', id], {
    queryParams: { senderEmail: this.accountService.CurrentUser()?.Email, recipientEmail: Recipientemail },
  });
}
  http = inject(HttpClient);
  accountService = inject(AccountService); // Pravilno ime promenljive (camelCase)
  users: any;

  ngOnInit(): void {
    this.LoadAllUsers();
  }

  LoadAllUsers(): void {
    let headers = new HttpHeaders();
    const token = this.accountService.getAccessToken(); 

    if (token) {
      headers = headers.append('Authorization', `Bearer ${token}`);
    } else {
      console.log('No token found for this user');
      return; 
    }

    this.http.get('http://localhost:5177/Messenger/GetUserForChatList', { headers }).subscribe({
      next: (response) => {
        this.users = response;
        console.log('Users that were fetched for list', this.users);
      },
      error: (err) => {
        console.error('Error while fetching users', err);
      }
    });
  }
}
