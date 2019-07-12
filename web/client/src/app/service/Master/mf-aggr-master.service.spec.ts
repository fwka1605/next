import { TestBed } from '@angular/core/testing';

import { MfAggrMasterService } from './mf-aggr-master.service';

describe('MfAggrMasterService', () => {
  beforeEach(() => TestBed.configureTestingModule({}));

  it('should be created', () => {
    const service: MfAggrMasterService = TestBed.get(MfAggrMasterService);
    expect(service).toBeTruthy();
  });
});
