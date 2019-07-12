import { TestBed } from '@angular/core/testing';

import { KanaHistoryCustomerMasterService } from './kana-history-customer-master.service';

describe('KanaHistoryCustomerMasterService', () => {
  beforeEach(() => TestBed.configureTestingModule({}));

  it('should be created', () => {
    const service: KanaHistoryCustomerMasterService = TestBed.get(KanaHistoryCustomerMasterService);
    expect(service).toBeTruthy();
  });
});
