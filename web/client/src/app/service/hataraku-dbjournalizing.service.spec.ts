import { TestBed } from '@angular/core/testing';

import { HatarakuDBJournalizingService } from './hataraku-dbjournalizing.service';

describe('HatarakuDBJournalizingService', () => {
  beforeEach(() => TestBed.configureTestingModule({}));

  it('should be created', () => {
    const service: HatarakuDBJournalizingService = TestBed.get(HatarakuDBJournalizingService);
    expect(service).toBeTruthy();
  });
});
