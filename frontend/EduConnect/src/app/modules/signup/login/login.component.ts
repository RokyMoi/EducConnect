import { HttpClient } from '@angular/common/http';
import { Component, inject, OnInit } from '@angular/core';
import { AccountService } from '../../../services/account.service';
import { FormsModule } from '@angular/forms';
import { Router } from '@angular/router';
import { User } from '../../../models/User';
import { PersonControllerService } from '../../../services/shared/person-controller.service';
import { AuthenticationGuardService } from '../../../services/shared/authentication-guard.service';
import { DefaultServerResponse } from '../../../models/shared/default-server-response';
import { SnackboxService } from '../../../services/shared/snackbox.service';

@Component({
  selector: 'app-login',
  imports: [FormsModule],
  templateUrl: './login.component.html',
  styleUrl: './login.component.css',
  standalone: true,
})
export class LoginComponent implements OnInit {
  private accountService = inject(AccountService);
  private routerNav = inject(Router);
  loggedIn = false;
  model: any = {};
  warning: string = '';
  warningClass: string = 'warning';

  constructor(
    private personControllerService: PersonControllerService,
    private authenticationGuardService: AuthenticationGuardService,
    private snackboxService: SnackboxService
  ) {}

  ngOnInit(): void {
    this.authenticationGuardService.checkIsValidToken().subscribe({
      next: (response) => {
        console.log('Response');
      },
      error: (error) => {
        console.log('Error');
      },
    });
  }
  login() {
    this.personControllerService
      .loginUser({
        usernameOrEmail: this.model.usernameOrEmail,
        password: this.model.password,
      })
      .subscribe({
        next: (response) => {
          console.log(response);
          const token = response.headers
            .get('Authorization')
            ?.replace('Bearer ', '')
            .trim();

          localStorage.setItem('Authorization', token as string);

          const role = response.body?.data.role;
          const roleString = String(role).toLowerCase();

          this.routerNav.navigate([`/${roleString}/dashboard`]);
        },
        error: (error) => {
          console.log(error);
          if (error.error.message) {
            this.warning = error.error.message;
          } else {
            this.warning = 'An error occurred. Please try again later.';
          }
        },
      });
  }
}
