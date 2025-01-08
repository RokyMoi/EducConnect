import { NgFor } from '@angular/common';
import {
  Component,
  EventEmitter,
  forwardRef,
  Input,
  Output,
} from '@angular/core';
import {
  Form,
  FormControl,
  NG_VALUE_ACCESSOR,
  ReactiveFormsModule,
} from '@angular/forms';

@Component({
  standalone: true,
  selector: 'app-radio-button-group',
  imports: [ReactiveFormsModule, NgFor],
  templateUrl: './radio-button-group.component.html',
  styleUrl: './radio-button-group.component.css',
  providers: [
    {
      provide: NG_VALUE_ACCESSOR,
      useExisting: forwardRef(() => RadioButtonGroupComponent),
      multi: true,
    },
  ],
})
export class RadioButtonGroupComponent {
  @Input() options: { name: string; value: string }[] = [];
  @Input() fieldsetTitle: string = 'Title';
  @Input() formControl: FormControl = new FormControl('');
  @Input() warning: string = '';
  @Input() warningColor: string = 'red';

  @Output() selectedOptionChanged = new EventEmitter<string>();

  onChange: any = () => {};
  onTouched: any = () => {};

  onValueChange(event: Event, newValue: string) {
    this.selectedOptionChanged.emit(newValue);
  }
  writeValue(value: any): void {}
  registerOnChange(fn: any): void {
    this.onChange = fn;
  }

  registerOnTouched(fn: any): void {
    this.onTouched = fn;
  }
}
