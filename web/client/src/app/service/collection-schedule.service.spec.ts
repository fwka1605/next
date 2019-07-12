import { TestBed } from '@angular/core/testing';

import { CollectionScheduleService } from './collection-schedule.service';

describe('CollectionScheduleService', () => {
  beforeEach(() => TestBed.configureTestingModule({}));

  it('should be created', () => {
    const service: CollectionScheduleService = TestBed.get(CollectionScheduleService);
    expect(service).toBeTruthy();
  });
});
