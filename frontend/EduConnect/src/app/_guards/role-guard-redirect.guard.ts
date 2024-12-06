import { CanActivateFn } from '@angular/router';
import { inject } from '@angular/core';
import { Router } from '@angular/router';
import { AccountService } from '../services/account.service';


export const roleGuardRedirectGuard: CanActivateFn = (route, state)=> {
  const accService = inject(AccountService);
  const ruter = inject(Router);
  const CurrentUser = accService.CurrentUser();

  if(CurrentUser){
    if(CurrentUser.Role === 'student'){
      ruter.navigate(['/student-dashboard']);
    }
    else if(CurrentUser.Role === 'tutor'){
      ruter.navigate(['/tutor-dashboard']);
    }
    return false
  }
  ruter.navigate(['/']);
  return false;
};
