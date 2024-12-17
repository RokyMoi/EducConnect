import { Component, EventEmitter, Input, Output } from '@angular/core';

@Component({
  selector: 'app-submit-button',
  imports: [],
  standalone: true,
  templateUrl: './submit-button.component.html',
  styleUrl: './submit-button.component.css',
})
export class SubmitButtonComponent {
  @Input() submitButtonText: string = 'Submit';
  @Input() isButtonEnabled: boolean = true;
  @Input() buttonWidth: string = '100%';
  @Input() buttonBackgroundColor: string = 'blue';
  @Input() buttonTextColor: string = 'white';
  @Input() buttonBorder: string = 'none';
  @Input() buttonBorderRadius: string = '18px';
  @Input() buttonPadding: string = '12px';
  @Input() buttonFontSize: string = '1.5rem';
  @Input() buttonFontWeight: string = 'bold';
  @Input() buttonCursor: string = 'pointer';
  @Input() buttonTransition: string = 'all 0.3s ease-in-out';
  @Input() buttonTextAlign: string = 'center';
  @Input() buttonMargin: string = '0';
  @Input() buttonHoverBackgroundColor: string = 'rgb(2, 80, 176)';
  @Input() buttonHoverTextColor: string = 'white';
  @Input() buttonHoverBorder: string = 'none';
  @Input() buttonHoverBoxShadow: string = '4px 6px 0 rgba(63, 63, 63, 0.135)';
  @Input() buttonHoverMargin: string = '0';

  @Output() buttonClick = new EventEmitter<Event>();

  buttonStyles: any = {};

  handleButtonClick(event: Event) {
    this.buttonClick.emit(event);
  }

  handleMouseOver() {
    this.buttonStyles = {
      backgroundColor: this.buttonHoverBackgroundColor,
      color: this.buttonHoverTextColor,
      border: this.buttonHoverBorder,
      boxShadow: this.buttonHoverBoxShadow,
      margin: this.buttonHoverMargin,
    };
  }

  handleMouseLeave() {
    this.buttonStyles = {}; // Reset to default styles when mouse leaves
  }
}
