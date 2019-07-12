import { Injectable } from '@angular/core';
import { HttpRequestService } from './common/http-request.service';
import { UserInfoService } from './common/user-info.service';
import { Observable } from 'rxjs';
import { ClientKeySearch } from '../model/client-key-search.model';
import { WebApiUrl, WebApiSaffix } from '../common/const/http.const';
import { StringUtil } from '../common/util/string-util';

@Injectable({
  providedIn: 'root'
})
export class DBService {

  constructor(
    private httpRequestService:HttpRequestService,
    private userInfoService: UserInfoService,

  ) { }

  public GetClientKey(programId="PE0101",clientName=null): Observable<any> {
    let clientKeySearch  = new ClientKeySearch ();

    clientKeySearch.programId= programId;

    if(StringUtil.IsNullOrEmpty(clientName)){
      clientKeySearch.clientName=this.userInfoService.AccessToken;
    }
    else{
      clientKeySearch.clientName=clientName;
    }

    clientKeySearch.companyCode=this.userInfoService.Company.code;
    clientKeySearch.loginUserCode=this.userInfoService.LoginUser.code;

    return this.httpRequestService.postReqest(
      WebApiUrl + 'Db' + WebApiSaffix + '/GetClientKey', clientKeySearch );
  }    

}
