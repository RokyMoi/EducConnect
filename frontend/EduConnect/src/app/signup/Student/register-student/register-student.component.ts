import { Component, inject } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { AccountService } from '../../../services/account.service';

@Component({
  selector: 'app-register-student',
  standalone: true,
  imports: [FormsModule],
  templateUrl: './register-student.component.html',
  styleUrl: './register-student.component.css'
})
export class RegisterStudentComponent {
  private AccountService = inject(AccountService);
  model = {
    firstname: '',
    lastname: '',
    username: '',
    email: '',
    password: '',
    phoneNumber: '',
    phoneNumberCountryCode: '',
    countryOfOrigin: ''
  };
RegistrujSe() {
  this.AccountService.register(this.model).subscribe({
    next: (response) => {
      console.log(response);
    },
    error: (err) => {
      console.log('Error during registration:', err);
    }
  });
}

}
