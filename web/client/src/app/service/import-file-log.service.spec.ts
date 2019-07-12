import { TestBed } from '@angular/core/testing';

import { ImportFileLogService } from './import-file-log.service';

describe('ImportFileLogService', () => {
  beforeEach(() => TestBed.configureTestingModule({}));

  it('should be created', () => {
    const service: ImportFileLogService = TestBed.get(ImportFileLogService);
    expect(service).toBeTruthy();
  });
});
