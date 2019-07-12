import { AccountTitle } from '../../model/account-title.model';
import { Injectable } from '@angular/core';
import { AccountTitlesResult } from 'src/app/model/account-titles-result.model';
import { AccountTitleResult } from 'src/app/model/account-title-result.model';
import { ProcessResult } from 'src/app/model/process-result.model';
import { AccountTitleSearch } from 'src/app/model/account-title-search.model';
import { HttpRequestService } from '../common/http-request.service';
import { WebApiUrl, WebApiSaffix } from 'src/app/common/const/http.const';
import { Observable } from 'rxjs';
import { UserInfoService } from '../common/user-info.service';
import { StringUtil } from 'src/app/common/util/string-util';

@Injectable({
  providedIn: 'root'
})
export class AccountTitleMasterService {

  constructor(
    private httpRequestService: HttpRequestService,
    private userInfoService: UserInfoService,
  ) { }

  public Get(accountTitleCode: string = ""): Observable<any> {
    let accountTitleSearch = new AccountTitleSearch();

    accountTitleSearch.companyId = this.userInfoService.Company.id;

    if (!StringUtil.IsNullOrEmpty(accountTitleCode)) {
      accountTitleSearch.codes = new Array<string>();
      accountTitleSearch.codes.push(accountTitleCode);
    }

    return this.httpRequestService.postReqest(WebApiUrl + 'AccountTitle' + WebApiSaffix + '/Get', accountTitleSearch);
  }

  public Save(accountTitle: AccountTitle): Observable<any> {
    accountTitle.companyId = this.userInfoService.Company.id;
    accountTitle.updateBy = this.userInfoService.LoginUser.id;

    return this.httpRequestService.postReqest(WebApiUrl + 'AccountTitle' + WebApiSaffix + '/Save', accountTitle);
  }

  public Delete(accountTitle: AccountTitle): Observable<any> {
    return this.httpRequestService.postReqest(
      WebApiUrl + 'AccountTitle' + WebApiSaffix + '/Delete', accountTitle);
  }

  public Import(accountTitle: AccountTitle): Observable<any> {
    return this.httpRequestService.postReqest(
      WebApiUrl + 'AccountTitle' + WebApiSaffix + '/Import', accountTitle);
  }


}
