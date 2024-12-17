import {
  Component,
  ElementRef,
  EventEmitter,
  forwardRef,
  Input,
  Output,
  ViewChild,
} from '@angular/core';
import {
  FormControl,
  ReactiveFormsModule,
  ControlValueAccessor,
  NG_VALUE_ACCESSOR,
} from '@angular/forms';

@Component({
  selector: 'app-text-input',
  standalone: true,
  imports: [ReactiveFormsModule],
  templateUrl: './text-input-component.component.html',
  styleUrl: './text-input-component.component.css',
  providers: [
    {
      provide: NG_VALUE_ACCESSOR,
      useExisting: forwardRef(() => TextInputComponentComponent),
      multi: true,
    },
  ],
})
export class TextInputComponentComponent {
  @Input() label: string = 'Input';
  @Input() placeholder: string = 'Enter your values here...';
  @Input() inputId: string = 'input';
  @Input() type: string = 'text';
  @Input() warning: string = '';
  @Input() width: string = '100%';
  @Input() formControl: FormControl = new FormControl('');
  @Output() input = new EventEmitter<Event>();

  onInput(event: Event) {
    this.input.emit(event);
  }

  value: string = '';

  // Callback functions for ControlValueAccessor
  private onChange: (value: string) => void = () => {};
  private onTouched: () => void = () => {};

  // Called when value is written from the form control
  writeValue(value: string): void {
    this.value = value;
  }

  // Called when the value changes in the UI
  registerOnChange(fn: (value: string) => void): void {
    this.onChange = fn;
  }

  // Called when the control is touched in the UI
  registerOnTouched(fn: () => void): void {
    this.onTouched = fn;
  }

  // Updates the value and notifies the form control
  onInputChange(event: Event): void {
    const value = (event.target as HTMLInputElement).value;
    this.value = value;
    this.onChange(value);
    this.input.emit(event);
  }

  // Sets whether the control is disabled
  setDisabledState?(isDisabled: boolean): void {
    // Optional: Update any internal disabled state
  }
}
