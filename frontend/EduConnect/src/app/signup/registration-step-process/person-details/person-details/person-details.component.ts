import { Component, inject, OnInit } from '@angular/core';
import { TextInputComponentComponent } from '../../../../common/input/text/text-input-component/text-input-component.component';
import Country from '../../../../_models/reference/country/country.model';
import { SelectDropdownComponent } from '../../../../common/select/select-dropdown/select-dropdown.component';
import { AccountService } from '../../../../services/account.service';
import { HttpClient } from '@angular/common/http';
import ApiLinks from '../../../../../assets/api/link.api';

@Component({
  selector: 'app-person-details',
  imports: [TextInputComponentComponent, SelectDropdownComponent],
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

  //Variables for first name
  lastNameLabel: string = 'Last name';
  lastNamePlaceholder: string = 'Enter your last name...';
  lastNameWarning: string = '';

  //Variables for username
  usernameLabel: string = 'Username';
  usernamePlaceholder: string = 'Enter your username...';
  usernameWarning: string = '';

  //Variables for country of origin
  selectCountryLabel: string = 'Country of origin';
  selectCountryPlaceholder: string = 'Select your country of origin...';
  selectCountryWarning: string = '';
  countryList: Country[] = [];
  countries: any[] = [];

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
}
