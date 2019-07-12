import { TestBed } from '@angular/core/testing';

import { EBFileSettingMasterService } from './ebfile-setting-master.service';

describe('EBFileSettingMasterService', () => {
  beforeEach(() => TestBed.configureTestingModule({}));

  it('should be created', () => {
    const service: EBFileSettingMasterService = TestBed.get(EBFileSettingMasterService);
    expect(service).toBeTruthy();
  });
});
