import { TestBed } from '@angular/core/testing';

import { PeriodicBillingSettingMasterService } from './periodic-billing-setting-master.service';

describe('PeriodicBillingSettingMasterService', () => {
  beforeEach(() => TestBed.configureTestingModule({}));

  it('should be created', () => {
    const service: PeriodicBillingSettingMasterService = TestBed.get(PeriodicBillingSettingMasterService);
    expect(service).toBeTruthy();
  });
});
