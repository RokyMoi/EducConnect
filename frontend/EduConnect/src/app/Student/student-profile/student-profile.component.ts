import { Component, inject } from '@angular/core';
import { AccountService } from '../../services/account.service';
import { Router } from '@angular/router';

@Component({
  selector: 'app-student-profile',
  imports: [],
  templateUrl: './student-profile.component.html',
  styleUrl: './student-profile.component.css'
})
export class StudentProfileComponent {
  AccService = inject(AccountService);
  ruter = inject(Router);
  LogoutFromProfile(){
    this.AccService.logout();
    this.ruter.navigate(['/']);
  }

}
