import { TestBed } from '@angular/core/testing';

import { SectionWithLoginUserMasterService } from './section-with-login-user-master.service';

describe('SectionWithLoginUserMasterService', () => {
  beforeEach(() => TestBed.configureTestingModule({}));

  it('should be created', () => {
    const service: SectionWithLoginUserMasterService = TestBed.get(SectionWithLoginUserMasterService);
    expect(service).toBeTruthy();
  });
});
