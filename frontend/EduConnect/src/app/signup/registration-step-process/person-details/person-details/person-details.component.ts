import { Component, inject, OnInit } from '@angular/core';
import { TextInputComponentComponent } from '../../../../common/input/text/text-input-component/text-input-component.component';
import Country from '../../../../_models/reference/country/country.model';
import { SelectDropdownComponent } from '../../../../common/select/select-dropdown/select-dropdown.component';
import { AccountService } from '../../../../services/account.service';
import { HttpClient } from '@angular/common/http';
import ApiLinks from '../../../../../assets/api/link.api';
import { SubmitButtonComponent } from '../../../../common/button/submit-button/submit-button.component';
import {
  FormControl,
  FormGroup,
  ReactiveFormsModule,
  Validators,
} from '@angular/forms';
import { FloatingWarningBoxComponent } from '../../../../common/floating-warning-box/floating-warning-box/floating-warning-box.component';
import { NgIf, NgStyle } from '@angular/common';
import {
  MatProgressSpinner,
  ProgressSpinnerMode,
} from '@angular/material/progress-spinner';

@Component({
  selector: 'app-person-details',
  imports: [
    TextInputComponentComponent,
    SelectDropdownComponent,
    SubmitButtonComponent,
    FloatingWarningBoxComponent,
    ReactiveFormsModule,
    NgIf,
    MatProgressSpinner,
    NgStyle,
  ],
  templateUrl: './person-details.component.html',
  styleUrl: './person-details.component.css',
})
export class PersonDetailsComponent implements OnInit {
  //Inject the http client class
  http = inject(HttpClient);

  //Inject classes for the AccountService class which is used to access the API endpoints regarding the account
  accountService = inject(AccountService);

  //Variables for setting the app-text-input component's modular properties
  //Variables for first name
  firstNameLabel: string = 'First name';
  firstNamePlaceholder: string = 'Enter your first name...';
  firstNameWarning: string = '';
  showFirstName: boolean = false;

  //Variables for first name
  lastNameLabel: string = 'Last name';
  lastNamePlaceholder: string = 'Enter your last name...';
  lastNameWarning: string = '';
  showLastName: boolean = false;

  //Variables for username
  usernameLabel: string = 'Username';
  usernamePlaceholder: string = 'Enter your username...';
  usernameWarning: string = '';

  //Variables for country of origin
  selectCountryLabel: string = 'Country of origin';
  selectCountryPlaceholder: string = 'Select your country of origin...';
  selectCountryWarning: string = '';
  selectedCountryOfOrigin: Country | undefined = undefined;
  showCountryOfOrigin: boolean = false;
  countryList: Country[] = [];
  countries: any[] = [];

  //Form control
  personDetailsFormGroup = new FormGroup({
    firstName: new FormControl(''),
    lastName: new FormControl(''),
    username: new FormControl('', [
      Validators.required,
      Validators.pattern(/^(?=.*[a-zA-Z])[a-zA-Z0-9_.]+$/),
      Validators.minLength(2),
      Validators.maxLength(20),
    ]),
    countryOfOrigin: new FormControl(''),
  });

  //Warning box variables
  warningBoxTitle: string = 'Are these your personal details?';
  warningBoxText: string = '';
  showWarningBox: boolean = false;

  //Text to display once the data has been retrieved from the server
  saveOperationResult: string = '';
  saveOperationResultTextColor: string = 'yellow';
  isDataTransmissionActive: boolean = false;
  spinnerMode: ProgressSpinnerMode = 'indeterminate';
  spinnerColor: string = 'orange';
  isDataTransmissionComplete: boolean = false;

  //Yes button variable modifications
  warningBoxYesButtonText: string = 'Yes';
  warningBoxYesButtonBackgroundColor: string = 'green';

  //No button variable modifications
  warningBoxNoButtonText: string = 'No';
  warningBoxNoButtonBackgroundColor: string = 'orange';

  //Continue button variable modifications
  continueButtonText: string = 'Continue';

  //Edit button variable modifications
  editButtonText: string = 'Edit';

  //Common button variable modifications
  warningBoxButtonMargin: string = '16px 0px';

