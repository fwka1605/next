import { Injectable } from '@angular/core';
import { UserInfoService } from './common/user-info.service';
import { HttpRequestService } from './common/http-request.service';
import { ReportSetting } from '../model/report-setting.model';
import { Observable } from 'rxjs';
import { WebApiUrl, WebApiSaffix } from '../common/const/http.const';
import { CustomerLedgerSearch } from '../model/customer-ledger-search.model';

@Injectable({
  providedIn: 'root'
})
export class CustomerLedgerService {

  constructor(
    private userInfoService: UserInfoService,
    private httpRequestService:HttpRequestService
  ) { }

  public Get(customerLedgerSearch:CustomerLedgerSearch): Observable<any> {
    return this.httpRequestService.postReqest(WebApiUrl + 'CustomerLedger' + WebApiSaffix + '/Get',customerLedgerSearch );
  }

  public GetReport(customerLedgerSearch:CustomerLedgerSearch): Observable<any> {
    return this.httpRequestService.postReportReqest(WebApiUrl + 'CustomerLedger' + WebApiSaffix + '/GetReport',customerLedgerSearch );
  }


    
}
