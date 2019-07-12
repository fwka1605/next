import { TestBed } from '@angular/core/testing';

import { PasswordPolicyMasterService } from './password-policy-master.service';

describe('PasswordPolicyMasterService', () => {
  beforeEach(() => TestBed.configureTestingModule({}));

  it('should be created', () => {
    const service: PasswordPolicyMasterService = TestBed.get(PasswordPolicyMasterService);
    expect(service).toBeTruthy();
  });
});
