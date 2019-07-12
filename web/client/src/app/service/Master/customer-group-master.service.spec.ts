import { TestBed } from '@angular/core/testing';

import { CustomerGroupMasterService } from './customer-group-master.service';

describe('CustomerGroupMasterService', () => {
  beforeEach(() => TestBed.configureTestingModule({}));

  it('should be created', () => {
    const service: CustomerGroupMasterService = TestBed.get(CustomerGroupMasterService);
    expect(service).toBeTruthy();
  });
});
