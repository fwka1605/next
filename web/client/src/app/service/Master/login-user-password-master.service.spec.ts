import { TestBed } from '@angular/core/testing';

import { LoginUserPasswordMasterService } from './login-user-password-master.service';

describe('LoginUserPasswordMasterService', () => {
  beforeEach(() => TestBed.configureTestingModule({}));

  it('should be created', () => {
    const service: LoginUserPasswordMasterService = TestBed.get(LoginUserPasswordMasterService);
    expect(service).toBeTruthy();
  });
});
