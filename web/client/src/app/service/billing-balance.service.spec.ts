import { TestBed } from '@angular/core/testing';

import { BillingBalanceService } from './billing-balance.service';

describe('BillingBalanceService', () => {
  beforeEach(() => TestBed.configureTestingModule({}));

  it('should be created', () => {
    const service: BillingBalanceService = TestBed.get(BillingBalanceService);
    expect(service).toBeTruthy();
  });
});
