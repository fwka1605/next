import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';

import { HttpRequestService } from './common/http-request.service';
import { ServiceInfo } from './util/service-info';
import { MfAggrTransactionSearch } from '../model/mf-aggr-transaction-search.model';
import { MfAggrTransaction } from '../model/mf-aggr-transaction.model';
import { UserInfoService } from './common/user-info.service';

const controller = 'MfAggrTransaction';

@Injectable({
  providedIn: 'root'
})
export class MfAggrTransactionService {

  constructor(
    private httpRequestService: HttpRequestService,
    private userInfoService: UserInfoService
  ) { }

  public get(option: MfAggrTransactionSearch): Observable<any> {
    return this.httpRequestService.postReqest(ServiceInfo.getUrl('Get', controller), option);
  }

  public getIds(): Observable<any> {
    return this.httpRequestService.postReqest(ServiceInfo.getUrl('GetIds', controller), undefined);
  }

  public save(transactions: MfAggrTransaction[]): Observable<any> {
    return this.httpRequestService.postReqest(ServiceInfo.getUrl('Save', controller), transactions);
  }

  public getLastOne(option: MfAggrTransactionSearch): Observable<any> {
    option.companyId = this.userInfoService.Company.id;
    return this.httpRequestService.postReqest(ServiceInfo.getUrl('GetLastOne', controller), option);
  }

  public delete(ids: number[]): Observable<any> {
    return this.httpRequestService.postReqest(ServiceInfo.getUrl('Delete', controller), ids);
  }
}
