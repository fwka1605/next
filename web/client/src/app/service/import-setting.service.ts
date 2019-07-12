import { Injectable } from '@angular/core';
import {  ImporterSetting } from '../model/importer-setting.model';
import { ImporterSettingsResult } from '../model/importer-settings-result.model';
import { ProcessResult } from '../model/process-result.model';
import { Observable } from 'rxjs';
import { UserInfoService } from './common/user-info.service';
import { HttpRequestService } from './common/http-request.service';
import { WebApiUrl } from '../common/const/http.const';
import { ImporterSettingDetailsResult } from '../model/importer-setting-details-result.model';
import { ImporterSettingDetail } from '../model/importer-setting-detail.model';

@Injectable({
  providedIn: 'root'
})
export class ImportSettingService {

  constructor(
  ) { }


}
