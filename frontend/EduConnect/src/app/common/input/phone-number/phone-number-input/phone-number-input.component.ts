import { Component, EventEmitter, Input, Output } from '@angular/core';
import { FormControl, ReactiveFormsModule } from '@angular/forms';

@Component({
  selector: 'app-phone-number-input',
  imports: [ReactiveFormsModule],
  templateUrl: './phone-number-input.component.html',
  styleUrl: './phone-number-input.component.css',
})
export class PhoneNumberInputComponent {
  @Input() label: string = '';
  @Input() placeholder: string = 'Enter your phone number here...';
  @Input() phoneNumberInputFormControl: FormControl = new FormControl('');
  @Input() warning: string = '';

  @Output() phoneNumberInput = new EventEmitter<Event>();

  handleTextInput(event: Event) {
    this.phoneNumberInput.emit(event);
  }
}
