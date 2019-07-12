import { Injectable } from '@angular/core';
import { HttpRequestService } from '../common/http-request.service';
import { UserInfoService } from '../common/user-info.service';
import { WebApiUrl, WebApiSaffix } from 'src/app/common/const/http.const';
import { SectionWithLoginUser } from 'src/app/model/section-with-login-user.model';
import { MasterImportData } from 'src/app/model/master-import-data.model';
import { SectionWithLoginUserSearch } from 'src/app/model/section-with-login-user-search.model';
import { MasterImportSource } from 'src/app/model/master-import-source.model';

@Injectable({
  providedIn: 'root'
})
export class SectionWithLoginUserMasterService {

  constructor(
    private httpRequestService: HttpRequestService,
    private userInfoService: UserInfoService,
  ) { }

  public GetItems(sectionWithLoginUserSearch: SectionWithLoginUserSearch = new SectionWithLoginUserSearch()) {
    sectionWithLoginUserSearch.companyId = this.userInfoService.ApplicationControl.companyId;

    return this.httpRequestService.postReqest(
      WebApiUrl + 'SectionWithLoginUser' + WebApiSaffix + '/GetItems', sectionWithLoginUserSearch);
  }

  public Save(importData: MasterImportData<SectionWithLoginUser>) {
    return this.httpRequestService.postReqest(
      WebApiUrl + 'SectionWithLoginUser' + WebApiSaffix + '/Save', importData);
  }

  public Import(source: MasterImportSource) {
    return this.httpRequestService.postReqest(
      WebApiUrl + 'SectionWithLoginUser' + WebApiSaffix + '/Import', source);
  }

  ///// Exist //////////////////////////////////////////////////////////////////////
  public ExistLoginUser(loginUserId: number) {
    return this.httpRequestService.postReqest(
      WebApiUrl + 'SectionWithLoginUser' + WebApiSaffix + '/ExistLoginUser', loginUserId);
  }

  public ExistSection(sectionId: number) {
    return this.httpRequestService.postReqest(
      WebApiUrl + 'SectionWithLoginUser' + WebApiSaffix + '/ExistSection', sectionId);
  }
}
