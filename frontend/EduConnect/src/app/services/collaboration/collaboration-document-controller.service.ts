import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { DefaultServerResponse } from '../../models/shared/default-server-response';
import ApiLinks from '../../../assets/api/link.api';
import { CreateDocumentRequest } from '../../models/shared/collaboration-document-controller/create-document-request';
import { GetAllDocumentsByCreatedByPersonIdResponse } from '../../models/shared/collaboration-document-controller/get-all-documents-by-created-by-person-id-response';
import { GetAllDocumentsByParticipantPersonIdResponse } from '../../models/shared/collaboration-document-controller/get-all-documents-by-participant-person-id-response';
import { GetAllInvitationsForPersonIdResponse } from '../../models/shared/collaboration-document-controller/get-all-invitations-for-person-id-response';
import { GetAllInvitationsSentByPersonIdResponse } from '../../models/shared/collaboration-document-controller/get-all-invitations-sent-by-person-id-response';
import { GetCollaborationDocumentInviteInfoResponse } from '../../models/shared/collaboration-document-controller/get-collaboration-document-invite-info-response';
import { GetUsersBySearchQueryRequest } from '../../models/shared/collaboration-document-controller/get-users-by-search-query-request';
import { buildHttpParams } from '../../helpers/build-http-params.helper';
import { GetUsersBySearchQueryResponse } from '../../models/shared/collaboration-document-controller/get-users-by-search-query-response';
import { InviteUserToDocumentRequest } from '../../models/shared/collaboration-document-controller/invite-user-to-document-request';

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

  public getCollaborationDocumentInviteInfo(documentId: string) {
    const token = localStorage.getItem('Authorization');
    return this.httpClient.get<
      DefaultServerResponse<GetCollaborationDocumentInviteInfoResponse>
    >(`${this.apiUrl}/document/info/${documentId}`, {
      headers: {
        Authorization: `Bearer ${token}`,
      },
    });
  }

  public checkCollaborationDocumentOwner(documentId: string) {
    const token = localStorage.getItem('Authorization');
    return this.httpClient.get<DefaultServerResponse<boolean>>(
      `${this.apiUrl}/document/owner/${documentId}`,
      {
        headers: {
          Authorization: `Bearer ${token}`,
        },
      }
    );
  }

  public GetUsersBySearchQuery(request: GetUsersBySearchQueryRequest) {
    const token = localStorage.getItem('Authorization');
    const params = buildHttpParams(request);
    return this.httpClient.get<
      DefaultServerResponse<GetUsersBySearchQueryResponse[]>
    >(`${this.apiUrl}/user/search`, {
      headers: {
        Authorization: `Bearer ${token}`,
      },
      params: params,
    });
  }

  public inviteUserToDocument(request: InviteUserToDocumentRequest) {
    const token = localStorage.getItem('Authorization');
    const params = buildHttpParams(request);
    return this.httpClient.post<DefaultServerResponse<string>>(
      `${this.apiUrl}/invite`,
      request,
      {
        headers: {
          Authorization: `Bearer ${token}`,
        },
      }
    );
  }
}
