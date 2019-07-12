import { TestBed } from '@angular/core/testing';

import { TaxClassMasterService } from './tax-class-master.service';

describe('TaxClassMasterService', () => {
  beforeEach(() => TestBed.configureTestingModule({}));

  it('should be created', () => {
    const service: TaxClassMasterService = TestBed.get(TaxClassMasterService);
    expect(service).toBeTruthy();
  });
});
