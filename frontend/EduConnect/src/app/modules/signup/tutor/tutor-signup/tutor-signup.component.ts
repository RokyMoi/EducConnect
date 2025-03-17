import { Component, inject, Input, OnInit } from '@angular/core';
import { RouterModule } from '@angular/router';

import {
  FormControl,
  FormGroup,
  Validators,
  ReactiveFormsModule,
  ValidatorFn,
  AbstractControl,
  ValidationErrors,
} from '@angular/forms';
import { VerifyCodeComponent } from '../verify-code/verify-code.component';
import { CommonModule } from '@angular/common';
import {
  MatProgressSpinner,
  ProgressSpinnerMode,
} from '@angular/material/progress-spinner';

import { HeaderTemplateComponent } from '../../../../common/header/header-template/header-template.component';
import { TextInputComponentComponent } from '../../../../common/input/text/text-input-component/text-input-component.component';
import { EmailInputComponent } from '../../../../common/email/email-input/email-input.component';
import { PasswordInputComponent } from '../../../../common/password/password-input/password-input.component';
import { SubmitButtonComponent } from '../../../../common/button/submit-button/submit-button.component';
import { AccountService } from '../../../../services/account.service';
import { ProgressBarComponent } from '../../../../common/output/progress-bar/progress-bar.component';

@Component({
  selector: 'app-tutor-signup',
  imports: [
    CommonModule,
    RouterModule,
    MatProgressSpinner,
    EmailInputComponent,
    PasswordInputComponent,
    SubmitButtonComponent,
    ReactiveFormsModule,
    ProgressBarComponent,
    VerifyCodeComponent,
  ],
  standalone: true,
  templateUrl: './tutor-signup.component.html',
  styleUrl: './tutor-signup.component.css',
})
export class TutorSignupComponent {
  buttonText = 'Register';
  emailWarning: string = '';
  passwordWarning: string = '';

  passwordStrength: number = 0;
  dataProcessingResult: string = '';
  dataProcessingResultColor: string = 'yellow';

  signinForm = new FormGroup({
    email: new FormControl('', [Validators.required, Validators.email]),
    password: new FormControl('', [
      Validators.required,
      Validators.minLength(8),
    ]),
  });

  //Variables for progress bar styling
  progressBarMarginBottom: string = '2px';
  progressBarWidth: string = '0%';
  progressBarColor: string = 'red';
  passwordStrengthDescription: string = '';
  passwordStrengthDescriptionMarginBottom: string = '12px';
  progressBarColorStrength: {
    [key: number]: string;
  } = {
    0: '#ff0000',
    10: '#ff4000',
    30: '#bb8b09',
    40: '#ffc000',
    50: '#ffff00',
    60: '#00b0ff',
    70: '#006ad4',
    80: '#00d1a2',
    90: '#86cf00',
    100: '#008000',
  };
  progressBarStrengthDescription: { [key: number]: string } = {
    0: 'Nothing',
    10: 'Barely secure',
    30: 'Very weak',
    40: 'Weak',
    50: 'Medium strength',
    60: 'Somewhat secure',
    70: 'Secure',
    80: 'Good strength',
    90: 'Very strong',
    100: 'Extremely strong',
  };

  //Variables for displaying verification code input component
  isFloatingVisible: boolean = false;
  private AccountService = inject(AccountService);
  tutorSignupModel = {
    email: '',
    password: '',
  };

  //Variable for loading spinner
  isDataBeingProcessed: boolean = false;
  spinnerColor: string = 'yellow';
  spinnerMode: ProgressSpinnerMode = 'indeterminate';

  handleEmailInput(event: Event) {
    this.signinForm.controls.email.setValue(
      (event.target as HTMLInputElement).value
    );

    if (this.signinForm.controls.email.invalid) {
      if (this.signinForm.controls.email.hasError('required')) {
        this.emailWarning = 'Email is required';
      }
      if (this.signinForm.controls.email.hasError('email')) {
        this.emailWarning = 'Please enter a valid email';
      }
    }
    if (this.signinForm.controls.email.valid) {
      this.emailWarning = '';
    }
  }

