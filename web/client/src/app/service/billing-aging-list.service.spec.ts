import { TestBed } from '@angular/core/testing';

import { BillingAgingListService } from './billing-aging-list.service';

describe('BillingAgingListService', () => {
  beforeEach(() => TestBed.configureTestingModule({}));

  it('should be created', () => {
    const service: BillingAgingListService = TestBed.get(BillingAgingListService);
    expect(service).toBeTruthy();
  });
});
