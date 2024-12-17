import { Component, inject, OnInit } from '@angular/core';
import {
  FormControl,
  FormGroup,
  Validators,
  ReactiveFormsModule,
  AbstractControl,
  ValidationErrors,
} from '@angular/forms';
import { SelectDropdownComponent } from '../../../common/select/select-dropdown/select-dropdown.component';
import { CommonModule } from '@angular/common';
import ApiLinks from '../../../../assets/api/link.api';
import Country from '../../../_models/reference/country/country.model';
import { HttpClient } from '@angular/common/http';
import { PhoneNumberInputComponent } from '../../../common/input/phone-number/phone-number-input/phone-number-input.component';
import { ErrorStateMatcher } from '@angular/material/core';
import { SubmitButtonComponent } from '../../../common/button/submit-button/submit-button.component';
import { FloatingWarningBoxComponent } from '../../../common/floating-warning-box/floating-warning-box/floating-warning-box.component';
import { AccountService } from '../../../services/account.service';
import SuccessHttpResponseData from '../../../../../../../.history/frontend/EduConnect/src/app/_models/data/http.response.data/success.http.response.data_20241217014910';
import ErrorHttpResponseData from '../../../_models/data/http.response.data/error.http.response.data';
@Component({
  selector: 'app-phone-number',
  imports: [
    ReactiveFormsModule,
    SelectDropdownComponent,
    CommonModule,
    PhoneNumberInputComponent,
    SubmitButtonComponent,
    FloatingWarningBoxComponent,
  ],
  templateUrl: './phone-number.component.html',
  styleUrl: './phone-number.component.css',
})
export class PhoneNumberComponent implements OnInit {
  http = inject(HttpClient);
  accountService = inject(AccountService);
  phoneNumberFormGroup = new FormGroup({
    countryCode: new FormControl('', [Validators.required]),
    phoneNumber: new FormControl('', [
      Validators.required,
      this.phoneNumberValidator.bind(this),
    ]),
  });
  countries: any[] = [];
  selectLabel: string = 'Select a calling code';

  countryList: Country[] = [];
  countryCodeSelectPlaceholder: string = 'Select a country';
  selectCountryCodeWarning: string = '';

  phoneNumberInputLabel: string = 'Enter your phone number';
  selectedCountry: string = '';
  selectedCountryCodeAndCountryName: string = '';
  phoneNumberWarning: string = '';
  phoneNumberValue: string = '';
  warningBoxTitle: string = 'Is this your phone number?';
  warningBoxText: string = '';

  //Variable for setting the floating warning box visibility
  showWarningBox: boolean = false;

  //Yes button variable modifications
  warningBoxYesButtonText: string = 'Yes';
  warningBoxYesButtonBackgroundColor: string = 'green';

  //No button variable modifications
  warningBoxNoButtonText: string = 'No';
  warningBoxNoButtonBackgroundColor: string = 'orange';

  //Common button variable modifications
  warningBoxButtonMargin: string = '16px 0px';

  //Text to display once the data has been retrieved from the server
  saveOperationResult: string = '';

  ngOnInit(): void {
    this.getCountries();
  }
  getCountries() {
    this.http.get<Country[]>(ApiLinks.getAllCountries).subscribe({
      next: (response) => {
        this.countryList = (response as any).data.countries;
        console.log(this.countryList);
        this.countries.push({
          name: 'Select a country',
          value: 'null',
        });
        this.countries = this.countryList.map((country) => {
          return {
            name: `${country.flagEmoji} ${country.name}`,
            value: country.countryId,
          };
        });
      },
      error: (error) => {
        console.log(error);
      },
    });
  }

  handleCountryCodeSelect(event: Event) {
    const formerValueId = this.phoneNumberFormGroup.controls.countryCode.value;

    let formerValue = '';

    if (formerValueId?.length !== 0) {
      if (formerValueId?.length !== 0) {
        const foundCountry = this.countryList.find(
          (country) => country.countryId === formerValueId
        );
        formerValue = foundCountry?.nationalCallingCode || ''; // Fallback to an empty string
      }
    }

    this.selectedCountry =
      this.countryList.find(
        (country) =>
          country.countryId === (event.target as HTMLSelectElement).value
      )?.nationalCallingCode ?? '';
    this.selectedCountryCodeAndCountryName = this.countryList
      .filter(
        (country) =>
          country.countryId === (event.target as HTMLSelectElement).value
      )
      .map((country) => {
        return `${country.flagEmoji} ${country.name}`;
      })[0];
    this.phoneNumberFormGroup.controls.phoneNumber.setValue(
      this.selectedCountry +
        this.phoneNumberFormGroup.controls.phoneNumber.value?.slice(
          formerValue.length
        )
    );
    this.selectCountryCodeWarning = '';
    console.log('Former value Id: ', formerValueId);
    console.log('Former value: ', formerValue);
    console.log('Selected country: ', this.selectedCountry);
    console.log(this.selectedCountry);
    console.log(this.phoneNumberFormGroup.controls.phoneNumber.value);
  }

