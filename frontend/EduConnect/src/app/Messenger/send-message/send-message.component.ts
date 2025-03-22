import { NgIf } from '@angular/common';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Component, EventEmitter, inject, Output } from '@angular/core';
import { FormBuilder, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { MessageService } from '../../services/message.service';
import { AccountService } from '../../services/account.service';
import { Message } from '../../_models/messenger/message';

@Component({
  selector: 'app-send-message',
  standalone: true,
  imports: [ReactiveFormsModule, NgIf],
  templateUrl: './send-message.component.html',
  styleUrls: ['./send-message.component.css']
})
export class SendMessageComponent {
  messageForm: FormGroup;
  @Output() FormStatus= new EventEmitter<boolean>();
  SendMessageUrl="http://localhost:5177/Messenger/CreateMessageForUser";
  http=inject(HttpClient);
  messageService = inject(MessageService)
  accountService=inject(AccountService);
  messagesArray: any;


  constructor(private fb: FormBuilder, private router: Router) {
    this.messageForm = fb.group({
      email: ['', [Validators.required, Validators.email]],  
      content: ['', [Validators.required, Validators.minLength(1)]]
    });
  }

  get email() {
    return this.messageForm.get('email');
  }

  get content() {
    return this.messageForm.get('content');
  }
LoadMessages() {
    let headers = new HttpHeaders();
    const token = this.accountService.getAccessToken();

    if (token) {
      headers = headers.append('Authorization', `Bearer ${token}`);
    } else {
      console.error('No access token found.');
    }

    this.http.get<Message[]>("http://localhost:5177/Messenger/GetLastMessagesForDirectMessaging", { headers })
      .subscribe({
        next: (response) => {
          this.messagesArray = response;
        },
        error: (error) => {
          console.error('Error loading messages:', error);
        }
      });
  }
  onBack() {
    
    this.FormStatus.emit(false);
  }

  onSubmit() {
    if (this.messageForm.valid) {
      console.log('Form Submitted', this.messageForm.value);
     
        this.messageService.SendMessageToUser(this.messageForm.controls['email'].value,this.messageForm.controls['content'].value ) .then(() => {
       
        })
        .catch(error => {
          console.error("Error sending message:", error);
        });
      
     
    } else {
      console.error('Form is invalid');
    }
  }
}
