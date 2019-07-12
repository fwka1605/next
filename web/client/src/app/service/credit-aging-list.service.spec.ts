import { TestBed } from '@angular/core/testing';

import { CreditAgingListService } from './credit-aging-list.service';

describe('CreditAgingListService', () => {
  beforeEach(() => TestBed.configureTestingModule({}));

  it('should be created', () => {
    const service: CreditAgingListService = TestBed.get(CreditAgingListService);
    expect(service).toBeTruthy();
  });
});