  ngOnInit(): void {
    this.getCountries();
  }
  getCountries() {
    this.http
      .get<Country[]>(ApiLinks.getAllCountries, {
        headers: {
          Authorization: `Bearer ${this.accountService.getAccessToken()}`,
        },
      })
      .subscribe({
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

  handleCountryOfOriginSelect(event: Event) {
    console.log(this.personDetailsFormGroup.controls.countryOfOrigin.value);
    this.selectedCountryOfOrigin = this.countryList.find(
      (country) =>
        country.countryId === (event.target as HTMLSelectElement).value
    );
    console.log(this.selectedCountryOfOrigin);
  }

  handleUsernameInput(event: Event) {
    console.log(this.personDetailsFormGroup.controls.username.value);
    console.log(this.personDetailsFormGroup.controls.username.errors);
    const error = this.personDetailsFormGroup.controls.username.errors;

    if (error) {
      if (error['required']) {
        this.usernameWarning = 'Username is required';
      } else if (error['pattern']) {
        this.usernameWarning = 'Username can only contain letters and numbers';
      } else if (error['minlength']) {
        this.usernameWarning = 'Username must be at least 2 characters long';
      } else if (error['maxlength']) {
        this.usernameWarning = 'Username must be less than 20 characters long';
      } else {
        this.usernameWarning = '';
      }
    } else {
      this.usernameWarning = '';
    }
  }

  usernameValidation() {
    const error = this.personDetailsFormGroup.controls.username.errors;

    if (error) {
      if (error['required']) {
        this.usernameWarning = 'Username is required';
      } else if (error['pattern']) {
        this.usernameWarning = 'Username can only contain letters and numbers';
      } else if (error['minlength']) {
        this.usernameWarning = 'Username must be at least 2 characters long';
      } else if (error['maxlength']) {
        this.usernameWarning = 'Username must be less than 20 characters long';
      } else {
        this.usernameWarning = '';
      }
    } else {
      this.usernameWarning = '';
    }
  }
  handleSubmitButton() {
    if (this.personDetailsFormGroup.controls.username.invalid) {
      this.usernameValidation();
      return;
    }

    this.showFirstName =
      this.personDetailsFormGroup.controls.firstName.value !== '';
    this.showLastName =
      this.personDetailsFormGroup.controls.lastName.value !== '';
    this.showCountryOfOrigin =
      this.personDetailsFormGroup.controls.countryOfOrigin.value !== '';
    this.showCountryOfOrigin = this.selectedCountryOfOrigin !== undefined;

    console.log('Show first name: ', this.showFirstName);
    console.log('Show last name: ', this.showLastName);
    console.log('Show country of origin: ', this.showCountryOfOrigin);

    this.toggleFloatingBox();
    console.log(this.personDetailsFormGroup.value);
  }

  handleYesButton(event: Event) {
    this.isDataTransmissionActive = true;
    this.saveOperationResult = 'Saving personal details...';
    this.saveOperationResultTextColor = 'blue';
    let response: any;
    const requestBody = {
      firstName: this.personDetailsFormGroup.controls.firstName.value as string,
      lastName: this.personDetailsFormGroup.controls.lastName.value as string,
      username: this.personDetailsFormGroup.controls.username.value as string,
      countryOfOrigin: this.personDetailsFormGroup.controls.countryOfOrigin
        .value as string,
    };

    //Send the filtered data
    this.accountService
      .createPersonDetails(
        requestBody.firstName,
        requestBody.lastName,
        requestBody.username,
        requestBody.countryOfOrigin
      )
      .then((result) => {
        this.isDataTransmissionActive = false;
        this.isDataTransmissionComplete = true;
        console.log('Result: ', result);
        response = result;
        if ((result?.success as string) === 'true') {
          this.saveOperationResultTextColor = 'green';
        }
        if ((result?.success as string) === 'false') {
          this.saveOperationResultTextColor = 'red';
        }
        this.saveOperationResult = result?.message as string;
      });
  }
  handleNoButton(event: Event) {
    console.log('No button clicked');
    this.toggleFloatingBox();
  }

  handleContinueButton(event: Event) {
    console.log('Continue button clicked');
    this.accountService.router.navigateByUrl('/signup/education');
  }

  handleEditButton(event: Event) {
    console.log('Edit button clicked');
    this.saveOperationResult = '';
    this.saveOperationResultTextColor = 'yellow';
    this.isDataTransmissionActive = false;
    this.isDataTransmissionComplete = false;
    this.toggleFloatingBox();
  }
  toggleFloatingBox() {
    console.log('Toggle floating box');
    this.showWarningBox = !this.showWarningBox;
  }
}
