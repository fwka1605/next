import { Injectable } from '@angular/core';
import { HolidayCalendar } from 'src/app/model/holiday-calendar.model';
import { HolidayCalendarsResult } from 'src/app/model/holiday-calendars-result.model';
import { ProcessResult } from 'src/app/model/process-result.model';
import { WebApiUrl, WebApiSaffix } from 'src/app/common/const/http.const';
import { HttpRequestService } from '../common/http-request.service';
import { Observable } from 'rxjs';
import { HolidayCalendarSearch } from 'src/app/model/holiday-calendar-search.model';
import { MasterImportSource } from 'src/app/model/master-import-source.model';

@Injectable({
  providedIn: 'root'
})
export class HolidayCalendarMasterService {

  constructor(
    private httpRequestService: HttpRequestService,
  ) { }

  public GetItems(option: HolidayCalendarSearch): Observable<any> {
    return this.httpRequestService.postReqest(
      WebApiUrl + 'HolidayCalendar' + WebApiSaffix + '/GetItems', option);
  }

  public Save(holidays: Array<HolidayCalendar>): Observable<any> {
    return this.httpRequestService.postReqest(
      WebApiUrl + 'HolidayCalendar' + WebApiSaffix + '/Save', holidays);
  }

  public Delete(holidays: Array<HolidayCalendar>): Observable<any> {
    return this.httpRequestService.postReqest(
      WebApiUrl + 'HolidayCalendar' + WebApiSaffix + '/Delete', holidays);
  }

  public Import(source: MasterImportSource): Observable < any > {
    return this.httpRequestService.postReqest(
      WebApiUrl + 'HolidayCalendar' + WebApiSaffix + '/Import', source);
  }

}
