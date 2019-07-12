import { TestBed } from '@angular/core/testing';

import { PaymentAgencyMasterService } from './payment-agency-master.service';

describe('PaymentAgencyMasterService', () => {
  beforeEach(() => TestBed.configureTestingModule({}));

  it('should be created', () => {
    const service: PaymentAgencyMasterService = TestBed.get(PaymentAgencyMasterService);
    expect(service).toBeTruthy();
  });
});
