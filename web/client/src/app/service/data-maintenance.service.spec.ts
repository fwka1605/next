import { TestBed } from '@angular/core/testing';

import { DataMaintenanceService } from './data-maintenance.service';

describe('DataMaintenanceService', () => {
  beforeEach(() => TestBed.configureTestingModule({}));

  it('should be created', () => {
    const service: DataMaintenanceService = TestBed.get(DataMaintenanceService);
    expect(service).toBeTruthy();
  });
});
