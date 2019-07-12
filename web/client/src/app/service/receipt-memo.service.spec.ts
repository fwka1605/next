import { TestBed } from '@angular/core/testing';

import { ReceiptMemoService } from './receipt-memo.service';

describe('ReceiptMemoService', () => {
  beforeEach(() => TestBed.configureTestingModule({}));

  it('should be created', () => {
    const service: ReceiptMemoService = TestBed.get(ReceiptMemoService);
    expect(service).toBeTruthy();
  });
});
