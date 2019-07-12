import { Injectable } from '@angular/core';
import { LoginUser } from 'src/app/model/login-user.model';
import { UsersResult } from 'src/app/model/users-result.model';
import { ProcessResult } from 'src/app/model/process-result.model';
import { HttpRequestService } from '../common/http-request.service';
import { UserInfoService } from '../common/user-info.service';
import { Observable } from 'rxjs';
import { WebApiUrl, WebApiSaffix } from 'src/app/common/const/http.const';
import { LoginUserSearch } from 'src/app/model/login-user-search.model';
import { StringUtil } from 'src/app/common/util/string-util';
import { MasterImportSource } from 'src/app/model/master-import-source.model';

@Injectable({
  providedIn: 'root'
})
export class LoginUserMasterService {

  constructor(
    private httpRequestService: HttpRequestService,
    private userInfoService: UserInfoService,
  ) { }

  public GetItems(userCode: string = ""): Observable<any> {

    let loginUserSearch = new LoginUserSearch();
    loginUserSearch.companyId = this.userInfoService.Company.id;

    if (!StringUtil.IsNullOrEmpty(userCode)) {
      loginUserSearch.codes = new Array<string>();
      loginUserSearch.codes.push(userCode);
    }
    return this.httpRequestService.postReqest(WebApiUrl + 'LoginUser' + WebApiSaffix + '/GetItems', loginUserSearch);
  }

  public Save(loginUser: LoginUser) {
    loginUser.companyId = this.userInfoService.Company.id;
    loginUser.updateBy = this.userInfoService.LoginUser.id;

    return this.httpRequestService.postReqest(
      WebApiUrl + 'LoginUser' + WebApiSaffix + '/Save', loginUser);
  }

  public Delete(Id: number) {
    return this.httpRequestService.postReqest(
      WebApiUrl + 'LoginUser' + WebApiSaffix + '/Delete', Id);
  }

  public Import(source: MasterImportSource) {
    return this.httpRequestService.postReqest(
      WebApiUrl + 'LoginUser' + WebApiSaffix + '/Import', source);
  }

  public GetReport() {
    return this.httpRequestService.postReportReqest(
      WebApiUrl + 'LoginUser' + WebApiSaffix + '/GetReport', this.userInfoService.Company.id);
  }


  ///// Exist //////////////////////////////////////////////////////////////////////
  public ExitStaff(staffId: number) {
    return this.httpRequestService.postReqest(
      WebApiUrl + 'LoginUser' + WebApiSaffix + '/ExitStaff', staffId);
  }


}
