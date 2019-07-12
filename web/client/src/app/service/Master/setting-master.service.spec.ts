import { TestBed } from '@angular/core/testing';

import { SettingMasterService } from './setting-master.service';

describe('SettingMasterService', () => {
  beforeEach(() => TestBed.configureTestingModule({}));

  it('should be created', () => {
    const service: SettingMasterService = TestBed.get(SettingMasterService);
    expect(service).toBeTruthy();
  });
});
