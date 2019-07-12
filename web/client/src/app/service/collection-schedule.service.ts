import { Injectable } from '@angular/core';
import { UserInfoService } from './common/user-info.service';
import { HttpRequestService } from './common/http-request.service';
import { Observable } from 'rxjs';
import { CollectionScheduleSearch } from '../model/collection-schedule-search.model';
import { WebApiUrl, WebApiSaffix } from '../common/const/http.const';

@Injectable({
  providedIn: 'root'
})
export class CollectionScheduleService {

  constructor(
    private userInfoService: UserInfoService,
    private httpRequestService: HttpRequestService
  ) { }

  public Get(collectionScheduleSearch:CollectionScheduleSearch): Observable<any> {

    return this.httpRequestService.postReqest(
      WebApiUrl + 'CollectionSchedule' + WebApiSaffix + '/Get', collectionScheduleSearch);
  }

  public GetReport(collectionScheduleSearch:CollectionScheduleSearch): Observable<any> {

    return this.httpRequestService.postReportReqest(
      WebApiUrl + 'CollectionSchedule' + WebApiSaffix + '/GetReport', collectionScheduleSearch);
  }

  public GetSpreadSheet(collectionScheduleSearch:CollectionScheduleSearch): Observable<any> {

    return this.httpRequestService.postSpreadSheetReqest(
      WebApiUrl + 'CollectionSchedule' + WebApiSaffix + '/GetSpreadSheet', collectionScheduleSearch);
  }



    
}
