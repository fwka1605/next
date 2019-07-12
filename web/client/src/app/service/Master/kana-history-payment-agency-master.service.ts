import { Injectable } from '@angular/core';
import { HttpRequestService } from '../common/http-request.service';
import { UserInfoService } from '../common/user-info.service';
import { KanaHistorySearch } from 'src/app/model/kana-history-search.model';
import { Observable } from 'rxjs';
import { WebApiUrl, WebApiSaffix } from 'src/app/common/const/http.const';
import { KanaHistoryPaymentAgency } from 'src/app/model/kana-history-payment-agency.model';
import { MasterImportSource } from 'src/app/model/master-import-source.model';

@Injectable({
  providedIn: 'root'
})
export class KanaHistoryPaymentAgencyMasterService {

  constructor(
    private httpRequestService: HttpRequestService,
    private userInfoService: UserInfoService,
  ) { }

  public GetItems(option: KanaHistorySearch): Observable<any> {
    option.companyId = this.userInfoService.Company.id;

    return this.httpRequestService.postReqest(
      WebApiUrl + 'KanaHistoryPaymentAgency' + WebApiSaffix + '/GetItems', option);
  }

  public Delete(history: KanaHistoryPaymentAgency): Observable<any> {
    return this.httpRequestService.postReqest(
      WebApiUrl + 'KanaHistoryPaymentAgency' + WebApiSaffix + '/Delete', history);
  }

  public Import(source: MasterImportSource): Observable<any> {
    return this.httpRequestService.postReqest(
      WebApiUrl + 'KanaHistoryPaymentAgency' + WebApiSaffix + '/Import', source);
  }

}
