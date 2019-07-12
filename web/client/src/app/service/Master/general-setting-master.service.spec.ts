import { TestBed } from '@angular/core/testing';

import { GeneralSettingMasterService } from './general-setting-master.service';

describe('GeneralSettingMasterService', () => {
  beforeEach(() => TestBed.configureTestingModule({}));

  it('should be created', () => {
    const service: GeneralSettingMasterService = TestBed.get(GeneralSettingMasterService);
    expect(service).toBeTruthy();
  });
});
