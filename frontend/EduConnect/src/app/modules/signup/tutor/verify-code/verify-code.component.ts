import { CommonModule } from '@angular/common';
import { Component, Input, inject } from '@angular/core';

import { FormControl, ReactiveFormsModule, Validators } from '@angular/forms';
import {
  MatProgressSpinnerModule,
  ProgressSpinnerMode,
} from '@angular/material/progress-spinner';

import { SubmitButtonComponent } from '../../../../common/button/submit-button/submit-button.component';
import { TextInputComponentComponent } from '../../../../common/input/text/text-input-component/text-input-component.component';
import { AccountService } from '../../../../services/account.service';
import { User } from '../../../../models/User';
import { Router } from '@angular/router';

@Component({
  selector: 'app-verify-code',
  imports: [
    CommonModule,
    SubmitButtonComponent,
    ReactiveFormsModule,
    MatProgressSpinnerModule,
  ],
  templateUrl: './verify-code.component.html',
  styleUrl: './verify-code.component.css',
  standalone: true,
})
export class VerifyCodeComponent {
  @Input() isFloatingVisible: boolean = false;
  @Input() toggleFloatingBox: Function = () => {};
  @Input() verificationCodeToCheck: string = '';
  @Input() warningText: string = this.verificationCodeToCheck;
  @Input() warningTextColor: string = 'red';
  @Input() emailToVerify: string = '';
  @Input() passwordForLogin: string = '';
  spinnerMode: ProgressSpinnerMode = 'indeterminate';
  spinnerColor: string = 'orange';
  isCheckActive: boolean = false;
  dataProcessingResult: string = '';
  dataProcessingResultColor: string = 'orange';
  dataProcessingFailed: boolean = false;
  resendButtonText: string = 'Resend';
  buttonMargin: string = '24px 0px';
  verificationCodeFromControl = new FormControl('', [
    Validators.required,
    Validators.minLength(8),
    Validators.maxLength(8),
  ]);

  loginResult: any;
  //AccountService injection, class for http request and response sending and receiving
  private AccountService = inject(AccountService);

  //Router injection, class for navigation between pages
  private routerNavigation = inject(Router);
  handleVerificationCodeValidation(): boolean {
    var isValid = true;

    if (this.verificationCodeFromControl.hasError('required')) {
      this.warningText = 'Verification code is required';
      isValid = false;
    }
    if (this.verificationCodeFromControl.hasError('minlength')) {
      this.warningText = 'Verification code must be 8 characters long';
      isValid = false;
    }
    if (this.verificationCodeFromControl.hasError('maxlength')) {
      this.warningText = 'Verification code must be 8 characters long';
      isValid = false;
    }
    if (this.verificationCodeFromControl.valid) {
      this.warningText = '';
      isValid = true;
    }
    return isValid;
  }

  checkVerificationCode() {
    if (
      this.handleVerificationCodeValidation() &&
      this.verificationCodeFromControl.value
    ) {
      console.log('Trying to check verification code');
      this.isCheckActive = true;
      this.dataProcessingResult = 'Verifying code...';
      this.dataProcessingResultColor = 'orange';
      console.log('Email: ' + this.emailToVerify);
      console.log(
        'Verification code: ' + this.verificationCodeFromControl.value
      );
      const response = this.AccountService.verifyEmail(
        this.emailToVerify,
        this.verificationCodeFromControl.value
      );
      response.subscribe({
        next: (response) => {
          console.log(response);
          this.isCheckActive = false; // Hide the spinner on success
          this.warningText =
            'Verification successful, please wait while we log you in';
          this.warningTextColor = 'green';
          this.dataProcessingResult = 'Verification successful';
          const userToLogin = {
            email: this.emailToVerify,
            password: this.passwordForLogin,
          };
          console.log('User email to login: ' + userToLogin.email);
          console.log('User password to login: ' + userToLogin.password);
          this.AccountService.login(userToLogin).subscribe({
            next: (loginResponse) => {
              console.log('Login successful: ', loginResponse);
              const currentUser = this.AccountService.CurrentUser();
              if (currentUser?.Role === 'student') {
                this.routerNavigation.navigateByUrl('/student-dashboard');
              }
              if (currentUser?.Role === 'tutor') {
                this.routerNavigation.navigateByUrl('/tutor-dashboard');
              }
            },
            error: (loginError) => {
              console.error('Login failed: ', loginError);
            },
          });
        },
        error: (error) => {
          console.error('Error verifying code:', error);
          this.warningText =
            error.error?.message +
              ', if this error persists, please use resend button' ||
            'Verification failed';
          this.isCheckActive = false; // Hide the spinner on error
          this.dataProcessingFailed = true;
        },
      });
    }
  }

  resendVerificationCode() {
    this.isCheckActive = true;
    this.dataProcessingResult = 'Resending verification code...';
    this.dataProcessingResultColor = 'orange';
    const response = this.AccountService.resendVerificationCode(
      this.emailToVerify
    );
    response.subscribe({
      next: (response) => {
        console.log(response);
        this.isCheckActive = false;
        this.warningText = 'Verification code has been resent';
        this.warningTextColor = 'green';
        this.dataProcessingFailed = false;
      },
      error: (error) => {
        console.log(error);
        this.isCheckActive = false;
        this.warningText = error.error.message;
      },
    });
  }
}
