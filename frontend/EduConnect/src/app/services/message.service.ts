import { inject, Injectable, signal } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { PaginationResult } from '../_models/pagination';
import { Message } from '../_models/messenger/message';
import { setPaginatedResponse, setPaginationHeaders } from '../_models/paginationHelper';
import { AccountService } from './account.service';

@Injectable({
  providedIn: 'root'
})
export class MessageService {
baseUrl='http://localhost:5177/Messenger/GetMessagesForUser';
private http = inject(HttpClient);
accService=inject(AccountService);
paginatedResult = signal<PaginationResult<Message[]> | null>(null);


getMessages(pageNumber: number,pageSize: number,container:string){
  let params = setPaginationHeaders(pageNumber, pageSize);
    let headers = new HttpHeaders();
    

    const token = this.accService.getAccessToken();
    if (token) {
      headers = headers.append('Authorization', `Bearer ${token}`);
    } else {
      // Handle case where token is not found or expired
      console.error('No access token found.');
      
    }

    params = params.append('Container', container);

    
    return this.http.get<Message[]>(this.baseUrl, { observe: 'response', params, headers }).subscribe({
       next: reponse=> setPaginatedResponse(reponse,this.paginatedResult)
    });
  }
}
