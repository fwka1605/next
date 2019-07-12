import { TestBed } from '@angular/core/testing';

import { ReceiptHeaderService } from './receipt-header.service';

describe('ReceiptHeaderService', () => {
  beforeEach(() => TestBed.configureTestingModule({}));

  it('should be created', () => {
    const service: ReceiptHeaderService = TestBed.get(ReceiptHeaderService);
    expect(service).toBeTruthy();
  });
});
