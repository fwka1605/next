
import { Injectable } from '@angular/core';
import { MenuAuthoritiesResult } from 'src/app/model/menu-authorities-result.model';
import { MenuAuthority } from 'src/app/model/menu-authority.model';
import { ProcessResult } from 'src/app/model/process-result.model';
import { HttpRequestService } from '../common/http-request.service';
import { Observable } from 'rxjs';
import { WebApiUrl, WebApiSaffix } from 'src/app/common/const/http.const';
import { MenuAuthoritySearch } from 'src/app/model/menu-authority-search.model';
import { UserInfoService } from '../common/user-info.service';

@Injectable({
  providedIn: 'root'
})
export class MenuAuthorityMasterService {


  constructor(
    private httpRequestService:HttpRequestService,
    private userInforService: UserInfoService
  ) { }

  public GetItemsByMyCompany(): Observable<any>{
    let menuAuthoritySearch = new MenuAuthoritySearch();
    menuAuthoritySearch.companyId= this.userInforService.Company.id;
    return this.httpRequestService.postReqest(WebApiUrl + 'MenuAuthority' + WebApiSaffix + '/GetItems', menuAuthoritySearch);
  }

  public GetItemsByCompany(companyId:number): Observable<any>{
    let menuAuthoritySearch = new MenuAuthoritySearch();
    menuAuthoritySearch.companyId= companyId;
    return this.httpRequestService.postReqest(WebApiUrl + 'MenuAuthority' + WebApiSaffix + '/GetItems', menuAuthoritySearch);
  }

  public GetAllItems(): Observable<any>{
    let menuAuthoritySearch = new MenuAuthoritySearch();
    return this.httpRequestService.postReqest(WebApiUrl + 'MenuAuthority' + WebApiSaffix + '/GetItems', menuAuthoritySearch);
  }


}
