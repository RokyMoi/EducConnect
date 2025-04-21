import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { DefaultServerResponse } from '../../models/shared/default-server-response';
import ApiLinks from '../../../assets/api/link.api';
import { CreateDocumentRequest } from '../../models/shared/collaboration-document-controller/create-document-request';
import { GetAllDocumentsByCreatedByPersonIdResponse } from '../../models/shared/collaboration-document-controller/get-all-documents-by-created-by-person-id-response';
import { GetAllDocumentsByParticipantPersonIdResponse } from '../../models/shared/collaboration-document-controller/get-all-documents-by-participant-person-id-response';
import { GetAllInvitationsForPersonIdResponse } from '../../models/shared/collaboration-document-controller/get-all-invitations-for-person-id-response';
import { GetAllInvitationsSentByPersonIdResponse } from '../../models/shared/collaboration-document-controller/get-all-invitations-sent-by-person-id-response';

@Injectable({
  providedIn: 'root',
})
export class CollaborationDocumentControllerService {
  private readonly apiUrl = ApiLinks.CollaborationDocumentControllerUrl;

  constructor(private httpClient: HttpClient) {}

  public checkIsDocumentTitleTaken(title: string) {
    const token = localStorage.getItem('Authorization');
    return this.httpClient.get<DefaultServerResponse<boolean>>(
      `${this.apiUrl}/check/${title}`,
      {
        headers: {
          Authorization: `Bearer ${token}`,
        },
      }
    );
  }

  public createDocument(request: CreateDocumentRequest) {
    const token = localStorage.getItem('Authorization');
    return this.httpClient.post<DefaultServerResponse<string | null>>(
      `${this.apiUrl}/create`,
      request,
      {
        headers: {
          Authorization: `Bearer ${token}`,
        },
      }
    );
  }

  public getAllDocumentsByPersonId() {
    const token = localStorage.getItem('Authorization');
    return this.httpClient.get<
      DefaultServerResponse<GetAllDocumentsByCreatedByPersonIdResponse[]>
    >(`${this.apiUrl}/owner/all`, {
      headers: {
        Authorization: `Bearer ${token}`,
      },
    });
  }

  public getAllDocumentsByParticipantPersonId() {
    const token = localStorage.getItem('Authorization');
    return this.httpClient.get<
      DefaultServerResponse<GetAllDocumentsByParticipantPersonIdResponse[]>
    >(`${this.apiUrl}/participant/all`, {
      headers: {
        Authorization: `Bearer ${token}`,
      },
    });
  }

  public getAllInvitationsForPersonId() {
    const token = localStorage.getItem('Authorization');
    return this.httpClient.get<
      DefaultServerResponse<GetAllInvitationsForPersonIdResponse[]>
    >(`${this.apiUrl}/invitation/all`, {
      headers: {
        Authorization: `Bearer ${token}`,
      },
    });
  }

  public acceptInvitation(invitationId: string) {
    const token = localStorage.getItem('Authorization');
    return this.httpClient.patch<DefaultServerResponse<null>>(
      `${this.apiUrl}/invitation/accept/${invitationId}`,
      null,
      {
        headers: {
          Authorization: `Bearer ${token}`,
        },
      }
    );
  }

  public rejectInvitation(invitationId: string) {
    const token = localStorage.getItem('Authorization');
    return this.httpClient.patch<DefaultServerResponse<null>>(
      `${this.apiUrl}/invitation/reject/${invitationId}`,
      null,
      {
        headers: {
          Authorization: `Bearer ${token}`,
        },
      }
    );
  }

  public deleteInvitation(invitationId: string) {
    const token = localStorage.getItem('Authorization');
    return this.httpClient.delete<DefaultServerResponse<null>>(
      `${this.apiUrl}/invitation/delete/${invitationId}`,
      {
        headers: {
          Authorization: `Bearer ${token}`,
        },
      }
    );
  }

  public getAllInvitationsSentByPersonId() {
    const token = localStorage.getItem('Authorization');
    return this.httpClient.get<
      DefaultServerResponse<GetAllInvitationsSentByPersonIdResponse[]>
    >(`${this.apiUrl}/invite/all`, {
      headers: {
        Authorization: `Bearer ${token}`,
      },
    });
  }

  public deleteInviteSentByPersonByInvitationId(invitationId: string) {
    const token = localStorage.getItem('Authorization');
    return this.httpClient.delete<DefaultServerResponse<null>>(
      `${this.apiUrl}/invite/delete/${invitationId}`,
      {
        headers: {
          Authorization: `Bearer ${token}`,
        },
      }
    );
  }
}
