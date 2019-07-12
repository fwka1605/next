import { TestBed } from '@angular/core/testing';

import { ApplicationControlMasterService } from './application-control-master.service';

describe('ApplicationControlMasterService', () => {
  beforeEach(() => TestBed.configureTestingModule({}));

  it('should be created', () => {
    const service: ApplicationControlMasterService = TestBed.get(ApplicationControlMasterService);
    expect(service).toBeTruthy();
  });
});
