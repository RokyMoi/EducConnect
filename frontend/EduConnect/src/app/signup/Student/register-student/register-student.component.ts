import { Component, inject, OnInit } from '@angular/core';
import { FormControl, FormGroup, FormsModule, ReactiveFormsModule, Validators } from '@angular/forms';
import { AccountService } from '../../../services/account.service';
import { NgIf } from '@angular/common';
import { Router } from '@angular/router';

@Component({
  selector: 'app-register-student',
  imports: [ReactiveFormsModule,NgIf],
  templateUrl: './register-student.component.html',
  styleUrl: './register-student.component.css',
  standalone: true,
})
export class RegisterStudentComponent {
  private AccountService = inject(AccountService);
  model = {
    firstname: '',
    lastname: '',
    username: '',
    email: '',
    password: '',
    phoneNumber: '',
    phoneNumberCountryCode: '',
    countryOfOrigin: '',
  };
  RegistrujSe() {
    this.AccountService.register(this.model).subscribe({
      next: (response) => {
        console.log(response);
      },
      error: (err) => {
        console.log('Error during registration:', err);
      },
    });
  }
}
