import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { HttpRequestService } from './common/http-request.service';
import { UserInfoService } from './common/user-info.service';
import { WebApiUrl, WebApiSaffix } from 'src/app/common/const/http.const';
import { AccountTransferLog } from '../model/account-transfer-log.model';
import { AccountTransferSearch } from 'src/app/model/account-transfer-search.model';
import { AccountTransferDetail } from 'src/app/model/account-transfer-detail.model';
import { AccountTransferImportSource } from 'src/app/model/account-transfer-import-source.model';
import { ServiceInfo } from './util/service-info';

const controller = 'AccountTransfer';

@Injectable({
  providedIn: 'root'
})
export class AccountTransferService {

  constructor(
    private httpRequestService:HttpRequestService,
    private userInfoService: UserInfoService,    
  ) { }

  /////Export/////////////////////////////////////////////////
  public Get(): Observable<any> {
    let id:number = this.userInfoService.Company.id;
    return this.httpRequestService.postReqest(
      ServiceInfo.getUrl('Get',controller), id);    
  }

  public Extract(accountTransferSearch: AccountTransferSearch): Observable<any> {
    return this.httpRequestService.postReqest(
      ServiceInfo.getUrl('Extract',controller), accountTransferSearch);    
  }

  public Save(accountTransferDetails: AccountTransferDetail[]): Observable<any> {
    return this.httpRequestService.postReqest(
      ServiceInfo.getUrl('Save',controller), accountTransferDetails);    
  }

  public Cancel(logs:AccountTransferLog[]): Observable<any> {
    return this.httpRequestService.postReqest(
      ServiceInfo.getUrl('Cancel',controller), logs);    
  }

  /////Import/////////////////////////////////////////////////
  public Read(accountTransferImportSource:AccountTransferImportSource): Observable<any> {
    return this.httpRequestService.postReqest(
      ServiceInfo.getUrl('Read','BillingAccountTransfer'), accountTransferImportSource);    
  }

  public Import(accountTransferImportSource:AccountTransferImportSource): Observable<any> {
    return this.httpRequestService.postReqest(
      ServiceInfo.getUrl('Import','BillingAccountTransfer'), accountTransferImportSource);    
  }

  ///// PDF ////////////////////////////////////////////////////////////////////////
  public GetReport(accountTransferImportSource:AccountTransferImportSource): Observable<any>{
    return this.httpRequestService.postReportReqest(
      ServiceInfo.getUrl('GetRerpot','BillingAccountTransfer'), accountTransferImportSource);
  }  

}
