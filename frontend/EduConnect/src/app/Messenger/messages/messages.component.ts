import { Component, inject, NgModule, OnInit } from '@angular/core';
import { MessageService } from '../../services/message.service';
import { NgModel } from '@angular/forms';
import { RouterLink } from '@angular/router';
import { TimeInterval } from 'rxjs/internal/operators/timeInterval';

@Component({
  selector: 'app-messages',
  standalone:true,
  imports: [RouterLink],
  templateUrl: './messages.component.html',
  styleUrl: './messages.component.css'
})
export class MessagesComponent implements OnInit {
message: any;
GetRouter(message: any) {
  
}

  messageService = inject(MessageService);
  Container = 'Outbox'
  pageNumber=1;
  messages: any[] = []; 
  pageSize=5;

  setContainerAndLoad(container: string): void {
    this.Container = container;
    this.LoadMessages();
  }
   
  ngOnInit(): void {
    this.LoadMessages();
  }
  timeAgo(date: Date): string {
    const now = new Date();
    const then = new Date(date); // Koristi Date objekat direktno
    const seconds = Math.floor((now.getTime() - then.getTime()) / 1000);

    const minutes = Math.floor(seconds / 60);
    const hours = Math.floor(minutes / 60);
    const days = Math.floor(hours / 24);
    const months = Math.floor(days / 30); // Jednostavna logika za mesece
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
  LoadMessages(){
    this.messageService.getMessages(this.pageNumber,this.pageSize,this.Container);
  }
pageChanged(event:any){
  if(this.pageNumber !== event.page){
    this.pageNumber = event.page;
    this.LoadMessages();
  }

}
}
