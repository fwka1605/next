import { TestBed } from '@angular/core/testing';

import { CustomerPaymentContractMasterService } from './customer-payment-contract-master.service';

describe('CustomerPaymentContractMasterService', () => {
  beforeEach(() => TestBed.configureTestingModule({}));

  it('should be created', () => {
    const service: CustomerPaymentContractMasterService = TestBed.get(CustomerPaymentContractMasterService);
    expect(service).toBeTruthy();
  });
});
