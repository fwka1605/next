import { TestBed } from '@angular/core/testing';

import { ReminderSettingService } from './reminder-setting.service';

describe('ReminderSettingService', () => {
  beforeEach(() => TestBed.configureTestingModule({}));

  it('should be created', () => {
    const service: ReminderSettingService = TestBed.get(ReminderSettingService);
    expect(service).toBeTruthy();
  });
});
