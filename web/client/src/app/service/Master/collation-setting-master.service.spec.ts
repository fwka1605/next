import { TestBed } from '@angular/core/testing';

import { CollationSettingMasterService } from './collation-setting-master.service';

describe('CollationSettingMasterService', () => {
  beforeEach(() => TestBed.configureTestingModule({}));

  it('should be created', () => {
    const service: CollationSettingMasterService = TestBed.get(CollationSettingMasterService);
    expect(service).toBeTruthy();
  });
});
