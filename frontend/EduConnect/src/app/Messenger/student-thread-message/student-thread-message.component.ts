import { Component, inject, OnDestroy, OnInit } from '@angular/core';
import { Message } from '../../_models/messenger/message';
import { MessageService } from '../../services/message.service';
import { ActivatedRoute, Router } from '@angular/router';
import { AccountService } from '../../services/account.service';
import { NgClass, NgFor, NgForOf, NgIf } from '@angular/common';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { FormsModule } from '@angular/forms';


@Component({
  selector: 'app-student-thread-message',
  standalone: true,
  imports: [NgIf,NgFor,NgForOf,NgClass,FormsModule],
  templateUrl: './student-thread-message.component.html',
  styleUrls: ['./student-thread-message.component.css'],
})
export class StudentThreadMessageComponent implements OnInit,OnDestroy {
  router = inject(Router);
  userForPhoto:any;
  accountService = inject(AccountService);
GetBackToDirectMessage() {
window.history.back();
}
  messageService = inject(MessageService);

  threadId!: string;
  senderEmail!: string;
  recipientEmail!: string;
  messages: Message[] = [];
    http = inject(HttpClient);
    messageContent='';
      photourl="";

  constructor(private route: ActivatedRoute) {}
  ngOnDestroy(): void {
    this.messageService.StopHubConnection();
  }

  ngOnInit(): void {
   
    this.threadId = this.route.snapshot.paramMap.get('id')!;

    
    this.route.queryParams.subscribe((params) => {
      this.senderEmail = params['senderEmail'];
      this.recipientEmail = params['recipientEmail'];

      console.log('Thread ID:', this.threadId);
      console.log('Sender Email:', this.senderEmail);
      console.log('Recipient Email:', this.recipientEmail);

      var currentUser = this.accountService.CurrentUser();
      if( currentUser?.Email == this.senderEmail){
        this.messageService.ceateHubConnection(currentUser, this.recipientEmail);
      }
      if(currentUser?.Email == this.recipientEmail){
        this.messageService.ceateHubConnection(currentUser, this.senderEmail);
      
      }
      // this.loadMessages(this.recipientEmail);
      const currentUserEmail = this.accountService.CurrentUser()?.Email;
      this.userForPhoto = (currentUserEmail === this.senderEmail) 
        ? this.recipientEmail 
        : this.senderEmail;

      

    this.GetImageForUser(this.userForPhoto);
    });
  }
  SendMessage() {
    this.messageService.SendMessageToUser(this.recipientEmail, this.messageContent)
      .then(() => {
        this.messageContent = ''; 
      })
      .catch(error => {
        console.error("Error sending message:", error);
      });
  }
  GetImageForUser(email:string): string{
      let headers = new HttpHeaders();
  const token = this.accountService.getAccessToken();
  
  if (token) {
    headers = headers.append('Authorization', `Bearer ${token}`);
  } else {
    console.error('No access token found.');
  }
  
  
  
  this.http.get<{data: {url:string}}>(`http://localhost:5177/Photo/GetPhotoForUser/${email}`, { headers })
    .subscribe({
      next: (response) => {
        this.photourl=response.data.url;
        console.log(response);
      },
      error: (error) => {
        
        console.error('Error fetching photo:', error);
      }
  
    });
    return this.photourl;

  }
  timeAgo(date: Date): string {
    const seconds = Math.floor((new Date().getTime() - new Date(date).getTime()) / 1000);
    const minutes = Math.floor(seconds / 60);
    const hours = Math.floor(minutes / 60);
    const days = Math.floor(hours / 24);

    if (days > 0) return `${days} day(s) ago`;
    if (hours > 0) return `${hours} hour(s) ago`;
    if (minutes > 0) return `${minutes} minute(s) ago`;
    return `${seconds} second(s) ago`;
  }
  

  // loadMessages(email:string): void {
    

  //   if (!email) {
  //     console.error('Error: Recipient email was not set');
  //     return;
  //   }

  //   this.messageService.getMessageThread(email).subscribe({
  //     next: (response) => {
  //       this.messages = response;
  //       console.log('Messages loaded:', this.messages);
       
  //       console.log('Service Dule Savic: ',this.messageService.messageThread())
  //     },
  //     error: (err) => console.error('Error loading messages:', err),
  //   });
  // }
}
