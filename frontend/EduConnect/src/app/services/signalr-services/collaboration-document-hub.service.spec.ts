import { TestBed } from '@angular/core/testing';

import { CollaborationDocumentHubService } from './collaboration-document-hub.service';

describe('CollaborationDocumentHubService', () => {
  let service: CollaborationDocumentHubService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(CollaborationDocumentHubService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
