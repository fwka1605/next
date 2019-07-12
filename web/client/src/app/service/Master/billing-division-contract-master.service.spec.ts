import { TestBed } from '@angular/core/testing';

import { BillingDivisionContractMasterService } from './billing-division-contract-master.service';

describe('BillingDivisionContractMasterService', () => {
  beforeEach(() => TestBed.configureTestingModule({}));

  it('should be created', () => {
    const service: BillingDivisionContractMasterService = TestBed.get(BillingDivisionContractMasterService);
    expect(service).toBeTruthy();
  });
});
