import { TestBed } from '@angular/core/testing';

import { TutorStatusService } from './tutor-status.service';

describe('TutorStatusService', () => {
  let service: TutorStatusService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(TutorStatusService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
