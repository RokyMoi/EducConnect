import {
  Component,
  EventEmitter,
  Input,
  OnChanges,
  OnInit,
  Output,
  QueryList,
  SimpleChanges,
  ViewChildren,
} from '@angular/core';
import { TimeAvailability } from '../../../../_models/person/time-availabilty/time-availability';
import { NgFor, NgIf } from '@angular/common';
import { DayOfWeek } from '../../../../../enums/day-of-week.enum';
import { FormsModule } from '@angular/forms';
import TimeHelper from '../../../../helpers/time.helper';
import { AccountService } from '../../../../services/account.service';
import { MatIconModule } from '@angular/material/icon';

@Component({
  standalone: true,
  selector: 'app-availability-records-table',
  imports: [NgFor, NgIf, FormsModule, MatIconModule],
  templateUrl: './availability-records-table.component.html',
  styleUrl: './availability-records-table.component.css',
})
export class AvailabilityRecordsTableComponent implements OnChanges {
  @Input() availabilityRecords: TimeAvailability[] = [];
  @Input() accountService!: AccountService;

  @Output() triggerRefresh = new EventEmitter<void>();
  responseMessage: string = '';
  responseMessageColor: string = 'green';
  isResponseMessageVisible: boolean = false;

  // Track sorting state
  currentSortColumn: keyof TimeAvailability | null = null;
  isAscending: boolean = true;

  editRowIndex: number | null = null; //Track which row is being edited
  editColumnName: string = '';
  currentCell: string = '';
  currentData: Partial<TimeAvailability> = {};
  editData: Partial<TimeAvailability> = {};

  isValidationSuccess: boolean | null = null;
  validationError: string = '';
  //Options for day selection
  dayOptions = Object.entries(DayOfWeek)
    .filter(([key, value]) => !isNaN(Number(value)))
    .map(([key, value]) => ({ label: key, value }));

  ngOnChanges(changes: SimpleChanges): void {
    console.log(changes['availabilityRecords']);
    if (changes['availabilityRecords']) {
      this.sortByDay();
    }
    console.log(changes);
  }
  getDayName(dayIndex: string): string {
    return `${DayOfWeek[dayIndex as keyof typeof DayOfWeek]}(${dayIndex})`;
  }
  sortByDay() {
    this.availabilityRecords.sort((a, b) => {
      const dayA = a.dayOfWeek === 0 ? 7 : a.dayOfWeek;
      const dayB = b.dayOfWeek === 0 ? 7 : b.dayOfWeek;
      return dayA - dayB;
    });
  }
  // Sort by a specific column (ascending/descending toggle)
  sortByColumn(column: keyof TimeAvailability): void {
    if (this.currentSortColumn === column) {
      this.isAscending = !this.isAscending; // Toggle direction
    } else {
      this.currentSortColumn = column;
      this.isAscending = true; // Default to ascending
    }

    this.availabilityRecords.sort((a, b) => {
      if (column === 'dayOfWeek') {
        const dayA = a.dayOfWeek === 0 ? 7 : a.dayOfWeek;
        const dayB = b.dayOfWeek === 0 ? 7 : b.dayOfWeek;
        return this.isAscending ? dayA - dayB : dayB - dayA;
      }
      const valueA = a[column];
      const valueB = b[column];

      if (valueA < valueB) return this.isAscending ? -1 : 1;
      if (valueA > valueB) return this.isAscending ? 1 : -1;
      return 0;
    });
  }

