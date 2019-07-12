import { TestBed } from '@angular/core/testing';

import { LocalStorageManageService } from './local-storage-manage.service';

describe('LocalStorageManageService', () => {
  beforeEach(() => TestBed.configureTestingModule({}));

  it('should be created', () => {
    const service: LocalStorageManageService = TestBed.get(LocalStorageManageService);
    expect(service).toBeTruthy();
  });
});
