import { Component, Input, OnInit } from '@angular/core';
import { RouterModule } from '@angular/router';
import { HeaderTemplateComponent } from '../../../common/header/header-template/header-template.component';
import { TextInputComponentComponent } from '../../../common/input/text/text-input-component/text-input-component.component';
import { EmailInputComponent } from '../../../common/email/email-input/email-input.component';
import { PasswordInputComponent } from '../../../common/password/password-input/password-input.component';
import { SubmitButtonComponent } from '../../../common/button/submit-button/submit-button.component';
import {
  FormControl,
  FormGroup,
  Validators,
  ReactiveFormsModule,
} from '@angular/forms';
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
  signinForm = new FormGroup({
    email: new FormControl('', [Validators.required, Validators.email]),
    password: new FormControl('', [
      Validators.required,
      Validators.minLength(8),
      Validators.pattern(/^(?=.*[A-Z])/), //Password must have at least one uppercase letter,
      Validators.pattern(/^(?=.*[a-z])/), //Password must have at least one lowercase letter,
      Validators.pattern(/^(?=.*\d)/), //Password must have at least one digit,
      Validators.pattern(/^(?=.*[@$!%*?&])/), //Password must have at least one special character from this set (@$!%*?&)
    ]),
  });

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
      if (!this.signinForm.controls.password.hasError('minlength')) {
        this.passwordWarning = '';
      }
      if (this.signinForm.controls.password.hasError('pattern')) {
        this.passwordWarning = 'Password not valid';
      }
    }
    if (this.signinForm.controls.password.valid) {
      this.passwordWarning = 'Password is OK';
    }
    console.log(this.signinForm.controls.password.value);
  }
}
