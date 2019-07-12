import { TestBed } from '@angular/core/testing';

import { TaskScheduleMasterService } from './task-schedule-master.service';

describe('TaskScheduleMasterService', () => {
  beforeEach(() => TestBed.configureTestingModule({}));

  it('should be created', () => {
    const service: TaskScheduleMasterService = TestBed.get(TaskScheduleMasterService);
    expect(service).toBeTruthy();
  });
});
