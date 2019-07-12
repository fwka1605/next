import { TestBed } from '@angular/core/testing';

import { LoginUserLicenseMasterService } from './login-user-license-master.service';

describe('LoginUserLicenseMasterService', () => {
  beforeEach(() => TestBed.configureTestingModule({}));

  it('should be created', () => {
    const service: LoginUserLicenseMasterService = TestBed.get(LoginUserLicenseMasterService);
    expect(service).toBeTruthy();
  });
});
