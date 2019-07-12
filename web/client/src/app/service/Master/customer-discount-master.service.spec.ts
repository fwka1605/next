import { TestBed } from '@angular/core/testing';

import { CustomerDiscountMasterService } from './customer-discount-master.service';

describe('CustomerDiscountMasterService', () => {
  beforeEach(() => TestBed.configureTestingModule({}));

  it('should be created', () => {
    const service: CustomerDiscountMasterService = TestBed.get(CustomerDiscountMasterService);
    expect(service).toBeTruthy();
  });
});
