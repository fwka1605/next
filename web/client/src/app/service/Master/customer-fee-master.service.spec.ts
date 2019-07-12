import { TestBed } from '@angular/core/testing';

import { CustomerFeeMasterService } from './customer-fee-master.service';

describe('CustomerFeeMasterService', () => {
  beforeEach(() => TestBed.configureTestingModule({}));

  it('should be created', () => {
    const service: CustomerFeeMasterService = TestBed.get(CustomerFeeMasterService);
    expect(service).toBeTruthy();
  });
});
