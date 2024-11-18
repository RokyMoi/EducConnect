import { TestBed } from '@angular/core/testing';

import { ServerHealthCheckService } from './server-health-check.service';

describe('ServerHealthCheckService', () => {
  let service: ServerHealthCheckService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(ServerHealthCheckService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
