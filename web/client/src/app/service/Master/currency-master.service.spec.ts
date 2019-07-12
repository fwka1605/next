import { TestBed } from '@angular/core/testing';

import { CurrencyMasterService } from './currency-master.service';

describe('CurrencyMasterService', () => {
  beforeEach(() => TestBed.configureTestingModule({}));

  it('should be created', () => {
    const service: CurrencyMasterService = TestBed.get(CurrencyMasterService);
    expect(service).toBeTruthy();
  });
});
