import { TestBed } from '@angular/core/testing';

import { HolidayCalendarMasterService } from './holiday-calendar-master.service';

describe('HolidayCalendarMasterService', () => {
  beforeEach(() => TestBed.configureTestingModule({}));

  it('should be created', () => {
    const service: HolidayCalendarMasterService = TestBed.get(HolidayCalendarMasterService);
    expect(service).toBeTruthy();
  });
});
