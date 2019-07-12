import { TestBed } from '@angular/core/testing';

import { BankAccountTypeMasterService } from './bank-account-type-master.service';

describe('BankAccountTypeMasterService', () => {
  beforeEach(() => TestBed.configureTestingModule({}));

  it('should be created', () => {
    const service: BankAccountTypeMasterService = TestBed.get(BankAccountTypeMasterService);
    expect(service).toBeTruthy();
  });
});
