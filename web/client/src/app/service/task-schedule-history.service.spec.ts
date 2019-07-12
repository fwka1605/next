import { TestBed } from '@angular/core/testing';

import { TaskScheduleHistoryService } from './task-schedule-history.service';

describe('TaskScheduleHistoryService', () => {
  beforeEach(() => TestBed.configureTestingModule({}));

  it('should be created', () => {
    const service: TaskScheduleHistoryService = TestBed.get(TaskScheduleHistoryService);
    expect(service).toBeTruthy();
  });
});
