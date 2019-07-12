import { TestBed } from '@angular/core/testing';

import { StaffMasterService } from './staff-master.service';

describe('StaffMasterService', () => {
  beforeEach(() => TestBed.configureTestingModule({}));

  it('should be created', () => {
    const service: StaffMasterService = TestBed.get(StaffMasterService);
    expect(service).toBeTruthy();
  });
});
