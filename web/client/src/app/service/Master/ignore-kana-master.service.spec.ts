import { TestBed } from '@angular/core/testing';

import { IgnoreKanaMasterService } from './ignore-kana-master.service';

describe('IgnoreKanaMasterService', () => {
  beforeEach(() => TestBed.configureTestingModule({}));

  it('should be created', () => {
    const service: IgnoreKanaMasterService = TestBed.get(IgnoreKanaMasterService);
    expect(service).toBeTruthy();
  });
});
