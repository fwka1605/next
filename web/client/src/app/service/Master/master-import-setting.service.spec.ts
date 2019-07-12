import { TestBed } from '@angular/core/testing';

import { MasterImportSettingService } from './master-import-setting.service';

describe('MasterImportSettingService', () => {
  beforeEach(() => TestBed.configureTestingModule({}));

  it('should be created', () => {
    const service: MasterImportSettingService = TestBed.get(MasterImportSettingService);
    expect(service).toBeTruthy();
  });
});
