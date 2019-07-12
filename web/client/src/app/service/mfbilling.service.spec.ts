import { TestBed } from '@angular/core/testing';

import { MFBillingService } from './mfbilling.service';

describe('MFBillingService', () => {
  beforeEach(() => TestBed.configureTestingModule({}));

  it('should be created', () => {
    const service: MFBillingService = TestBed.get(MFBillingService);
    expect(service).toBeTruthy();
  });
});
