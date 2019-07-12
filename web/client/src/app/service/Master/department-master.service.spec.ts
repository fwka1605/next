import { TestBed } from '@angular/core/testing';

import { DepartmentMasterService } from './department-master.service';

describe('DepartmentMasterService', () => {
  beforeEach(() => TestBed.configureTestingModule({}));

  it('should be created', () => {
    const service: DepartmentMasterService = TestBed.get(DepartmentMasterService);
    expect(service).toBeTruthy();
  });
});
