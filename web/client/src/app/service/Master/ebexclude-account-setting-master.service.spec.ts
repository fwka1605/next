import { TestBed } from '@angular/core/testing';

import { EBExcludeAccountSettingMasterService } from './ebexclude-account-setting-master.service';

describe('EBExcludeAccountSettingMasterService', () => {
  beforeEach(() => TestBed.configureTestingModule({}));

  it('should be created', () => {
    const service: EBExcludeAccountSettingMasterService = TestBed.get(EBExcludeAccountSettingMasterService);
    expect(service).toBeTruthy();
  });
});
