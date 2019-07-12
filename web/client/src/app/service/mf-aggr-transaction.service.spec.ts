import { TestBed } from '@angular/core/testing';

import { MfAggrTransactionService } from './mf-aggr-transaction.service';

describe('MfAggrTransactionService', () => {
  beforeEach(() => TestBed.configureTestingModule({}));

  it('should be created', () => {
    const service: MfAggrTransactionService = TestBed.get(MfAggrTransactionService);
    expect(service).toBeTruthy();
  });
});
