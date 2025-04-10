import { TestBed } from '@angular/core/testing';

import { CourseAnalyticsService } from './course-analytics.service';

describe('CourseAnalyticsService', () => {
  let service: CourseAnalyticsService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(CourseAnalyticsService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
