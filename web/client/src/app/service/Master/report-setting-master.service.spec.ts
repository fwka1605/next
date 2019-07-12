import { TestBed } from '@angular/core/testing';

import { ReportSettingMasterService } from './report-setting-master.service';

describe('ReportSettingMasterService', () => {
  beforeEach(() => TestBed.configureTestingModule({}));

  it('should be created', () => {
    const service: ReportSettingMasterService = TestBed.get(ReportSettingMasterService);
    expect(service).toBeTruthy();
  });
});
