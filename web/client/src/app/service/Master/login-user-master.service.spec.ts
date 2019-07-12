import { TestBed } from '@angular/core/testing';

import { LoginUserMasterService } from './login-user-master.service';

describe('LoginUserMasterService', () => {
  beforeEach(() => TestBed.configureTestingModule({}));

  it('should be created', () => {
    const service: LoginUserMasterService = TestBed.get(LoginUserMasterService);
    expect(service).toBeTruthy();
  });
});
