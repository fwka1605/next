import { TestBed } from '@angular/core/testing';

import { BillingInvoiceService } from './billing-invoice.service';

describe('BillingInvoiceService', () => {
  beforeEach(() => TestBed.configureTestingModule({}));

  it('should be created', () => {
    const service: BillingInvoiceService = TestBed.get(BillingInvoiceService);
    expect(service).toBeTruthy();
  });
});
