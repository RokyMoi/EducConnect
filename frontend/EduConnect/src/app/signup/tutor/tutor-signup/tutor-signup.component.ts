import { Component, inject, Input, OnInit } from '@angular/core';
import { RouterModule } from '@angular/router';
import { HeaderTemplateComponent } from '../../../common/header/header-template/header-template.component';
import { TextInputComponentComponent } from '../../../common/input/text/text-input-component/text-input-component.component';
import { EmailInputComponent } from '../../../common/email/email-input/email-input.component';
import { PasswordInputComponent } from '../../../common/password/password-input/password-input.component';
import { SubmitButtonComponent } from '../../../common/button/submit-button/submit-button.component';
import { AccountService } from '../../../services/account.service';
import {
  FormControl,
  FormGroup,
  Validators,
  ReactiveFormsModule,
  ValidatorFn,
  AbstractControl,
  ValidationErrors,
} from '@angular/forms';
import {MatProgressBarModule} from '@angular/material/progress-bar';
@Component({
  selector: 'app-tutor-signup',
  standalone: true,
  imports: [
    RouterModule,
    HeaderTemplateComponent,
    TextInputComponentComponent,
    EmailInputComponent,
    PasswordInputComponent,
    SubmitButtonComponent,
    ReactiveFormsModule,
  ],
  templateUrl: './tutor-signup.component.html',
  styleUrl: './tutor-signup.component.css',
})
export class TutorSignupComponent {
  buttonText = 'Register';
  emailWarning: string = '';
  passwordWarning: string = '';

  formNotValidText: string = '';
  signinForm = new FormGroup({
    email: new FormControl('', [Validators.required, Validators.email]),
    password: new FormControl('', [
      Validators.required,
      Validators.minLength(8),
    ]),
  });

  private AccountService = inject(AccountService);
  tutorSignupModel = {
    email: '',
    password: '',
  };

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
  }

  handleSubmitButton($event: Event) {
    if (this.signinForm.invalid) {
      this.formNotValidText =
        'Please input valid email and password to continue';
    }
    if (this.signinForm.valid) {
      this.formNotValidText = '';
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
      this.AccountService.registerUserAsTutor(
        this.signinForm.controls.email.value,
        this.signinForm.controls.password.value
      ).subscribe({
        next: (response) => {
          console.log(response);
          this.formNotValidText = (response as any).message;
        },
        error: (error) => {
          console.log('Error occurred during registration: ', error);
          console.log(error.error.message);
          this.formNotValidText = error.error.message;
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
        passwordStrength += 1;
      }
      //Check if the password has at least one lowercase letter
      if (/[a-z]/.test(value)) {
        passwordStrength += 2;
      }
      //Check if the password has at least one number
      if (/[0-9]/.test(value)) {
        passwordStrength += 3;
      }
      //Check if the password has at least one special character
      if (/[!@#$%^&*]+/.test(value)) {
        passwordStrength += 4;
      }
    }

    console.log(passwordStrength);
    return passwordStrength;
  }
}
