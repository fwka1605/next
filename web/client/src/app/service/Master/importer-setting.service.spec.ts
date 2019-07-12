import { TestBed } from '@angular/core/testing';

import { ImporterSettingService } from './importer-setting.service';

describe('ImporterSettingService', () => {
  beforeEach(() => TestBed.configureTestingModule({}));

  it('should be created', () => {
    const service: ImporterSettingService = TestBed.get(ImporterSettingService);
    expect(service).toBeTruthy();
  });
});
