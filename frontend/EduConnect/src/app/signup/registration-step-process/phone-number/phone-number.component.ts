import { Component, inject, OnInit } from '@angular/core';
import {
  FormControl,
  FormGroup,
  Validators,
  ReactiveFormsModule,
} from '@angular/forms';
import { SelectDropdownComponent } from '../../../common/select/select-dropdown/select-dropdown.component';
import { CommonModule } from '@angular/common';
import ApiLinks from '../../../../assets/api/link.api';
import Country from '../../../_models/reference/country/country.model';
import { HttpClient } from '@angular/common/http';
import { PhoneNumberInputComponent } from '../../../common/input/phone-number/phone-number-input/phone-number-input.component';
@Component({
  selector: 'app-phone-number',
  imports: [
    ReactiveFormsModule,
    SelectDropdownComponent,
    CommonModule,
    PhoneNumberInputComponent,
  ],
  templateUrl: './phone-number.component.html',
  styleUrl: './phone-number.component.css',
})
export class PhoneNumberComponent implements OnInit {
  http = inject(HttpClient);
  phoneNumberFormGroup = new FormGroup({
    countryCode: new FormControl('', [Validators.required]),
    phoneNumber: new FormControl('', [Validators.required]),
  });
  countries: any[] = [];
  selectLabel: string = 'Select a calling code';

  countryList: Country[] = [];
  countryCodeSelectPlaceholder: string = 'Select a country';

  phoneNumberInputLabel: string = 'Enter your phone number';
  selectedCountry: string = '';

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
    this.selectedCountry =
      this.countryList.find(
        (country) =>
          country.countryId === (event.target as HTMLSelectElement).value
      )?.nationalCallingCode ?? '';
    this.phoneNumberFormGroup.controls.countryCode.setValue(
      (event.target as HTMLSelectElement).value
    );
    console.log(this.selectedCountry);
  }

  handlePhoneNumberInput(event: Event) {
    this.phoneNumberFormGroup.controls.phoneNumber.setValue(
      `(${this.selectedCountry}) ` + (event.target as HTMLInputElement).value
    );
    console.log(this.phoneNumberFormGroup.controls.phoneNumber.value);
  }
}
