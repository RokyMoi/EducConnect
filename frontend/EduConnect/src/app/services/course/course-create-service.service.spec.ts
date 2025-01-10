import { TestBed } from '@angular/core/testing';

import { CourseCreateService } from './course-create-service.service';

describe('CourseCreateServiceService', () => {
  let service: CourseCreateService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(CourseCreateService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
