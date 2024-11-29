import {
  Component,
  EventEmitter,
  Input,
  Output,
  Type,
  ViewChild,
} from '@angular/core';
import { ReactiveFormsModule, FormControl, Validators } from '@angular/forms';
import { TextInputComponentComponent } from '../../input/text/text-input-component/text-input-component.component';
@Component({
  selector: 'app-email-input',
  imports: [TextInputComponentComponent, ReactiveFormsModule],
  standalone: true,
  templateUrl: './email-input.component.html',
  styleUrl: './email-input.component.css',
})
export class EmailInputComponent {
  emailLabel = 'Email';
  emailPlaceholder = 'Enter your email here...';
  emailInputId = 'emailInputId';

  //Add email validation
  @Input() emailFormControl: FormControl = new FormControl('');

  @Input() emailWarning: string = '';

  @Output() emailInput = new EventEmitter<Event>();

  handleTextInput(event: Event) {
    this.emailInput.emit(event);
  }
}
