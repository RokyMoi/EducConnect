import { HttpClient } from '@angular/common/http';
import { Component, inject } from '@angular/core';
import { AccountService } from '../../services/account.service';
import { FormsModule } from '@angular/forms';
import { Router } from '@angular/router';
import { User } from '../../_models/User';

@Component({
  selector: 'app-login',
  imports: [FormsModule],
  templateUrl: './login.component.html',
  styleUrl: './login.component.css',
  standalone: true,
})
export class LoginComponent {
  private accountService = inject(AccountService);
  private routerNav = inject(Router);
  loggedIn = false;
  model: any = {};
  warning: string = '';
  warningClass: string = 'warning';

  login() {
    this.accountService.login(this.model).subscribe({
      next: (response) => {
        const currentUser = this.accountService.CurrentUser();
        if (currentUser?.Role === 'student') {
          this.routerNav.navigateByUrl('/student-dashboard');
        }
        if (currentUser?.Role === 'tutor') {
          this.routerNav.navigateByUrl('/tutor/dashboard');
        }
      },
      error: (err) => {
        console.error('Login failed:', err);
        this.warning = err.error.message;
        this.warningClass = 'warning';
      },
    });
  }
}
