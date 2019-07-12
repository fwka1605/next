import { Injectable } from '@angular/core';
import {   LogData } from '../model/log-data.model';
import {  LogDatasResult } from '../model/log-datas-result.model';
import { ProcessResult } from '../model/process-result.model';
import { HttpRequestService } from './common/http-request.service';
import { UserInfoService } from './common/user-info.service';
import { Observable } from 'rxjs';
import { WebApiUrl, WebApiSaffix } from '../common/const/http.const';
import { NgbCalendar } from '@ng-bootstrap/ng-bootstrap';
import { DateUtil } from '../common/util/date-util';

@Injectable({
  providedIn: 'root'
})
export class LogsService {


  constructor(
    private httpRequestService: HttpRequestService,
    private userInfoService: UserInfoService,
    private calendar:NgbCalendar,
  ) { }
    
  public Log(munuId:number,operationName:string): Observable<any> {

    let today = this.calendar.getToday();

    let logData = new LogData();

    logData.companyId=this.userInfoService.Company.id;
    logData.clientName="";
    logData.loggedAt= today.year + "/" + today.month + "/" + today.day + "  00:00:00";
    
    logData.loginUserCode=this.userInfoService.LoginUser.code;
    logData.loginUserName=this.userInfoService.LoginUser.name;
    logData.menuId=munuId;
    logData.operationName=operationName;
    logData.logCount=0;
    logData.firstLoggedAt=today.year + "/" + today.month + "/" + today.day + " 00:00:00";

    return this.httpRequestService.postReqest(WebApiUrl + 'LogData' + WebApiSaffix + '/Log', logData);
  }

}
