import { Injectable } from '@angular/core';
import { HttpRequestService } from '../common/http-request.service';
import { Observable } from 'rxjs';
import { WebApiUrl, WebApiSaffix } from 'src/app/common/const/http.const';
import { UserInfoService } from '../common/user-info.service';

@Injectable({
  providedIn: 'root'
})
export class PasswordPolicyMasterService {


  constructor(
    private userInfoService:UserInfoService,
    private httpRequestService:HttpRequestService
  ) { }

  public Get(companyId:number=-1): Observable<any>{

    if(companyId==-1){
      companyId=this.userInfoService.Company.id;
    }

    return this.httpRequestService.postReqest(WebApiUrl + 'PasswordPolicy' + WebApiSaffix + '/Get', companyId);

  }

}
