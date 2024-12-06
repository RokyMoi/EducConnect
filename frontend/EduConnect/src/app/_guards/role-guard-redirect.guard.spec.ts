import { TestBed } from '@angular/core/testing';
import { CanActivateFn } from '@angular/router';

import { roleGuardRedirectGuard } from './role-guard-redirect.guard';

describe('roleGuardRedirectGuard', () => {
  const executeGuard: CanActivateFn = (...guardParameters) => 
      TestBed.runInInjectionContext(() => roleGuardRedirectGuard(...guardParameters));

  beforeEach(() => {
    TestBed.configureTestingModule({});
  });

  it('should be created', () => {
    expect(executeGuard).toBeTruthy();
  });
});
