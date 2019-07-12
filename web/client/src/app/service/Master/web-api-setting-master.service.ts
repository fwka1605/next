import { Injectable } from '@angular/core';
import { HttpRequestService } from '../common/http-request.service';
import { UserInfoService } from '../common/user-info.service';
import { WebApiUrl, WebApiSaffix } from 'src/app/common/const/http.const';
import { WebApiSetting } from 'src/app/model/web-api-setting.model';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class WebApiSettingMasterService {

  constructor(
    private httpRequestService: HttpRequestService,
    private userInfoService: UserInfoService,    
  ) { }

  ///// Get //////////////////////////////////////////////////////////////////////  
  public GetByIdAsync(apiTypeId:number): Observable<any> {
    let webApiSetting = new WebApiSetting();
    webApiSetting.companyId = this.userInfoService.Company.id;
    webApiSetting.apiTypeId = apiTypeId;
    return this.httpRequestService.postReqest(
      WebApiUrl + 'WebApiSetting' + WebApiSaffix + '/GetByIdAsync', webApiSetting);
  }

  ///// Other //////////////////////////////////////////////////////////////////////
  public SaveAsync(webApiSetting:WebApiSetting): Observable<any> {
    return this.httpRequestService.postReqest(
      WebApiUrl + 'WebApiSetting' + WebApiSaffix + '/SaveAsync', webApiSetting);
  }

  public DeleteAsync(apiTypeId:number): Observable<any> {
    let webApiSetting = new WebApiSetting();
    webApiSetting.companyId = this.userInfoService.Company.id;
    webApiSetting.apiTypeId = apiTypeId;
    return this.httpRequestService.postReqest(
      WebApiUrl + 'WebApiSetting' + WebApiSaffix + '/DeleteAsync', webApiSetting);
  }

}
