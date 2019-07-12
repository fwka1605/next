import { TestBed } from '@angular/core/testing';

import { FunctionAuthorityMasterService } from './function-authority-master.service';

describe('FunctionAuthorityMasterService', () => {
  beforeEach(() => TestBed.configureTestingModule({}));

  it('should be created', () => {
    const service: FunctionAuthorityMasterService = TestBed.get(FunctionAuthorityMasterService);
    expect(service).toBeTruthy();
  });
});
