import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { WebApiUrl, WebApiSaffix } from 'src/app/common/const/http.const';
import { UserInfoService } from '../common/user-info.service';
import { HttpRequestService } from '../common/http-request.service';
import { ReportSetting } from 'src/app/model/report-setting.model';

@Injectable({
  providedIn: 'root'
})
export class ReportSettingMasterService {

  constructor(
    private userInfoService: UserInfoService,
    private httpRequestService:HttpRequestService
  ) { }

  public GetItems(reportSetting:ReportSetting): Observable<any> {
    return this.httpRequestService.postReqest(WebApiUrl + 'ReportSetting' + WebApiSaffix + '/GetItems',reportSetting );
  }

  public Save(reportSettings:Array<ReportSetting>): Observable<any> {
    return this.httpRequestService.postReqest(WebApiUrl + 'ReportSetting' + WebApiSaffix + '/Save',reportSettings );
  }  

}
