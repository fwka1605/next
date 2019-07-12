import { TestBed } from '@angular/core/testing';

import { StatusMasterService } from './status-master.service';

describe('StatusMasterService', () => {
  beforeEach(() => TestBed.configureTestingModule({}));

  it('should be created', () => {
    const service: StatusMasterService = TestBed.get(StatusMasterService);
    expect(service).toBeTruthy();
  });
});
