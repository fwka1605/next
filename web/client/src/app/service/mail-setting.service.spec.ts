import { TestBed } from '@angular/core/testing';

import { MailSettingService } from './mail-setting.service';

describe('MailSettingService', () => {
  beforeEach(() => TestBed.configureTestingModule({}));

  it('should be created', () => {
    const service: MailSettingService = TestBed.get(MailSettingService);
    expect(service).toBeTruthy();
  });
});
