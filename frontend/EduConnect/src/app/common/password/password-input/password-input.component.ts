import { Component } from '@angular/core';
import { TextInputComponentComponent } from '../../input/text/text-input-component/text-input-component.component';
@Component({
  selector: 'app-password-input',
  standalone: true,
  imports: [TextInputComponentComponent],
  templateUrl: './password-input.component.html',
  styleUrl: './password-input.component.css',
})
export class PasswordInputComponent {
  passwordLabel = 'Password';
  passwordPlaceholder = 'Enter your password here...';
  passwordInputId = 'passwordInputId';
  type = 'password';
  passwordWarning = 'Warning';
}
