import { Component, inject, OnInit } from '@angular/core';
import {
  FormControl,
  FormGroup,
  FormsModule,
  ReactiveFormsModule,
  Validators,
} from '@angular/forms';
import { AccountService } from '../../../services/account.service';
import { NgForOf, NgIf } from '@angular/common';
import { Router } from '@angular/router';
import { VerifyCodeComponent } from "../../tutor/verify-code/verify-code.component";
import { HttpClient } from '@angular/common/http';



@Component({
  selector: 'app-register-student',
  standalone: true,
  imports: [ReactiveFormsModule, NgIf, VerifyCodeComponent,NgForOf],
  templateUrl: './register-student.component.html',
  styleUrl: './register-student.component.css',
})
export class RegisterStudentComponent implements OnInit {
  private accountService = inject(AccountService);
  registerForm: FormGroup;
  isFloatingVisible: boolean = false;


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
  countries: any;
  http = inject(HttpClient);
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
       
      ]),
    
      phoneNumberCountryCode: new FormControl('', [
        Validators.required,
      
      ]),
    
      CountryOfOrigin: new FormControl('', Validators.required),
    });
  }

  ngOnInit(): void {
    this.http.get("http://localhost:5177/country/all").subscribe({
      next: (response: any) => {
      
        if (response && response.data && Array.isArray(response.data.countries)) {
          this.countries = response.data.countries;
        } else {
          console.error('Expected data.countries not found');
        }
      },
      error: (err) => {
        console.error('Error loading countries:', err);
      }
    });

  }

  onSubmit(): void {
    console.log('Form values:', this.registerForm.value); 
    if (this.registerForm.invalid) {
      this.showValidationOnSubmit();
      console.log('Form is invalid');
    } else {
      console.log('Form is valid, submitting...');
      const requestBody = {
        firstName: this.registerForm.get('firstName')?.value,
        lastName: this.registerForm.get('lastName')?.value,
        email: this.registerForm.get('email')?.value,
        password: this.registerForm.get('password')?.value,
        username: this.registerForm.get('username')?.value,
        phoneNumberCountryCodeCountryId: this.registerForm.get('phoneNumberCountryCode')?.value,
        phoneNumber: this.registerForm.get('phoneNumber')?.value,
        countryOfOriginCountryId: this.registerForm.get('CountryOfOrigin')?.value
      };
  
      this.accountService.register(requestBody).subscribe({
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
  toggleFloatingBox() {
    this.isFloatingVisible = !this.isFloatingVisible;
    console.log('Is floating visible:', this.isFloatingVisible);
  }
  showValidationOnSubmit(): void {
    Object.keys(this.registerForm.controls).forEach((key) => {
      const control = this.registerForm.get(key);
      if (control?.invalid) {
        console.log(`${key} is invalid:`, control.errors);
        control.markAsTouched();
      }
    });
  }
}
