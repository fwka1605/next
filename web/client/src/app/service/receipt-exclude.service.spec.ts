import { TestBed } from '@angular/core/testing';

import { ReceiptExcludeService } from './receipt-exclude.service';

describe('ReceiptExcludeService', () => {
  beforeEach(() => TestBed.configureTestingModule({}));

  it('should be created', () => {
    const service: ReceiptExcludeService = TestBed.get(ReceiptExcludeService);
    expect(service).toBeTruthy();
  });
});
