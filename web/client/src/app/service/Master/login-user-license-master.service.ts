import { Injectable } from '@angular/core';
import { LoginUserLicense } from 'src/app/model/login-user-license.model';
import { WebApiSaffix, WebApiUrl } from 'src/app/common/const/http.const';
import { Observable } from 'rxjs';
import { HttpRequestService } from '../common/http-request.service';

@Injectable({
  providedIn: 'root'
})
export class LoginUserLicenseMasterService {


  constructor(
    private httpRequestService:HttpRequestService
  ) { }

  public GetItems(loginUserLicense:LoginUserLicense): Observable<any>{

    return this.httpRequestService.postReqest
          (WebApiUrl + 'LoginUserLicense' + WebApiSaffix + '/GetItems', loginUserLicense);

  }
}
