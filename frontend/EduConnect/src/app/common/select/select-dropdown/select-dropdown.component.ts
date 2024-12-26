import { CommonModule } from '@angular/common';
import {
  Component,
  EventEmitter,
  forwardRef,
  Input,
  OnInit,
  Output,
  SimpleChanges,
} from '@angular/core';
import {
  FormControl,
  NG_VALUE_ACCESSOR,
  ReactiveFormsModule,
} from '@angular/forms';
@Component({
  selector: 'app-select-dropdown',
  imports: [ReactiveFormsModule, CommonModule],
  templateUrl: './select-dropdown.component.html',
  styleUrl: './select-dropdown.component.css',
  providers: [
    {
      provide: NG_VALUE_ACCESSOR,
      useExisting: forwardRef(() => SelectDropdownComponent),
      multi: true,
    },
  ],
})
export class SelectDropdownComponent implements OnInit {
  @Input() label: string = 'Select an option';

  @Input() placeholder: string = 'Select an option';

  @Input() options: { name: string; value: string }[] = [
    {
      name: this.placeholder,
      value: '',
    },
  ];
  @Input() formControl: FormControl = new FormControl('');
  @Input() warning: string = '';
  @Input() isSelectingValueRequired: boolean = false;

  @Output() onChangeEvent = new EventEmitter<Event>();
  // The value that is written to the form control
  private _value: any;

  // This function is triggered when the value of the select changes
  onChange: any = () => {};
  onTouched: any = () => {};

  ngOnInit(): void {
    if (this.options.length === 0) {
      this.options = [
        {
          name: this.placeholder,
          value: '',
        },
      ];
    }
  }

  writeValue(value: any): void {
    // Update the internal value when the form control value changes
    if (value !== undefined) {
      this._value = value;
    }
  }

  registerOnChange(fn: any): void {
    this.onChange = fn;
  }

  registerOnTouched(fn: any): void {
    this.onTouched = fn;
  }

  set value(value: any) {
    if (value !== this._value) {
      this._value = value;
      this.onChange(value); // Notify Angular of the value change
    }
  }

  get value(): any {
    return this._value;
  }

  // Handle the change event when a new option is selected
  onSelectChange(event: Event) {
    const selectElement = event.target as HTMLSelectElement;
    this.value = selectElement.value;
    this.onTouched();
  }

  handleSelectChange(event: Event) {
    this.onChangeEvent.emit(event);
  }
}
