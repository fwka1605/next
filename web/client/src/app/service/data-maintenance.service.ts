import { Injectable } from '@angular/core';
import { HttpRequestService } from './common/http-request.service';
import { UserInfoService } from './common/user-info.service';
import { Observable } from 'rxjs';
import { WebApiUrl, WebApiSaffix } from '../common/const/http.const';

@Injectable({
  providedIn: 'root'
})
export class DataMaintenanceService {

  constructor(
    private httpRequestService:HttpRequestService,
    private userInfoService: UserInfoService,
  ) { }

  public DeleteData(deleteDate:string): Observable<any>{
    return this.httpRequestService.postReqest(
      // https://stackoverflow.com/questions/47354807/how-to-post-a-string-in-the-body-of-a-post-request-with-angular-4-3-httpclient
      WebApiUrl + 'DataMaintenance' + WebApiSaffix + '/DeleteData', `\"${deleteDate}\"`);
    }  

}
