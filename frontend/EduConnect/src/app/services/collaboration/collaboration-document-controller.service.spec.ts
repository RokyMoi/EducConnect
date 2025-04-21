import { TestBed } from '@angular/core/testing';

import { CollaborationDocumentControllerService } from './collaboration-document-controller.service';

describe('CollaborationDocumentControllerService', () => {
  let service: CollaborationDocumentControllerService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(CollaborationDocumentControllerService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
