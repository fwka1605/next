import { TestBed } from '@angular/core/testing';

import { MenuAuthorityMasterService } from './menu-authority-master.service';

describe('MenuAuthorityMasterService', () => {
  beforeEach(() => TestBed.configureTestingModule({}));

  it('should be created', () => {
    const service: MenuAuthorityMasterService = TestBed.get(MenuAuthorityMasterService);
    expect(service).toBeTruthy();
  });
});
