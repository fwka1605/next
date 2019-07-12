import { TestBed } from '@angular/core/testing';

import { CompanyMasterService } from './company-master.service';

describe('CompanyMasterService', () => {
  beforeEach(() => TestBed.configureTestingModule({}));

  it('should be created', () => {
    const service: CompanyMasterService = TestBed.get(CompanyMasterService);
    expect(service).toBeTruthy();
  });
});