  startEdit(index: number, columnBeingEdited: string) {
    this.currentCell = columnBeingEdited + index;
    if (this.editRowIndex === index) {
      return;
    }
    this.isValidationSuccess = null;
    this.validationError = '';
    this.editRowIndex = index;
    console.log(this.currentCell);
    this.editData = { ...this.availabilityRecords[index] };
  }
  saveEdit(index: number) {
    if (this.isValidationSuccess !== false) {
      console.log('Update data:', this.editData);
      console.log('Current data:', this.availabilityRecords[index]);
      const updateRequest: TimeAvailability = {
        personAvailabilityId:
          this.availabilityRecords[index].personAvailabilityId,
        dayOfWeek: this.editData.dayOfWeek as number,
        startTime: this.editData.startTime as string,
        endTime: this.editData.endTime as string,
      };
      this.currentCell = '';
      this.editRowIndex = null;
      this.editData = {};
      this.accountService
        .updateTimeAvailability(updateRequest)
        .subscribe((response) => {
          console.log(response);
          if (response.success === 'true') {
            this.responseMessage = `Changed ${this.getDayName(
              this.availabilityRecords[index].dayOfWeek as unknown as string
            )}, ${this.availabilityRecords[index].startTime} - ${
              this.availabilityRecords[index].endTime
            } to ${this.getDayName(
              updateRequest.dayOfWeek as unknown as string
            )}, ${updateRequest.startTime} - ${updateRequest.endTime}`;
            this.responseMessageColor = 'green';
            this.triggerDataRefresh();
          }
          if (response.success === 'false') {
            this.responseMessage = response.message;
            this.responseMessageColor = 'red';
          }
          this.isResponseMessageVisible = true;
          setTimeout(() => {
            this.isResponseMessageVisible = false;
            this.responseMessage = '';
            this.responseMessageColor = 'green';
          }, 5000);
        });
    }
  }

  cancelEdit() {
    this.editRowIndex = null;
    this.currentCell = '';
    this.editData = {};
  }

  deleteRecord(recordIndex: number) {
    console.log('Deleting edit');
    console.log(recordIndex);
    console.log(this.availabilityRecords[recordIndex].personAvailabilityId);
    this.accountService
      .deleteTimeAvailability(
        this.availabilityRecords[recordIndex].personAvailabilityId
      )
      .subscribe((response) => {
        console.log(response);
        if (response.success === 'true') {
          this.responseMessage = `Deleted ${this.getDayName(
            this.availabilityRecords[recordIndex].dayOfWeek as unknown as string
          )}, ${this.availabilityRecords[recordIndex].startTime} - ${
            this.availabilityRecords[recordIndex].endTime
          }`;
          this.responseMessageColor = 'green';
          this.triggerDataRefresh();
        }
        if (response.success === 'false') {
          this.responseMessage = `We could not delete ${this.getDayName(
            this.availabilityRecords[recordIndex].dayOfWeek as unknown as string
          )}, ${this.availabilityRecords[recordIndex].startTime} - ${
            this.availabilityRecords[recordIndex].endTime
          }, because ${response.message}`;
          this.responseMessageColor = 'red';
          this.triggerDataRefresh();
        }
        this.isResponseMessageVisible = true;
        setTimeout(() => {
          this.isResponseMessageVisible = false;
          this.responseMessage = '';
          this.responseMessageColor = 'green';
        }, 5000);
      });
  }
  setDayNameValue(
    index: number,
    itemDayOfWeek: number,
    editDataDayOfWeek: number | undefined
  ): string {
    if (
      this.editRowIndex === index &&
      editDataDayOfWeek !== undefined &&
      editDataDayOfWeek !== itemDayOfWeek
    ) {
      return this.getDayName(editDataDayOfWeek.toString());
    }

    return this.getDayName(itemDayOfWeek.toString());
  }

  setStartTimeValue(
    index: number,
    itemStartTime: string,
    editDataStartTime: string | undefined
  ) {
    if (
      this.editRowIndex === index &&
      editDataStartTime !== undefined &&
      editDataStartTime !== itemStartTime
    ) {
      return editDataStartTime;
    }

    return itemStartTime.slice(0, -3);
  }
  setEndTimeValue(
    index: number,
    itemEndTime: string,
    editDataEndTime: string | undefined
  ) {
    if (
      this.editRowIndex === index &&
      editDataEndTime !== undefined &&
      editDataEndTime !== itemEndTime
    ) {
      return editDataEndTime;
    }

    return itemEndTime.slice(0, -3);
  }

