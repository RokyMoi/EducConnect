import { Component, inject, OnInit } from '@angular/core';
import { FormControl, FormGroup, FormsModule, ReactiveFormsModule, Validators } from '@angular/forms';
import { AccountService } from '../../../services/account.service';
import { NgIf } from '@angular/common';

@Component({
  selector: 'app-register-student',
  standalone: true,
  imports: [ReactiveFormsModule,NgIf],
  templateUrl: './register-student.component.html',
  styleUrl: './register-student.component.css'
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

  constructor() {
    this.registerForm = new FormGroup({
      firstName: new FormControl('', Validators.required),
      lastName: new FormControl('', Validators.required),
      username: new FormControl('', Validators.required),
      email: new FormControl('', [Validators.required, Validators.email]),
      password: new FormControl('', [
        Validators.required,
        Validators.minLength(8),
        Validators.pattern(/^(?=.*[A-Z])/),
        Validators.pattern(/^(?=.*[a-z])/),
        Validators.pattern(/^(?=.*\d)/),
        Validators.pattern(/^(?=.*[@$!%*?&#])/)
      ]),
      phoneNumber: new FormControl('', Validators.required),
      phoneNumberCountryCode: new FormControl('', Validators.required),
      CountryOfOrigin: new FormControl('', Validators.required)
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
        },
        error: (err) => {
          console.error('Registration error:', err);
        }
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
