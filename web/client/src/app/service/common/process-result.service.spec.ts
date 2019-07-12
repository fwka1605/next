import { TestBed } from '@angular/core/testing';

import { ProcessResultService } from './process-result.service';

describe('ProcessResultService', () => {
  beforeEach(() => TestBed.configureTestingModule({}));

  it('should be created', () => {
    const service: ProcessResultService = TestBed.get(ProcessResultService);
    expect(service).toBeTruthy();
  });
});
