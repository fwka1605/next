import { TestBed } from '@angular/core/testing';

import { PeriodicBillingService } from './periodic-billing.service';

describe('PeriodicBillingService', () => {
  beforeEach(() => TestBed.configureTestingModule({}));

  it('should be created', () => {
    const service: PeriodicBillingService = TestBed.get(PeriodicBillingService);
    expect(service).toBeTruthy();
  });
});
