import { TestBed } from '@angular/core/testing';

import { MyCoursesService } from './my-courses.service';

describe('MyCoursesService', () => {
  let service: MyCoursesService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(MyCoursesService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
