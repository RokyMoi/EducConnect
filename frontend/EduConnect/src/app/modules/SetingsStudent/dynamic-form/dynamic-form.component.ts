import { NgIf } from '@angular/common';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Component, EventEmitter, inject, Input, OnInit, Output } from '@angular/core';
import { FormBuilder, FormControl, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { AccountService } from '../../services/account.service';

@Component({
  selector: 'app-dynamic-form',
  standalone:true,
  imports: [ReactiveFormsModule,NgIf],
  templateUrl: './dynamic-form.component.html',
  styleUrl: './dynamic-form.component.css'
})
export class DynamicFormComponent implements OnInit {
ChangeFormStatus() {
this.FormStatus.emit(false);
}
  http = inject(HttpClient);
  accountService = inject(AccountService);
  dynamicForm!: FormGroup;
  @Input() valueNameChild: string = ''; 
  @Input() ApiLink: string = ''; 
  @Output() FormStatus = new EventEmitter<boolean>();
  @Output() FormType = new EventEmitter<string>();
    constructor(private fb: FormBuilder){}
  ngOnInit(): void {
    this.dynamicForm = this.fb.group({
      userInput: new FormControl('', [Validators.required]) 
    });
  }

  onSubmit() {
    if (this.dynamicForm.valid) {
      const userInputValue = this.dynamicForm.controls['userInput'].value;
      console.log('Form Submitted:', userInputValue);
      this.UploadingResult(userInputValue); 
      this.FormStatus.emit(false); 
    } else {
      console.error('Form is invalid.');
      this.dynamicForm.markAllAsTouched(); 
    }
  }
  UploadingResult(email: string) {
    const token = this.accountService.getAccessToken();
    const headers = new HttpHeaders({
      'Authorization': `Bearer ${token}`,
      'Content-Type': 'application/json'
    });
  
    const apiUrl = `${this.ApiLink}?input=${encodeURIComponent(email)}`;
  
    this.http.post(apiUrl, {}, { headers }).subscribe({
      next: () => {
        console.log("Successfully submitted.");
      },
      error: (err) => {
        console.error("Error occurred while sending request:", err);
      }
    });
  }
  
  }






