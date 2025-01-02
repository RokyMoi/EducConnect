import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';
import { SelectDropdownComponent } from '../../../../common/select/select-dropdown/select-dropdown.component';
import { DatePickerComponent } from '../../../../common/input/date/date-picker/date-picker/date-picker.component';
import { DayOfWeek } from '../../../../../enums/day-of-week.enum';
import {
  AbstractControl,
  FormControl,
  FormGroup,
  ValidationErrors,
  Validators,
} from '@angular/forms';
import { TimePickerComponent } from '../../../../common/input/time/time-picker/time-picker.component';
import { SubmitButtonComponent } from '../../../../common/button/submit-button/submit-button.component';
import TimeHelper from '../../../../helpers/time.helper';
import { AccountService } from '../../../../services/account.service';
import { TimeAvailabilityHttpSaveRequest } from '../../../../_models/person/time-availabilty/time-availability-http-save-request';
import { NgIf } from '@angular/common';
@Component({
  selector: 'app-availability-day-time-select',
  standalone:true,
  imports: [
    SelectDropdownComponent,
    DatePickerComponent,
    TimePickerComponent,
    SubmitButtonComponent,
    NgIf,
  ],
  templateUrl: './availability-day-time-select.component.html',
  styleUrl: './availability-day-time-select.component.css',
})
export class AvailabilityDayTimeSelectComponent implements OnInit {
  @Input() accountService: AccountService | undefined;

  @Output() triggerRefresh = new EventEmitter<void>();
  
  dayOfWeekOptions = this.getDayOfWeekOptions();

  //Variables for the day of the week select dropdown component
  dayOfWeekLabel = 'Day of the week';
  dayOfWeekPlaceholder = 'Select a day of the week';
  dayOfWeekError: string = '';

  //Variables for the start time time picker component
  startTimeLabel = 'Available from';
  startTimePlaceholder = 'Select a start time';
  startTimeError: string = '';

  //Variables for the end time time picker component
  endTimeLabel = 'Available till';
  endTimePlaceholder = 'Select an end time';
  endTimeError: string = '';

  //Variables for the submit button component
  addButtonText = '+';

  //Variables for the response message component
  @Input() responseMessage = '';
  @Input() responseMessageColor: string = 'green';
  isResponseMessageVisible: boolean = false;

  availabilityFormGroup = new FormGroup(
    {
      dayOfWeek: new FormControl('', [Validators.required]),
      startTime: new FormControl('', [Validators.required]),
      endTime: new FormControl('', [Validators.required]),
    },
    { validators: this.startTimeBeforeEndTimeValidator.bind(this) }
  );

  ngOnInit(): void {
    this.availabilityFormGroup.controls.startTime.valueChanges.subscribe(() => {
      this.checkStartTimeForError();
    });
    this.availabilityFormGroup.controls.endTime.valueChanges.subscribe(() => {
      this.checkEndTimeForError();
    });
  }
  private getDayOfWeekOptions() {
    return Object.keys(DayOfWeek)
      .filter((key) => isNaN(Number(key)))
      .map((key) => ({
        name: key,
        value: DayOfWeek[key as keyof typeof DayOfWeek].toString(),
      }));
  }

