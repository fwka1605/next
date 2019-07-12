import { TestBed } from '@angular/core/testing';

import { InputControlMasterService } from './input-control-master.service';

describe('InputControlMasterService', () => {
  beforeEach(() => TestBed.configureTestingModule({}));

  it('should be created', () => {
    const service: InputControlMasterService = TestBed.get(InputControlMasterService);
    expect(service).toBeTruthy();
  });
});
