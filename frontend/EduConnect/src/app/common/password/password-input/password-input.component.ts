import { Component, EventEmitter, Input, Output } from '@angular/core';
import { TextInputComponentComponent } from '../../input/text/text-input-component/text-input-component.component';
import {
  FormControl, ReactiveFormsModule
 } from '@angular/forms';
@Component({
  selector: 'app-password-input',
  standalone: true,
  imports: [TextInputComponentComponent, ReactiveFormsModule],
  templateUrl: './password-input.component.html',
  styleUrl: './password-input.component.css',
})
export class PasswordInputComponent {
  passwordLabel = 'Password';
  passwordPlaceholder = 'Enter your password here...';
  passwordInputId = 'passwordInputId';
  type = 'password';

  @Input() passwordWarning: string = '';
  @Input() passwordFormControl: FormControl = new FormControl('');
  
  @Output() passwordInput = new EventEmitter<Event>();

  handlePasswordInput(event: Event) {
    this.passwordInput.emit(event);
  }
}