  addNewAvailabilityInterval() {
    this.checkDayOfWeekForError();
    this.checkStartTimeForError();
    this.checkEndTimeForError();
    this.availabilityFormGroup.controls.dayOfWeek.valueChanges.subscribe(() => {
      this.checkDayOfWeekForError();
      this.checkStartTimeForError();
      this.checkEndTimeForError();
    });
    if (this.availabilityFormGroup.valid) {
      console.log(
        'Selected day of the week:',
        this.availabilityFormGroup.controls.dayOfWeek.value
      );
      const dayKey = Object.keys(DayOfWeek).filter((key) => {
        return key === this.availabilityFormGroup.controls.dayOfWeek.value;
      })[0];
      console.log('Day key:', dayKey);

      console.log(
        'Selected day of the week:',
        DayOfWeek[dayKey as keyof typeof DayOfWeek]
      );
      const createRequest: TimeAvailabilityHttpSaveRequest = {
        dayOfWeek: Number.parseInt(dayKey),
        startTime: this.availabilityFormGroup.controls.startTime
          .value as string,
        endTime: this.availabilityFormGroup.controls.endTime.value as string,
      };

      this.accountService
        ?.createTimeAvailability(createRequest)
        .subscribe((response) => {
          console.log(response);
          this.isResponseMessageVisible = true;
          if (response.success === 'true') {
            this.responseMessage = 'Availability added successfully';
            this.responseMessageColor = 'green';
            this.triggerDataRefresh();
          }

          if (response.success === 'false' && response.statusCode === 409) {
            this.responseMessage = `Your status as available already exists for the ${
              DayOfWeek[dayKey as keyof typeof DayOfWeek]
            } from ${this.availabilityFormGroup.controls.startTime.value} to ${
              this.availabilityFormGroup.controls.endTime.value
            }`;
            this.responseMessageColor = 'red';
          }
          if (response.success === 'false' && response.statusCode !== 409) {
            this.responseMessage = response.message;
            this.responseMessageColor = 'red';
          }

          setTimeout(() => {
            this.isResponseMessageVisible = false;
            this.responseMessage = '';
            this.responseMessageColor = 'green';
          }, 5000);
        });
    }
  }

  checkDayOfWeekForError() {
    if (this.availabilityFormGroup.controls.dayOfWeek.value === '') {
      this.availabilityFormGroup.controls.dayOfWeek.setErrors({
        required: true,
      });
    }
    if (this.availabilityFormGroup.controls.dayOfWeek.value != '') {
      this.availabilityFormGroup.controls.dayOfWeek.setErrors(null);
    }
    const errors = this.availabilityFormGroup.controls.dayOfWeek.errors;
    if (errors) {
      console.log(errors);
      this.dayOfWeekError = 'This field is required';
    }
    if (!errors) {
      this.dayOfWeekError = '';
    }
  }

  checkStartTimeForError() {
    const errors = this.availabilityFormGroup.controls.startTime.errors;
    if (errors?.['required']) {
      this.startTimeError = 'This field is required';
    }
    if (!errors) {
      this.startTimeError = '';
    }
  }
  checkEndTimeForError() {
    if (
      this.availabilityFormGroup.controls.startTime.value &&
      this.availabilityFormGroup.controls.endTime.value
    ) {
      if (
        TimeHelper.getTimeDifferenceInMinutes(
          this.availabilityFormGroup.controls.startTime.value,
          this.availabilityFormGroup.controls.endTime.value
        ) < 15
      ) {
        this.availabilityFormGroup.controls.endTime.setErrors({
          minLength: true,
        });
      }
    }

    if (
      this.availabilityFormGroup.controls.startTime.value &&
      this.availabilityFormGroup.controls.endTime.value &&
      this.availabilityFormGroup.controls.startTime.value >
        this.availabilityFormGroup.controls.endTime.value
    ) {
      this.availabilityFormGroup.controls.endTime.setErrors({
        startTimeAfterEndTime: true,
      });
    }

    const errors = this.availabilityFormGroup.controls.endTime.errors;

    if (errors?.['startTimeAfterEndTime']) {
      this.endTimeError = 'Start time must be before end time';
    }
    if (errors?.['required']) {
      this.endTimeError = 'This field is required';
    }
    if (errors?.['minLength']) {
      this.endTimeError =
        'End time must be at least 15 minutes after start time';
    }
    if (!errors) {
      this.endTimeError = '';
    }
  }
  private startTimeBeforeEndTimeValidator(
    control: AbstractControl
  ): ValidationErrors | null {
    const group = control as FormGroup;
    const startTime = group.controls['startTime'].value;
    const endTime = group.controls['endTime'].value;
    if (startTime && endTime && startTime > endTime) {
      return { startTimeAfterEndTime: true };
    }
    return null;
  }

  triggerDataRefresh() {
    this.triggerRefresh.emit();
  }
}
