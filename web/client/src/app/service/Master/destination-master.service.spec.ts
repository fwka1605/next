import { TestBed } from '@angular/core/testing';

import { DestinationMasterService } from './destination-master.service';

describe('DestinationMasterService', () => {
  beforeEach(() => TestBed.configureTestingModule({}));

  it('should be created', () => {
    const service: DestinationMasterService = TestBed.get(DestinationMasterService);
    expect(service).toBeTruthy();
  });
});
