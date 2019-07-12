import { TestBed } from '@angular/core/testing';

import { AccountTitleMasterService } from './account-title-master.service';

describe('AccountTitleMasterService', () => {
  beforeEach(() => TestBed.configureTestingModule({}));

  it('should be created', () => {
    const service: AccountTitleMasterService = TestBed.get(AccountTitleMasterService);
    expect(service).toBeTruthy();
  });
});
