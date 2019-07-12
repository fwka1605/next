import { Injectable } from '@angular/core';
import { GeneralSetting } from 'src/app/model/general-setting.model';
import { GeneralSettingsResult } from 'src/app/model/general-settings-result.model';
import { ProcessResult } from 'src/app/model/process-result.model';
import { Observable } from 'rxjs';
import { HttpRequestService } from '../common/http-request.service';
import { UserInfoService } from '../common/user-info.service';
import { WebApiUrl, WebApiSaffix } from 'src/app/common/const/http.const';

@Injectable({
  providedIn: 'root'
})
export class GeneralSettingMasterService {

  constructor(
    private httpRequestService: HttpRequestService,
    private userInfoService: UserInfoService,
  ) { }

  public GetItems(): Observable<any> {
    let generalSetting = new GeneralSetting();
    generalSetting.companyId = this.userInfoService.Company.id;
    return this.httpRequestService.postReqest(
      WebApiUrl + 'GeneralSetting' + WebApiSaffix + '/GetItems', generalSetting);
  }

  public Save(setting: GeneralSetting) {
    setting.updateBy = this.userInfoService.LoginUser.id;
    
    return this.httpRequestService.postReqest(
      WebApiUrl + 'GeneralSetting' + WebApiSaffix + '/Save', setting);
  }

  public GetReport() {
    let companyId = this.userInfoService.Company.id;
    return this.httpRequestService.postReportReqest(
      WebApiUrl + 'GeneralSetting' + WebApiSaffix + '/GetReport', companyId);
  }

}
