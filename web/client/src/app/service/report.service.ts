import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { UserInfoService } from './common/user-info.service';
import { HttpRequestService } from './common/http-request.service';
import { ScheduledPaymentListSearch } from '../model/scheduled-payment-list-search.model';
import { WebApiUrl, WebApiSaffix } from '../common/const/http.const';
import { ArrearagesListSearch } from '../model/arrearages-list-search.model';
import { ServiceInfo } from './util/service-info';

const controller = 'Report';

@Injectable({
  providedIn: 'root'
})
export class ReportService {

  constructor(
    private userInfoService:UserInfoService,
    private httpRequestService:HttpRequestService
  ) { }

  public ArrearagesList(arrearagesListSearch:ArrearagesListSearch): Observable<any> {
    return this.httpRequestService.postReqest(
      ServiceInfo.getUrl('ArrearagesList', controller),
      arrearagesListSearch);
  }     

  public ScheduledPaymentList(scheduledPaymentListSearch:ScheduledPaymentListSearch): Observable<any> {
    return this.httpRequestService.postReqest(
      ServiceInfo.getUrl('ScheduledPaymentList', controller),
      scheduledPaymentListSearch);
  }     

  public GetScheduledPaymentReport(scheduledPaymentListSearch:ScheduledPaymentListSearch): Observable<any> {
    return this.httpRequestService.postReportReqest(
      ServiceInfo.getUrl('GetScheduledPaymentReport', controller),
      scheduledPaymentListSearch);
  }  
  
  public GetArrearagesReport(arrearagesListSearch:ArrearagesListSearch): Observable<any> {
    return this.httpRequestService.postReportReqest(
      ServiceInfo.getUrl('GetArrearagesReport', controller),
      arrearagesListSearch);
  }

  public getArrearagesShpreadsheet(option: ArrearagesListSearch): Observable<any> {
    return this.httpRequestService.postSpreadSheetReqest(ServiceInfo.getUrl('GetArrearagesSpreadsheet', controller), option);
  }
}