  validateUpdateInput(index: number) {
    const updatedData = this.editData;
    const currentData = this.availabilityRecords[index];
    console.log(updatedData);

    this.validationError = '';
    this.isValidationSuccess = true;

    if (!updatedData.startTime || !updatedData.endTime) {
      this.isValidationSuccess = false;
      this.validationError = 'Start time and end time must be provided';
      return;
    }

    //Remove seconds from startTime and endTime if present
    let startTime = updatedData.startTime as string;
    let endTime = updatedData.endTime as string;
    if (startTime && startTime.length > 5) {
      startTime = startTime.slice(0, -3);
    }
    if (endTime && endTime.length > 5) {
      endTime = endTime.slice(0, -3);
    }

    //Validation 1. Check if the startTime value is greater or equal to the endTime value (if the startTime is after the endTime)
    if (startTime >= endTime) {
      this.isValidationSuccess = false;
      this.validationError = 'Start time must be before the end time';
      return;
    }

    //Validation 2. Check if the startTime and endTime difference in minutes is greater than 15 minutes
    const timeDifferenceInMinutes = TimeHelper.getTimeDifferenceInMinutes(
      startTime,
      endTime
    );
    console.log(timeDifferenceInMinutes);
    if (timeDifferenceInMinutes < 15) {
      this.isValidationSuccess = false;
      this.validationError = 'Time difference must be at least 15 minutes';
      return;
    }

    let isOverlapping = false;
    this.availabilityRecords.forEach((record, currentRecordIndex) => {
      const numberEditDataDayOfWeek = Number(this.editData.dayOfWeek);

      isOverlapping =
        record.dayOfWeek === numberEditDataDayOfWeek &&
        this.checkForOverlap(
          record.startTime as string,
          record.endTime as string,
          startTime,
          endTime
        ) &&
        currentRecordIndex !== index;
      return;
    });
    if (isOverlapping) {
      this.isValidationSuccess = false;
      this.validationError = 'Time slot is overlapping with another time slot';
      return;
    }
    console.log(isOverlapping);
    this.isValidationSuccess = true;
    this.validationError = '';
  }

  private checkForOverlap(
    referenceStartTime: string,
    referenceEndTime: string,
    checkStartTime: string,
    checkEndTime: string
  ) {
    //Case 1: Check if the checkStartTime is less than the referenceStartTime and the checkEndTime is more than the referenceStartTime
    if (
      checkStartTime < referenceStartTime &&
      checkEndTime > referenceStartTime
    ) {
      console.log('Case 1');
      return true;
    }

    //Case 2: Check if the checkStartTime is less than the referenceEndTime and the checkEndTime is more than the referenceEndTime
    if (checkStartTime < referenceEndTime && checkEndTime > referenceEndTime) {
      console.log('Case 2');
      return true;
    }

    //Case 3: Check if the checkStartTime is less than referenceStartTime and the checkEndTime is more than the referenceEndTime
    if (
      checkStartTime < referenceStartTime &&
      checkEndTime > referenceEndTime
    ) {
      console.log('Case 3');
      return true;
    }

    //Case 4: Check if the checkStartTime is more than the referenceStartTime and the checkEndTime is less than the referenceEndTime
    if (
      checkStartTime > referenceStartTime &&
      checkEndTime < referenceEndTime
    ) {
      console.log('Case 4');
      return true;
    }

    //Case 5: Check if the checkStartTime is equal to the referenceStartTime and the checkEndTime is equal to the referenceEndTime
    if (
      checkStartTime === referenceStartTime &&
      checkEndTime === referenceEndTime
    ) {
      console.log('Case 5');
      return true;
    }
    return false;
  }

  triggerDataRefresh() {
    this.triggerRefresh.emit();
  }
}
