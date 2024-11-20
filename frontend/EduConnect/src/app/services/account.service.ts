import { HttpClient } from '@angular/common/http';
import { inject, Injectable } from '@angular/core';

@Injectable({
  providedIn: 'root'
})
export class AccountService {
http = inject(HttpClient);
baseUrl="http://localhost:5177/api/"

Login(model: any){
  return this.http.post(this.baseUrl+"student-register",model);
}
}
