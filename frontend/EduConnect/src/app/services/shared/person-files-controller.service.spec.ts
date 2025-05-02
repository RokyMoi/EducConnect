import { TestBed } from '@angular/core/testing';

import { PersonFilesControllerService } from './person-files-controller.service';

describe('PersonFilesControllerService', () => {
  let service: PersonFilesControllerService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(PersonFilesControllerService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
