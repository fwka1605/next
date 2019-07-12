import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';

import { HttpRequestService } from '../common/http-request.service';
import { MfAggrAccount } from '../../model/mf-aggr-account.model';
import { MfAggrTag } from '../../model/mf-aggr-tag.model';
import { ServiceInfo } from '../util/service-info';

const controller = 'MfAggrMaster';

@Injectable({
  providedIn: 'root'
})
export class MfAggrMasterService {

  constructor(
    private httpRequestService: HttpRequestService,
  ) { }

  public getTags(): Observable<any> {
    return this.httpRequestService.postReqest(ServiceInfo.getUrl('GetTags', controller), undefined);
  }

  public saveTags(tags: MfAggrTag[]): Observable<any> {
    return this.httpRequestService.postReqest(ServiceInfo.getUrl('SaveTags', controller), tags);
  }

  public getAccounts(): Observable<any> {
    return this.httpRequestService.postReqest(ServiceInfo.getUrl('GetAccounts', controller), undefined);
  }

  public saveAccounts(accounts: MfAggrAccount[]): Observable<any> {
    return this.httpRequestService.postReqest(ServiceInfo.getUrl('SaveAccounts', controller), accounts);
  }
}
