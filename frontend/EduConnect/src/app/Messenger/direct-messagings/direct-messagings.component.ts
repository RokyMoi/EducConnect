import { Component, inject, OnInit } from '@angular/core';
import { MessageService } from '../../services/message.service';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Message } from '../../models/messenger/message';
import { AccountService } from '../../services/account.service';
import { NgFor, NgIf } from '@angular/common';
import { Router, RouterLink } from '@angular/router';
import { SendMessageComponent } from "../send-message/send-message.component";

@Component({
  selector: 'app-direct-messagings',
  standalone:true,
  imports: [NgFor, NgIf, RouterLink, SendMessageComponent],
  templateUrl: './direct-messagings.component.html',
  styleUrls: ['./direct-messagings.component.css'],
})
export class DirectMessagingsComponent implements OnInit {
HandleThis($event: boolean) {
this.emitterStatus = $event;
}
OpenModule() {
// this.emitterStatus=true;
this.router.navigateByUrl("/ListOfUsers");
}
  constructor(private router: Router) {}
  emitterStatus:boolean=false;


  messageService = inject(MessageService);
  AccountService = inject(AccountService);
  http = inject(HttpClient);
  photourl = '';
  UserPhotoEmai = '';
  messagesArray: any[] = [];


  private userPhotosCache: Map<string, string> = new Map();

  ngOnInit(): void {
    this.LoadMessages();
    console.log("Current User Email: ",this.AccountService.CurrentUser()?.Email)
  }

  OpenAThread(message: any): void {
    this.router.navigate(['/studentMessageThread', message.id], {
      queryParams: {
        senderEmail: message.senderEmail,
        recipientEmail: message.recipientEmail,
      },
    });
  }

  InitiliazeUserForPhoto(message: any) {
    console.log("Current User Email: ",this.AccountService.CurrentUser()?.Email)
    const currentUserEmail = this.AccountService.CurrentUser()?.Email;
    this.UserPhotoEmai =
      currentUserEmail === message.senderEmail
        ? message.recipientEmail
        : message.senderEmail;

    if (!this.userPhotosCache.has(this.UserPhotoEmai)) {
      this.GetImageForUser(this.UserPhotoEmai);
    } else {
      this.photourl = this.userPhotosCache.get(this.UserPhotoEmai) || '';
    }
  }

  GetImageForUser(email: string) {
    let headers = new HttpHeaders();
    const token = this.AccountService.getAccessToken();

    if (token) {
      headers = headers.append('Authorization', `Bearer ${token}`);
    } else {
      console.error('No access token found.');
    }

    this.http
      .get<{ data: { url: string } }>(
        `http://localhost:5177/Photo/GetPhotoForUser/${email}`,
        { headers }
      )
      .subscribe({
        next: (response) => {
          this.photourl = response.data.url;
          this.userPhotosCache.set(email, this.photourl);
          console.log(response);
        },
        error: (error) => {
          console.error('Error fetching photo:', error);
        },
      });
  }

  LoadMessages() {
    let headers = new HttpHeaders();
    const token = this.AccountService.getAccessToken();

    if (token) {
      headers = headers.append('Authorization', `Bearer ${token}`);
    } else {
      console.error('No access token found.');
    }

    this.http
      .get<Message[]>(
        'http://localhost:5177/Messenger/GetLastMessagesForDirectMessaging',
        { headers }
      )
      .subscribe({
        next: (response) => {
          this.messagesArray = response;
        },
        error: (error) => {
          console.error('Error loading messages:', error);
        },
      });
  }

  timeAgo(date: Date): string {
    const now = new Date();
    const then = new Date(date);
    const seconds = Math.floor((now.getTime() - then.getTime()) / 1000);
    const minutes = Math.floor(seconds / 60);
    const hours = Math.floor(minutes / 60);
    const days = Math.floor(hours / 24);
    const months = Math.floor(days / 30);
    const years = Math.floor(months / 12);

    if (seconds < 60) {
      return `${seconds} seconds ago`;
    } else if (minutes < 60) {
      return `${minutes} minutes ago`;
    } else if (hours < 24) {
      return `${hours} hours ago`;
    } else if (days < 30) {
      return `${days} days ago`;
    } else if (months < 12) {
      return `${months} months ago`;
    } else {
      return `${years} years ago`;
    }
  }
}
