import { Injectable } from '@angular/core';
import ApiLinks from '../../../assets/api/link.api';
import { HttpClient } from '@angular/common/http';
import { DefaultServerResponse } from '../../models/shared/default-server-response';
import { GetAllFilesUploadedByPersonResponse } from '../../models/shared/person-files-controller/get-all-files-uploaded-by-person-response';

@Injectable({
  providedIn: 'root',
})
export class PersonFilesControllerService {
  apiUrl = ApiLinks.PersonFilesControllerUrl;

  constructor(private httpClient: HttpClient) {}

  public getAllFilesUploadedByPerson() {
    const token = localStorage.getItem('Authorization');
    return this.httpClient.get<
      DefaultServerResponse<GetAllFilesUploadedByPersonResponse[]>
    >(`${this.apiUrl}/all`, {
      headers: {
        Authorization: `Bearer ${token}`,
      },
    });
  }
}
