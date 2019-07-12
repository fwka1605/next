import { TestBed } from '@angular/core/testing';

import { JuridicalPersonalityMasterService } from './juridical-personality-master.service';

describe('JuridicalPersonalityMasterService', () => {
  beforeEach(() => TestBed.configureTestingModule({}));

  it('should be created', () => {
    const service: JuridicalPersonalityMasterService = TestBed.get(JuridicalPersonalityMasterService);
    expect(service).toBeTruthy();
  });
});
