import { TestBed } from '@angular/core/testing';

import { EBFormatMasterService } from './ebformat-master.service';

describe('EBFormatMasterService', () => {
  beforeEach(() => TestBed.configureTestingModule({}));

  it('should be created', () => {
    const service: EBFormatMasterService = TestBed.get(EBFormatMasterService);
    expect(service).toBeTruthy();
  });
});
