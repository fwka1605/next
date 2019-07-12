import { TestBed } from '@angular/core/testing';

import { AccountTransferService } from './account-transfer.service';

describe('AccountTransferService', () => {
  beforeEach(() => TestBed.configureTestingModule({}));

  it('should be created', () => {
    const service: AccountTransferService = TestBed.get(AccountTransferService);
    expect(service).toBeTruthy();
  });
});
