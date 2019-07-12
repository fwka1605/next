import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { HttpRequestService } from '../common/http-request.service';
import { UserInfoService } from '../common/user-info.service';
import { EBFileSettingSearch } from 'src/app/model/eb-file-setting-search.model';
import { WebApiUrl, WebApiSaffix } from 'src/app/common/const/http.const';
import { EBFileSetting } from 'src/app/model/eb-file-setting.model';

@Injectable({
  providedIn: 'root'
})
export class EBFileSettingMasterService {

  constructor(
    private userInfoService: UserInfoService,
    private httpRequestService: HttpRequestService,
  ) { }

  public Save(ebFileSetting: EBFileSetting): Observable<any> {

    return this.httpRequestService.postReqest(WebApiUrl + 'EBFileSetting' + WebApiSaffix + '/Save', ebFileSetting);
  }


  public GetItems(): Observable<any> {

    let eBFileSettingSearch = new EBFileSettingSearch()

    eBFileSettingSearch.companyId = this.userInfoService.Company.id;
    eBFileSettingSearch.ids = new Array<number>();
    eBFileSettingSearch.loginUserId = this.userInfoService.LoginUser.id;
    eBFileSettingSearch.updateIds = new Array<number>();
    return this.httpRequestService.postReqest(WebApiUrl + 'EBFileSetting' + WebApiSaffix + '/GetItems', eBFileSettingSearch);
  }

  public UpdateIsUseable(ebFileSettingSearch: EBFileSettingSearch): Observable<any> {
    ebFileSettingSearch.companyId = this.userInfoService.Company.id;
    ebFileSettingSearch.loginUserId = this.userInfoService.LoginUser.id;
    return this.httpRequestService.postReqest(WebApiUrl + 'EBFileSetting' + WebApiSaffix + '/UpdateIsUseable', ebFileSettingSearch);
  }

  public Delete(ebFileSetting: EBFileSetting): Observable<any> {
    return this.httpRequestService.postReqest(WebApiUrl + 'EBFileSetting' + WebApiSaffix + '/Delete', ebFileSetting);
  }

}
