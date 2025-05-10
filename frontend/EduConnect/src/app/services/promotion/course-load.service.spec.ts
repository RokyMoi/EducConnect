import { TestBed } from '@angular/core/testing';

import { CourseLoadService } from './course-load.service';

describe('CourseLoadService', () => {
  let service: CourseLoadService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(CourseLoadService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
