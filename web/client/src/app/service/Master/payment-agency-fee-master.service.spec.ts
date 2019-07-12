import { TestBed } from '@angular/core/testing';

import { PaymentAgencyFeeMasterService } from './payment-agency-fee-master.service';

describe('PaymentAgencyFeeMasterService', () => {
  beforeEach(() => TestBed.configureTestingModule({}));

  it('should be created', () => {
    const service: PaymentAgencyFeeMasterService = TestBed.get(PaymentAgencyFeeMasterService);
    expect(service).toBeTruthy();
  });
});
