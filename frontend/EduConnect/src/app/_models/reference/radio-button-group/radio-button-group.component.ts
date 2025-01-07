import { NgFor } from '@angular/common';
import { Component, EventEmitter, Input, Output } from '@angular/core';
import { Form, FormControl, ReactiveFormsModule } from '@angular/forms';

@Component({
  standalone: true,
  selector: 'app-radio-button-group',
  imports: [ReactiveFormsModule, NgFor],
  templateUrl: './radio-button-group.component.html',
  styleUrl: './radio-button-group.component.css',
})
export class RadioButtonGroupComponent {
  @Input() options: { name: string; value: string }[] = [];
  @Input() fieldsetTitle: string = 'Title';
  @Input() formControl: FormControl = new FormControl('');
  @Input() warning: string = '';
  @Input() warningColor: string = 'red';

  @Output() selectedOptionChanged = new EventEmitter<string>();

  onValueChange(event: Event, newValue: string) {
    this.selectedOptionChanged.emit(newValue);
  }
}
