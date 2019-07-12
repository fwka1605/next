import { TestBed } from '@angular/core/testing';

import { ClosingSettingMasterService } from './closing-setting-master.service';

describe('ClosingSettingMasterService', () => {
  beforeEach(() => TestBed.configureTestingModule({}));

  it('should be created', () => {
    const service: ClosingSettingMasterService = TestBed.get(ClosingSettingMasterService);
    expect(service).toBeTruthy();
  });
});
