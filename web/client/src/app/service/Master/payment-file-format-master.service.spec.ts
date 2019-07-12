import { TestBed } from '@angular/core/testing';

import { PaymentFileFormatMasterService } from './payment-file-format-master.service';

describe('PaymentFileFormatMasterService', () => {
  beforeEach(() => TestBed.configureTestingModule({}));

  it('should be created', () => {
    const service: PaymentFileFormatMasterService = TestBed.get(PaymentFileFormatMasterService);
    expect(service).toBeTruthy();
  });
});