  handlePhoneNumberInput(event: Event) {
    if (this.phoneNumberFormGroup.controls.countryCode.value === '') {
      this.selectCountryCodeWarning = 'Please select a country first';
    }
    if (
      !this.phoneNumberFormGroup.controls.phoneNumber.value?.startsWith(
        this.selectedCountry
      )
    ) {
      (event.target as HTMLInputElement).value = this.selectedCountry;
    }
    this.phoneNumberFormGroup.controls.phoneNumber.valueChanges.subscribe(
      (value) => {
        console.log('Value change: ', value);
        const phoneNumberControl =
          this.phoneNumberFormGroup.controls.phoneNumber;
        const validationErrors =
          phoneNumberControl.errors?.['validationErrors'];

        if (validationErrors && validationErrors.length > 0) {
          this.phoneNumberWarning =
            validationErrors[validationErrors.length - 1].toString();
          console.log(
            'Validation errors: ',
            validationErrors[validationErrors.length - 1]
          );
        }
        if (!validationErrors || validationErrors.length === 0) {
          this.phoneNumberWarning = '';
        }
        if (phoneNumberControl.errors?.['required']) {
          this.phoneNumberWarning = 'Phone number is required';
        }
      }
    );
  }

  //Custom validator
  //Check if the phone number contains only numbers
  //Check if the phone number contains only numbers and - and /
  //Check if the phone number length not including country code is between 3 and 12 including 3 and 12
  private phoneNumberValidator(
    control: AbstractControl
  ): ValidationErrors | null {
    const phoneNumber = control.value;
    if (!phoneNumber) {
      return null;
    }

    const errors: string[] = [];
    if (!this.selectedCountry) {
      errors.push('Country code must be selected');
      return { validationErrors: errors };
    }

    const strippedPhoneNumber = phoneNumber
      ?.slice(this.selectedCountry.length)
      .replace(/-/g, '')
      .replace(/\//g, '');

    if (!strippedPhoneNumber) {
      errors.push('This field is required');
    }

    if ((strippedPhoneNumber as string).length < 3) {
      errors.push('Phone number must be at least 3 numbers long');
    }

    if ((strippedPhoneNumber as string).length > 12) {
      errors.push('Phone number can be only 12 numbers long');
    }

    if (!/^[\d]+$/.test(strippedPhoneNumber)) {
      errors.push('Phone number can only contain numbers');
    }

    return errors.length > 0 ? { validationErrors: errors } : null;
  }

  handleSubmitButton(event: Event) {
    console.log('Submitted value');
    if (this.phoneNumberFormGroup.valid) {
      const phoneNumberValue = this.phoneNumberFormGroup.controls.phoneNumber
        .value as string;

      console.log(
        'Phone number after slice:',
        phoneNumberValue.replace(this.selectedCountry, '')
      );
      const modifiedPhoneNumber = phoneNumberValue.replace(
        this.selectedCountry,
        ''
      );
      this.phoneNumberValue = `(${this.selectedCountry}) ${modifiedPhoneNumber}`;
      console.log('Form is valid');

      this.showWarningBox = true;
    }
    if (this.phoneNumberFormGroup.invalid) {
      console.log('Form is invalid');
      this.selectCountryCodeWarning = this.phoneNumberFormGroup.controls
        .countryCode.errors?.['required']
        ? 'Please select a country'
        : '';
      const phoneNumberControl = this.phoneNumberFormGroup.controls.phoneNumber;
      const validationErrors = phoneNumberControl.errors?.['validationErrors'];

      this.phoneNumberWarning = this.phoneNumberFormGroup.controls.phoneNumber
        .errors?.['required']
        ? 'Phone number is required'
        : '';
      if (validationErrors && validationErrors.length > 0) {
        this.phoneNumberWarning =
          validationErrors[validationErrors.length - 1].toString();
        console.log(
          'Validation errors: ',
          validationErrors[validationErrors.length - 1]
        );
      }
    }
  }

  handleYesButton(event: Event) {
    console.log(
      'Selected country id: ',
      this.phoneNumberFormGroup.controls.countryCode.value
    );
    console.log(
      'Phone number: ',
      this.phoneNumberFormGroup.controls.phoneNumber.value
    );

    const selectedCountryId = this.phoneNumberFormGroup.controls.countryCode
      .value as string;

    const enteredPhoneNumber = this.phoneNumberFormGroup.controls.phoneNumber
      .value as string;

    const phoneNumberWithNoCountryCode = enteredPhoneNumber.replace(
      this.selectedCountry,
      ''
    );
    console.log('Selected country id: ', selectedCountryId);
    console.log('Entered phone number: ', enteredPhoneNumber);
    console.log(
      'Phone number with no country code: ',
      phoneNumberWithNoCountryCode
    );
    let response: any;
    //Send the filtered data
    this.accountService
      .createPhoneNumber(selectedCountryId, phoneNumberWithNoCountryCode)
      .then((result) => {
        console.log('Result: ', result);
        response = result;
        this.saveOperationResult = result?.message as string;
      });
  }
  handleNoButton(event: Event) {
    console.log('No button clicked');
    this.toggleFloatingBox();
  }

  toggleFloatingBox() {
    this.showWarningBox = !this.showWarningBox;
    console.log('Is floating visible:', this.showWarningBox);
  }
}
