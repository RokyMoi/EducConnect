import {
  Component,
  ElementRef,
  EventEmitter,
  forwardRef,
  Input,
  OnInit,
  Output,
  ViewChild,
} from '@angular/core';
import {
  FormControl,
  ReactiveFormsModule,
  ControlValueAccessor,
  NG_VALUE_ACCESSOR,
  FormGroup,
} from '@angular/forms';

@Component({
  selector: 'app-two-option-picker',
  standalone: true,
  imports: [ReactiveFormsModule],
  templateUrl: './two-option-picker.component.html',
  styleUrl: './two-option-picker.component.css',
  providers: [
    {
      provide: NG_VALUE_ACCESSOR,
      useExisting: forwardRef(() => TwoOptionPickerComponent),
      multi: true,
    },
  ],
})
export class TwoOptionPickerComponent implements OnInit {
  @Input() label: string = 'Input';
  @Input() placeholder: string = 'Enter your values here...';
  @Input() inputId: string = 'input';
  @Input() warning: string = '';
  @Input() width: string = '100%';
  @Input() formControlGroup: FormGroup = new FormGroup({
    firstOption: new FormControl(false),
    secondOption: new FormControl(false),
  });

  @Input() firstOptionText: string = 'Option 1';
  @Input() secondOptionText: string = 'Option 2';
  @Input() firstOptionValue: string = 'option1';
  @Input() secondOptionValue: string = 'option2';

  @Output() input = new EventEmitter<Event>();

  // Internal callbacks for ControlValueAccessor
  private onChange: (value: any) => void = () => {};
  private onTouched: () => void = () => {};

  ngOnInit(): void {
    const firstOption = this.formControlGroup.controls['firstOption'];
    const secondOption = this.formControlGroup.controls['secondOption'];

    firstOption.valueChanges.subscribe((value) => {
      if (value) {
        secondOption.setValue(false, { emitEvent: false });
      }
    });

    secondOption.valueChanges.subscribe((value) => {
      if (value) {
        firstOption.setValue(false, { emitEvent: false });
      }
    });
  }

  onRadioChange(option: 'firstOption' | 'secondOption') {
    const firstOptionControl = this.formControlGroup.controls[
      'firstOption'
    ] as FormControl;
    const secondOptionControl = this.formControlGroup.controls[
      'secondOption'
    ] as FormControl;

    if (option === 'firstOption') {
      firstOptionControl.setValue(true);
      secondOptionControl.setValue(false);
    }
    if (option === 'secondOption') {
      firstOptionControl.setValue(false);
      secondOptionControl.setValue(true);
    }
  }

  // ControlValueAccessor Methods
  writeValue(value: any): void {
    if (value) {
      this.formControlGroup.setValue(value, { emitEvent: false });
    }
  }

  registerOnChange(fn: any): void {
    this.onChange = fn;
  }

  registerOnTouched(fn: any): void {
    this.onTouched = fn;
  }

  setDisabledState?(isDisabled: boolean): void {
    if (isDisabled) {
      this.formControlGroup.disable();
    } else {
      this.formControlGroup.enable();
    }
  }
}
