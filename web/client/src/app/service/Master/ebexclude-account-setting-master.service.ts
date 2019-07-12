import { Injectable } from '@angular/core';
import { EBExcludeAccountSetting } from 'src/app/model/eb-exclude-account-setting.model';
import { EBExcludeAccountSettingListResult  } from 'src/app/model/eb-exclude-account-setting-list-result.model';
import { ProcessResult } from 'src/app/model/process-result.model';
import { Observable } from 'rxjs';
import { UserInfoService } from '../common/user-info.service';
import { HttpRequestService } from '../common/http-request.service';
import { WebApiUrl, WebApiSaffix } from 'src/app/common/const/http.const';

@Injectable({
  providedIn: 'root'
})
export class EBExcludeAccountSettingMasterService {

  constructor(
    private httpRequestService:HttpRequestService,  
    private userInfoService:UserInfoService,  
  ) { }


  public GetItems(): Observable<any> {

    return this.httpRequestService.postReqest(WebApiUrl + 'EBExcludeAccountSetting' + WebApiSaffix + '/GetItems', this.userInfoService.Company.id);
  } 
  
  public Save(ebExcludeAccountSetting:EBExcludeAccountSetting): Observable<any> {

    return this.httpRequestService.postReqest(WebApiUrl + 'EBExcludeAccountSetting' + WebApiSaffix + '/Save', ebExcludeAccountSetting);
  } 
  
  public Delete(ebExcludeAccountSetting:EBExcludeAccountSetting): Observable<any> {

    return this.httpRequestService.postReqest(WebApiUrl + 'EBExcludeAccountSetting' + WebApiSaffix + '/Delete', ebExcludeAccountSetting);
  } 

}
