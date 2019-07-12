import { Injectable } from '@angular/core';
import { CreditAgingListSearch } from 'src/app/model/credit-aging-list-search.model';
import { HttpRequestService } from './common/http-request.service';
import { UserInfoService } from './common/user-info.service';
import { Observable } from 'rxjs';
import { WebApiSaffix, WebApiUrl } from '../common/const/http.const';

@Injectable({
  providedIn: 'root'
})
export class CreditAgingListService {

  constructor(
    private httpRequestService: HttpRequestService,
    private userInfoService: UserInfoService,    
  ) { }

  public Get(creditAgingListSearch: CreditAgingListSearch): Observable<any> {

    return this.httpRequestService.postReqest(
      WebApiUrl + 'CreditAgingList' + WebApiSaffix + '/Get', creditAgingListSearch );
  }

  public GetSpreadsheet(creditAgingListSearch: CreditAgingListSearch): Observable<any> {

    return this.httpRequestService.postSpreadSheetReqest(
      WebApiUrl + 'CreditAgingList' + WebApiSaffix + '/GetSpreadsheet', creditAgingListSearch );
  }  

}
