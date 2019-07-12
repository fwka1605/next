import { TestBed } from '@angular/core/testing';

import { BillingDivisionSettingMasterService } from './billing-division-setting-master.service';

describe('BillingDivisionSettingMasterService', () => {
  beforeEach(() => TestBed.configureTestingModule({}));

  it('should be created', () => {
    const service: BillingDivisionSettingMasterService = TestBed.get(BillingDivisionSettingMasterService);
    expect(service).toBeTruthy();
  });
});
