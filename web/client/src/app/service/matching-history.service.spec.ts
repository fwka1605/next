import { TestBed } from '@angular/core/testing';

import { MatchingHistoryService } from './matching-history.service';

describe('MatchingHistoryService', () => {
  beforeEach(() => TestBed.configureTestingModule({}));

  it('should be created', () => {
    const service: MatchingHistoryService = TestBed.get(MatchingHistoryService);
    expect(service).toBeTruthy();
  });
});
