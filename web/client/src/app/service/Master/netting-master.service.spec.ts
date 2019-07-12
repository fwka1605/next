import { TestBed } from '@angular/core/testing';

import { NettingMasterService } from './netting-master.service';

describe('NettingMasterService', () => {
  beforeEach(() => TestBed.configureTestingModule({}));

  it('should be created', () => {
    const service: NettingMasterService = TestBed.get(NettingMasterService);
    expect(service).toBeTruthy();
  });
});
