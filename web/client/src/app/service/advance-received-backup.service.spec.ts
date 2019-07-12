import { TestBed } from '@angular/core/testing';

import { AdvanceReceivedBackupService } from './advance-received-backup.service';

describe('AdvanceReceivedBackupService', () => {
  beforeEach(() => TestBed.configureTestingModule({}));

  it('should be created', () => {
    const service: AdvanceReceivedBackupService = TestBed.get(AdvanceReceivedBackupService);
    expect(service).toBeTruthy();
  });
});
