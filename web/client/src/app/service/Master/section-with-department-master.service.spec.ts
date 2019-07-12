import { TestBed } from '@angular/core/testing';

import { SectionWithDepartmentMasterService } from './section-with-department-master.service';

describe('SectionWithDepartmentMasterService', () => {
  beforeEach(() => TestBed.configureTestingModule({}));

  it('should be created', () => {
    const service: SectionWithDepartmentMasterService = TestBed.get(SectionWithDepartmentMasterService);
    expect(service).toBeTruthy();
  });
});
