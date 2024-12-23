import { inject, Injectable, signal } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { PaginationResult } from '../_models/pagination';
import { Message } from '../_models/messenger/message';
import { setPaginatedResponse, setPaginationHeaders } from '../_models/paginationHelper';

@Injectable({
  providedIn: 'root'
})
export class MessageService {
baseUrl='http://localhost:5177/Messenger/GetMessagesForUser';
private http = inject(HttpClient);
paginatedResult = signal<PaginationResult<Message[]> | null>(null);

getMessages(pageNumber: number,pageSize: number,container:string){
let params = setPaginationHeaders(pageNumber,pageSize);
params = params.append('Container',container);
return this.http.get<Message[]>(this.baseUrl,{observe: 'response',params}).subscribe({
next: response=> {
  setPaginatedResponse(response,this.paginatedResult);
  console.log("Dulee");
}



})
}
}
