import { HttpClient } from '@angular/common/http';
import { Component,  inject } from '@angular/core';
import { AccountService } from '../../services/account.service';
import { FormsModule } from '@angular/forms';

@Component({
  selector: 'app-login',
  standalone: true,
  imports: [FormsModule],
  templateUrl: './login.component.html',
  styleUrl: './login.component.css'
})
export class LoginComponent {
  private accountService =  inject(AccountService);
  loggedIn = false;
  model: any ={};

  login(){
    this.accountService.login(this.model).subscribe({
    next: response=> {
      console.log(response);
      this.loggedIn =true;
    },
    error:err=> console.log(err)
    })
  }

  

}
