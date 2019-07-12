import { TestBed } from '@angular/core/testing';

import { MfHttpRequestServiceService } from './mf-http-request-service.service';

describe('MfHttpRequestServiceService', () => {
  beforeEach(() => TestBed.configureTestingModule({}));

  it('should be created', () => {
    const service: MfHttpRequestServiceService = TestBed.get(MfHttpRequestServiceService);
    expect(service).toBeTruthy();
  });
});
