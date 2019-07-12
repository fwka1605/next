import { TestBed } from '@angular/core/testing';

import { GridSettingMasterService } from './grid-setting-master.service';

describe('GridSettingMasterService', () => {
  beforeEach(() => TestBed.configureTestingModule({}));

  it('should be created', () => {
    const service: GridSettingMasterService = TestBed.get(GridSettingMasterService);
    expect(service).toBeTruthy();
  });
});
