import { TestBed } from '@angular/core/testing';

import { EducationService } from './education-service.service';

describe('EducationServiceService', () => {
  let service: EducationService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(EducationService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
