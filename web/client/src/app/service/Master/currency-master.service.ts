import { Injectable } from '@angular/core';
import { CurrenciesResult } from 'src/app/model/currencies-result.model';
import { ProcessResult } from 'src/app/model/process-result.model';
import { Currency } from 'src/app/model/currency.model';
import { HttpRequestService } from '../common/http-request.service';
import { UserInfoService } from '../common/user-info.service';
import { Observable } from 'rxjs';
import { CurrencySearch } from 'src/app/model/currency-search.model';
import { WebApiUrl, WebApiSaffix } from 'src/app/common/const/http.const';
import { MasterSearchOption } from 'src/app/model/master-search-option';
import { StringUtil } from 'src/app/common/util/string-util';

@Injectable({
  providedIn: 'root'
})
export class CurrencyMasterService {

  private currenciesResult:CurrenciesResult=null;


  constructor(
    private httpRequestService: HttpRequestService,
    private userInfoService: UserInfoService,

  ) { }


  public GetItems(currencyCode: string=""): Observable<any> {

    let currencySearch = new CurrencySearch();
    currencySearch.companyId = this.userInfoService.Company.id;
    if(!StringUtil.IsNullOrEmpty(currencyCode)){
      currencySearch.codes = new Array<string>();
      currencySearch.codes.push(currencyCode);
    }
    return this.httpRequestService.postReqest(WebApiUrl + 'Currency' + WebApiSaffix + '/GetItems', currencySearch);

  }
  
}
