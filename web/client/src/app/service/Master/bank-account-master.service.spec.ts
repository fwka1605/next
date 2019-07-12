import { TestBed } from '@angular/core/testing';

import { BankAccountMasterService } from './bank-account-master.service';

describe('BankAccountMasterService', () => {
  beforeEach(() => TestBed.configureTestingModule({}));

  it('should be created', () => {
    const service: BankAccountMasterService = TestBed.get(BankAccountMasterService);
    expect(service).toBeTruthy();
  });
});
