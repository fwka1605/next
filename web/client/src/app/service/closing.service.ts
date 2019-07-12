import { Injectable } from '@angular/core';
import { HttpRequestService } from './common/http-request.service';
import { UserInfoService } from './common/user-info.service';
import { Observable } from 'rxjs';
import { WebApiUrl, WebApiSaffix } from '../common/const/http.const';

@Injectable({
  providedIn: 'root'
})
export class ClosingService {

  constructor(
    private httpRequestService: HttpRequestService,
    private userInfoService: UserInfoService,    
  ) { }

  public GetClosingInformation(companyId:number=-1): Observable<any> {

    if(companyId==-1){
      companyId = this.userInfoService.Company.id;
    }
    return this.httpRequestService.postReqest(WebApiUrl + 'Closing' + WebApiSaffix + '/GetClosingInformation', companyId);
  }

}
