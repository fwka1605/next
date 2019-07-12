import { TestBed } from '@angular/core/testing';

import { ColumnNameSettingMasterService } from './column-name-setting-master.service';

describe('ColumnNameSettingMasterService', () => {
  beforeEach(() => TestBed.configureTestingModule({}));

  it('should be created', () => {
    const service: ColumnNameSettingMasterService = TestBed.get(ColumnNameSettingMasterService);
    expect(service).toBeTruthy();
  });
});
