import { TestBed } from '@angular/core/testing';

import { BankBranchMasterService } from './bank-branch-master.service';

describe('BankBranchMasterService', () => {
  beforeEach(() => TestBed.configureTestingModule({}));

  it('should be created', () => {
    const service: BankBranchMasterService = TestBed.get(BankBranchMasterService);
    expect(service).toBeTruthy();
  });
});
