import { Injectable } from '@angular/core';
import { FunctionAuthoritiesResult } from 'src/app/model/function-authorities-result.model';
import { ProcessResult } from 'src/app/model/process-result.model';
import { FunctionAuthority } from 'src/app/model/function-authority.model';
import { HttpRequestService } from '../common/http-request.service';
import { Observable } from 'rxjs';
import { WebApiUrl, WebApiSaffix } from 'src/app/common/const/http.const';
import { FunctionAuthoritySearch } from 'src/app/model/function-authority-search.model';
import { UserInfoService } from '../common/user-info.service';

@Injectable({
  providedIn: 'root'
})
export class FunctionAuthorityMasterService {

  constructor(
    private httpRequestService:HttpRequestService,
    private userInfoService: UserInfoService,
  ) { }

  public GetItems(companyId:number=-1): Observable<any>{
    let functionAuthoritySearch = new FunctionAuthoritySearch();
    if(companyId==-1){
      functionAuthoritySearch.companyId=this.userInfoService.Company.id; 
    }
    else{
      functionAuthoritySearch.companyId=companyId; 
    }
    return this.httpRequestService.postReqest(WebApiUrl + 'FunctionAuthority' + WebApiSaffix + '/GetItems', functionAuthoritySearch);
  }


}
