import { Component, inject, OnDestroy, OnInit } from '@angular/core';
import { Message } from '../../models/messenger/message';
import { MessageService } from '../../services/message.service';
import { ActivatedRoute, Router } from '@angular/router';
import { AccountService } from '../../services/account.service';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { FormsModule } from '@angular/forms';
import { CommonModule, NgClass, NgForOf } from '@angular/common';

@Component({
  selector: 'app-student-thread-message',
  standalone: true,
  imports: [CommonModule, FormsModule],
  templateUrl: './student-thread-message.component.html',
  styleUrls: ['./student-thread-message.component.css'],
})
export class StudentThreadMessageComponent implements OnInit, OnDestroy {
  router = inject(Router);
  userForPhoto: any;
  accountService = inject(AccountService);
  messageService = inject(MessageService);
  http = inject(HttpClient);

  threadId!: string;
  senderEmail!: string;
  recipientEmail!: string;
  messages: Message[] = [];
  messageContent = '';
  photourl = '';

  constructor(private route: ActivatedRoute) {}

  ngOnInit(): void {
    this.threadId = this.route.snapshot.paramMap.get('id')!;
    this.route.queryParams.subscribe((params) => {
      this.senderEmail = params['senderEmail'];
      this.recipientEmail = params['recipientEmail'];

      const currentUser = this.accountService.CurrentUser();
      if (currentUser?.Email === this.senderEmail) {
        this.messageService.ceateHubConnection(
          currentUser,
          this.recipientEmail
        );
      }
      if (currentUser?.Email === this.recipientEmail) {
        this.messageService.ceateHubConnection(currentUser, this.senderEmail);
      }

      const currentUserEmail = this.accountService.CurrentUser()?.Email;
      this.userForPhoto =
        currentUserEmail === this.senderEmail
          ? this.recipientEmail
          : this.senderEmail;
      this.getImageForUser(this.userForPhoto);
    });

    // Directly assign the value of messageThread to the messages array
    this.messages = this.messageService.messageThread();
  }

  ngOnDestroy(): void {
    this.messageService.StopHubConnection();
  }

  sendMessage(): void {
    // Prevent sending empty messages
    if (this.messageContent.trim() === '') {
      alert('Message content cannot be empty');
      return;
    }

    const currentUserEmail = this.accountService.CurrentUser()?.Email;

    let recipientEmailToSend = this.recipientEmail;

    if (currentUserEmail === this.recipientEmail) {
      recipientEmailToSend = this.senderEmail;
    }

    this.messageService
      .SendMessageToUser(recipientEmailToSend, this.messageContent)
      .then(() => {
        this.messageContent = ''; // Clear the message input
      })
      .catch((error) => {
        console.error('Error sending message:', error);
      });
  }

  getImageForUser(email: string): void {
    let headers = new HttpHeaders();
    const token = this.accountService.getAccessToken();
    if (token) {
      headers = headers.append('Authorization', `Bearer ${token}`);
    }

    this.http
      .get<{ data: { url: string } }>(
        `http://localhost:5177/Photo/GetPhotoForUser/${email}`,
        { headers }
      )
      .subscribe({
        next: (response) => {
          this.photourl = response.data.url;
        },
        error: (error) => {
          console.error('Error fetching photo:', error);
        },
      });
  }

  timeAgo(date: Date): string {
    const seconds = Math.floor(
      (new Date().getTime() - new Date(date).getTime()) / 1000
    );
    const minutes = Math.floor(seconds / 60);
    const hours = Math.floor(minutes / 60);
    const days = Math.floor(hours / 24);

    if (days > 0) return `${days} day(s) ago`;
    if (hours > 0) return `${hours} hour(s) ago`;
    if (minutes > 0) return `${minutes} minute(s) ago`;
    return `${seconds} second(s) ago`;
  }

  GetBackToDirectMessage(): void {
    window.history.back();
  }
}
