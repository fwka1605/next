import { TestBed } from '@angular/core/testing';

import { SectionMasterService } from './section-master.service';

describe('SectionMasterService', () => {
  beforeEach(() => TestBed.configureTestingModule({}));

  it('should be created', () => {
    const service: SectionMasterService = TestBed.get(SectionMasterService);
    expect(service).toBeTruthy();
  });
});
