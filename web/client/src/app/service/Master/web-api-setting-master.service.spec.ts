import { TestBed } from '@angular/core/testing';

import { WebApiSettingMasterService } from './web-api-setting-master.service';

describe('WebApiSettingMasterService', () => {
  beforeEach(() => TestBed.configureTestingModule({}));

  it('should be created', () => {
    const service: WebApiSettingMasterService = TestBed.get(WebApiSettingMasterService);
    expect(service).toBeTruthy();
  });
});