  handlePasswordInput(event: Event) {
    this.signinForm.controls.password.setValue(
      (event.target as HTMLInputElement).value
    );
    console.log(this.signinForm.controls.password.value);
    console.log(
      'Is password invalid: ' + this.signinForm.controls.password.invalid
    );
    console.log(this.signinForm.controls.password.hasError('required'));
    if (this.signinForm.controls.password.invalid) {
      if (this.signinForm.controls.password.hasError('required')) {
        this.passwordWarning = 'Password is required';
      }
      if (this.signinForm.controls.password.hasError('minlength')) {
        this.passwordWarning = 'Password must be at least 8 characters long';
      }
    }
    if (this.signinForm.controls.password.valid) {
      this.passwordWarning = '';
    }
    console.log(this.signinForm.controls.password.value);
    console.log('Password strength: ' + this.passwordStrengthValidator());
    this.passwordStrength = this.passwordStrengthValidator();
    console.log(this.passwordStrength);
    this.progressBarWidth = this.passwordStrength + '%';
    this.progressBarColor =
      this.progressBarColorStrength[this.passwordStrength];
    this.passwordStrengthDescription =
      this.progressBarStrengthDescription[this.passwordStrength];
  }

  handleSubmitButton($event: Event) {
    if (this.signinForm.invalid) {
      this.dataProcessingResultColor = 'red';
      this.dataProcessingResult =
        'Please input valid email and password to continue';
    }
    if (this.signinForm.valid) {
      this.dataProcessingResult = 'Please hold while we register you';
      this.dataProcessingResultColor = 'yellow';
      this.registerUserAsTutor();
    }
  }

  registerUserAsTutor() {
    console.log(this.signinForm.controls.email.value);
    console.log(this.signinForm.controls.password.value);

    //TODO: Make a request to the backend to register the user as a tutor
    if (
      this.signinForm.controls.email.value &&
      this.signinForm.controls.password.value
    ) {
      this.isDataBeingProcessed = true;
      this.AccountService.registerUserAsTutor(
        this.signinForm.controls.email.value,
        this.signinForm.controls.password.value
      ).subscribe({
        next: (response) => {
          this.dataProcessingResult = 'Registration successful';
          this.dataProcessingResultColor = 'green';

          console.log(response);
          this.isDataBeingProcessed = false;
          (response as any).success === 'true'
            ? this.toggleFloatingBox()
            : null;
        },
        error: (error) => {
          this.isDataBeingProcessed = false;
          console.log('Error occurred during registration: ', error);
          console.log(error.error.message);
          this.dataProcessingResult = error.error.message;
          this.dataProcessingResultColor = 'red';
        },
      });
    }
  }

  //Checks password strength and returns a score from 0 to 10
  //0 - no value
  //1 - has at least one uppercase letter
  //2 - has at least one lowercase letter
  //3 - has at least one number
  //4 - has at least one special character

  private passwordStrengthValidator() {
    const value = this.signinForm.controls.password.value;
    let passwordStrength: number = 0;
    if (value) {
      //Check if the password has at least one uppercase letter
      if (/[A-Z]/.test(value)) {
        passwordStrength += 10;
      }
      //Check if the password has at least one lowercase letter
      if (/[a-z]/.test(value)) {
        passwordStrength += 20;
      }
      //Check if the password has at least one number
      if (/[0-9]/.test(value)) {
        passwordStrength += 30;
      }
      //Check if the password has at least one special character
      if (/[!@#$%^&*]+/.test(value)) {
        passwordStrength += 40;
      }
    }

    console.log(passwordStrength);
    return passwordStrength;
  }

  toggleFloatingBox() {
    this.isFloatingVisible = !this.isFloatingVisible;
    console.log('Is floating visible:', this.isFloatingVisible);
  }
}
