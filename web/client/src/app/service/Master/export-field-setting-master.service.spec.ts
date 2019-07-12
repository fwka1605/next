import { TestBed } from '@angular/core/testing';

import { ExportFieldSettingMasterService } from './export-field-setting-master.service';

describe('ExportFieldSettingMasterService', () => {
  beforeEach(() => TestBed.configureTestingModule({}));

  it('should be created', () => {
    const service: ExportFieldSettingMasterService = TestBed.get(ExportFieldSettingMasterService);
    expect(service).toBeTruthy();
  });
});
