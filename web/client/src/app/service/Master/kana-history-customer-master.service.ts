import { Injectable } from '@angular/core';
import { KanaHistoryCustomer } from 'src/app/model/kana-history-customer.model';
import { KanaHistoryCustomersResult } from 'src/app/model/kana-history-customers-result.model';
import { ProcessResult } from 'src/app/model/process-result.model';
import { Observable } from 'rxjs';
import { KanaHistorySearch } from 'src/app/model/kana-history-search.model';
import { UserInfoService } from '../common/user-info.service';
import { HttpRequestService } from '../common/http-request.service';
import { WebApiUrl, WebApiSaffix } from 'src/app/common/const/http.const';
import { MasterImportSource } from 'src/app/model/master-import-source.model';

@Injectable({
  providedIn: 'root'
})
export class KanaHistoryCustomerMasterService {

  constructor(
    private httpRequestService: HttpRequestService,
    private userInfoService: UserInfoService,

  ) { }

  public GetItems(option: KanaHistorySearch): Observable<any> {
    option.companyId = this.userInfoService.Company.id;

    return this.httpRequestService.postReqest(
      WebApiUrl + 'KanaHistoryCustomer' + WebApiSaffix + '/GetItems', option);
  }

  public Delete(history: KanaHistoryCustomer): Observable<any> {
    history.companyId = this.userInfoService.Company.id;
    history.updateBy = this.userInfoService.LoginUser.id;

    return this.httpRequestService.postReqest(
      WebApiUrl + 'KanaHistoryCustomer' + WebApiSaffix + '/Delete', history);
  }

  public Import(source: MasterImportSource): Observable<any> {
    return this.httpRequestService.postReqest(
      WebApiUrl + 'KanaHistoryCustomer' + WebApiSaffix + '/Import', source);
  }


  ///// Exist //////////////////////////////////////////////////////////////////////
  public ExistCustomer(history: KanaHistoryCustomer): Observable<any> {
    return this.httpRequestService.postReqest(
      WebApiUrl + 'KanaHistoryCustomer' + WebApiSaffix + '/ExistCustomer', history);
  }

}
