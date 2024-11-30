import { Component, inject, OnInit } from '@angular/core';
import {
  FormControl,
  FormGroup,
  FormsModule,
  ReactiveFormsModule,
  Validators,
} from '@angular/forms';
import { AccountService } from '../../../services/account.service';
import { NgIf } from '@angular/common';
import { Router } from '@angular/router';



@Component({
  selector: 'app-register-student',
  standalone: true,
  imports: [ReactiveFormsModule, NgIf],
  templateUrl: './register-student.component.html',
  styleUrl: './register-student.component.css',
})
export class RegisterStudentComponent implements OnInit {
  private accountService = inject(AccountService);
  registerForm: FormGroup;

  firstNameWarning = 'First name is required';
  LastNameWarning = 'Last name is required';
  usernameWarning = 'Username is required';
  emailWarning = 'Email is required';
  passwordWarning = 'Password is required';
  PhoneWarning = 'Phone number is required';
  PhoneCountryCodeWarning = 'Country code is required';
  CountryWarning = 'Country of origin is required';
  LoginErrorMessage='';
  routerNav = inject(Router);
  constructor() {
  
    this.registerForm = new FormGroup({
      firstName: new FormControl('', Validators.required),
      lastName: new FormControl('', Validators.required),
      username: new FormControl('', Validators.required),
     
       email : new FormControl('', {
    validators: [Validators.required, Validators.email],
    updateOn: 'blur',
  }),
      password: new FormControl('', [
        Validators.required,
        Validators.minLength(8),
        Validators.pattern(/^(?=.*[A-Z])/),
        Validators.pattern(/^(?=.*[a-z])/),
        Validators.pattern(/^(?=.*\d)/),
        Validators.pattern(/^(?=.*[@$!%*?&#])/),
      ]),
      phoneNumber: new FormControl('', [
        Validators.required,
        Validators.pattern(/^\+?[1-9]\d{1,14}$/),
      ]),
    
      phoneNumberCountryCode: new FormControl('', [
        Validators.required,
        Validators.pattern(/^\d+$/), 
      ]),
    
      CountryOfOrigin: new FormControl('', [
        Validators.required,
        Validators.pattern(/^[a-zA-Z\s]+$/), 
      ])
    });
  }

  ngOnInit(): void {}

  onSubmit(): void {
    if (this.registerForm.invalid) {
      this.showValidationOnSubmit();
      console.log('Form is invalid');
    } else {
      console.log('Form is valid, submitting...');
      this.accountService.register(this.registerForm.value).subscribe({
        next: (response) => {
          console.log('Registration successful:', response);
          this.routerNav.navigateByUrl('/student-dashboard');
        },
        error: (err) => {
          console.error('Registration error:', err);
          this.LoginErrorMessage = err.error;
          
          
          
        },
      });
    }
  }

  showValidationOnSubmit(): void {
    Object.keys(this.registerForm.controls).forEach((key) => {
      const control = this.registerForm.get(key);
      if (control?.invalid) {
        control.markAsTouched();
      }
    });
  }
}
