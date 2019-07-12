import { TestBed } from '@angular/core/testing';

import { KanaHistoryPaymentAgencyMasterService } from './kana-history-payment-agency-master.service';

describe('KanaHistoryPaymentAgencyMasterService', () => {
  beforeEach(() => TestBed.configureTestingModule({}));

  it('should be created', () => {
    const service: KanaHistoryPaymentAgencyMasterService = TestBed.get(KanaHistoryPaymentAgencyMasterService);
    expect(service).toBeTruthy();
  });
});
