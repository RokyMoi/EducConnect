import { Component, Input } from '@angular/core';
import { RouterModule } from '@angular/router';
import { HeaderTemplateComponent } from '../../../common/header/header-template/header-template.component';
import { TextInputComponentComponent } from '../../../common/input/text/text-input-component/text-input-component.component';
import { EmailInputComponent } from "../../../common/email/email-input/email-input.component";
import { PasswordInputComponent } from "../../../common/password/password-input/password-input.component";
@Component({
  selector: 'app-tutor-signup',
  standalone: true,
  imports: [RouterModule, HeaderTemplateComponent, TextInputComponentComponent, EmailInputComponent, PasswordInputComponent],
  templateUrl: './tutor-signup.component.html',
  styleUrl: './tutor-signup.component.css',
})
export class TutorSignupComponent {}
