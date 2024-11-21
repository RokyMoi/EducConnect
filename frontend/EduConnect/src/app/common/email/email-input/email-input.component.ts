import { Component } from '@angular/core';
import { TextInputComponentComponent } from '../../input/text/text-input-component/text-input-component.component';
@Component({
  selector: 'app-email-input',
  standalone: true,
  imports: [TextInputComponentComponent],
  templateUrl: './email-input.component.html',
  styleUrl: './email-input.component.css',
})
export class EmailInputComponent {
  emailLabel = 'Email';
  emailPlaceholder = 'Enter your email here...';
  emailInputId = 'emailInputId';
}
