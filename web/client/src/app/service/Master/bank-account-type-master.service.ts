import { Injectable } from '@angular/core';
import { HttpRequestService } from '../common/http-request.service';
import { UserInfoService } from '../common/user-info.service';
import { Observable } from 'rxjs';
import { WebApiSaffix, WebApiUrl } from 'src/app/common/const/http.const';

@Injectable({
  providedIn: 'root'
})
export class BankAccountTypeMasterService {


  constructor(
    private httpRequestService: HttpRequestService,
    private userInfoService: UserInfoService,

  ) { }  

  public GetItems(): Observable<any> {

    return this.httpRequestService.postReqest(WebApiUrl + 'BankAccountType' + WebApiSaffix + '/GetItems', null);

  }

}
