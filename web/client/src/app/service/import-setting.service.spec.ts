import { TestBed } from '@angular/core/testing';

import { ImportSettingService } from './import-setting.service';

describe('ImportSettingService', () => {
  beforeEach(() => TestBed.configureTestingModule({}));

  it('should be created', () => {
    const service: ImportSettingService = TestBed.get(ImportSettingService);
    expect(service).toBeTruthy();
  });
});
