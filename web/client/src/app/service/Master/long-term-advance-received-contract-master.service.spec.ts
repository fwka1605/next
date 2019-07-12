import { TestBed } from '@angular/core/testing';

import { LongTermAdvanceReceivedContractMasterService } from './long-term-advance-received-contract-master.service';

describe('LongTermAdvanceReceivedContractMasterService', () => {
  beforeEach(() => TestBed.configureTestingModule({}));

  it('should be created', () => {
    const service: LongTermAdvanceReceivedContractMasterService = TestBed.get(LongTermAdvanceReceivedContractMasterService);
    expect(service).toBeTruthy();
  });
});
