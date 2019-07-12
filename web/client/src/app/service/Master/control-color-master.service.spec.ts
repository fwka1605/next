import { TestBed } from '@angular/core/testing';

import { ControlColorMasterService } from './control-color-master.service';

describe('ControlColorMasterService', () => {
  beforeEach(() => TestBed.configureTestingModule({}));

  it('should be created', () => {
    const service: ControlColorMasterService = TestBed.get(ControlColorMasterService);
    expect(service).toBeTruthy();
  });
});
