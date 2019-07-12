import { TestBed } from '@angular/core/testing';

import { PdfOutputSettingMasterService } from './pdf-output-setting-master.service';

describe('PdfOutputSettingMasterService', () => {
  beforeEach(() => TestBed.configureTestingModule({}));

  it('should be created', () => {
    const service: PdfOutputSettingMasterService = TestBed.get(PdfOutputSettingMasterService);
    expect(service).toBeTruthy();
  });
});
