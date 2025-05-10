import { Injectable } from '@angular/core';
import {HttpClient, HttpHeaders} from '@angular/common/http';
import {AccountService} from '../account.service';
export interface Course {
  courseId: number;
  title: string;

}
@Injectable({
  providedIn: 'root'
})
export class CourseLoadService {

  constructor(private http: HttpClient,
              private accountService: AccountService,) { }
  private getAuthHeaders(): HttpHeaders {
    const token = this.accountService.getAccessToken();
    return new HttpHeaders({
      'Authorization': `Bearer ${token}`,
      'Content-Type': 'application/json'
    });
  }

  getAllCourses(){
    return this.http.get<Course[]>("http://localhost:5177/UcitajPromotivniKurs",{
      headers: this.getAuthHeaders()
    });
  }
}
