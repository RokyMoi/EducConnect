import { HttpClient, HttpHeaders } from "@angular/common/http";
import { MessageService } from "../../services/message.service";
import { AccountService } from "../../services/account.service";
import { Component, inject, OnInit } from "@angular/core";
import { RouterLink } from "@angular/router";
import { Message } from "../../_models/messenger/message";

@Component({
  selector: 'app-messages',
  standalone: true,
  imports: [RouterLink],
  templateUrl: './messages.component.html',
  styleUrls: ['./messages.component.css']
})
export class MessagesComponent implements OnInit {
GetRouter(_t17: Message) {
throw new Error('Method not implemented.');
}
  message: any;
  Container = 'Outbox';
  pageNumber = 1;
  messages: any[] = []; 
  pageSize = 5;
  photourl = "";
  UserPhotoEmai = "";
  
  // Keš za slike korisnika
  private userPhotosCache: Map<string, string> = new Map();
  
  // Keš za poruke (ako želite da keširate stranice)
  private messagesCache: Map<string, any[]> = new Map();
  
  messageService = inject(MessageService);
  AccountService = inject(AccountService);
  http = inject(HttpClient);

  ngOnInit(): void {
    this.LoadMessages();
  }

  setContainerAndLoad(container: string): void {
    this.Container = container;
    this.LoadMessages();
  }

  InitiliazeUserForPhoto(message: any) {
    const currentUserEmail = this.AccountService.CurrentUser()?.Email;
    this.UserPhotoEmai = currentUserEmail === message.senderEmail
      ? message.recipientEmail
      : message.senderEmail;
    
    // Proverite da li je slika već keširana
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

    // Poziv za sliku korisnika
    this.http.get<{ data: { url: string } }>(`http://localhost:5177/Photo/GetPhotoForUser/${email}`, { headers })
      .subscribe({
        next: (response) => {
          this.photourl = response.data.url;
          this.userPhotosCache.set(email, this.photourl); // Keširaj sliku
          console.log(response);
        },
        error: (error) => {
          console.error('Error fetching photo:', error);
        }
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

  // Keširanje poruka za različite stranice
  LoadMessages() {
    // Koristite keširane poruke ako postoji
    const cacheKey = `${this.pageNumber}-${this.Container}`;
    if (this.messagesCache.has(cacheKey)) {
      this.messages = this.messagesCache.get(cacheKey)!;
    } else {
      this.messageService.getMessages(this.pageNumber, this.pageSize, this.Container);
    }
  }

  pageChanged(event: any) {
    if (this.pageNumber !== event.page) {
      this.pageNumber = event.page;
      this.LoadMessages();
    }
  }
}
