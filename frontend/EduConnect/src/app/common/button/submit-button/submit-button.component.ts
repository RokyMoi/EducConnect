import { Component, EventEmitter, Input, Output } from '@angular/core';

@Component({
  selector: 'app-submit-button',
  standalone: true,
  imports: [],
  templateUrl: './submit-button.component.html',
  styleUrl: './submit-button.component.css',
})
export class SubmitButtonComponent {
  @Input() submitButtonText: string = 'Submit';
  @Input() isButtonEnabled: boolean = true;

  @Output() buttonClick = new EventEmitter<Event>();

  handleButtonClick(event: Event) {
    this.buttonClick.emit(event);
  }
}
