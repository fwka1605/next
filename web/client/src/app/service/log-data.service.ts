import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';

import { LogData } from '../model/log-data.model';
import { LogDataSearch } from '../model/log-data-search.model';

import { HttpRequestService } from './common/http-request.service';
import { UserInfoService } from './common/user-info.service';
import { ServiceInfo } from './util/service-info';
import { DateUtil } from '../common/util/date-util';

const controller = 'LogData';

@Injectable({ providedIn: 'root' })
export class LogDataService {

  constructor(
    private httpRequestService: HttpRequestService,
    private userInfoService: UserInfoService
  ) { }

  public getItems(option: LogDataSearch): Observable<any> {
    return this.httpRequestService.postReqest(ServiceInfo.getUrl('GetItems', controller), option);
  }

  public log(logData: LogData): Observable<any> {
    if (logData.companyId == undefined) {
      logData.companyId = this.userInfoService.Company.id;
      logData.clientName = this.userInfoService.TenantCode;
      logData.loginUserCode = this.userInfoService.LoginUser.code;
      logData.loginUserName = this.userInfoService.LoginUser.name;
    }
    logData.loggedAt = DateUtil.getYYYYMMDD(1);

    return this.httpRequestService.postReqest(ServiceInfo.getUrl('Log', controller), logData);
  }

  public getStats(): Observable<any> {
    return this.httpRequestService.postReqest(ServiceInfo.getUrl('GetStats', controller),
      this.userInfoService.Company.id);
  }

  public deleteAll(): Observable<any> {
    return this.httpRequestService.postReqest(ServiceInfo.getUrl('DeleteAll', controller),
      this.userInfoService.Company.id);
  }
}
