import { Injectable } from '@angular/core';
import { TaxClass  } from 'src/app/model/tax-class.model';
import { TaxClassResult } from 'src/app/model/tax-class-result.model';
import { ProcessResult } from 'src/app/model/process-result.model';
import { Observable } from 'rxjs';
import { UserInfoService } from '../common/user-info.service';
import { HttpRequestService } from '../common/http-request.service';
import { WebApiUrl, WebApiSaffix } from 'src/app/common/const/http.const';

@Injectable({
  providedIn: 'root'
})
export class TaxClassMasterService {

  constructor(
    private httpRequestService: HttpRequestService,
    private userInfoService: UserInfoService,    
  ) { }

  public GetItems(taxid: number): Observable<any> {

    let taxClass = new TaxClass();
    taxClass.id = taxid;
    taxClass.name = "";
    return this.httpRequestService.postReqest(WebApiUrl + 'TaxClass' + WebApiSaffix + '/GetItems', taxClass);
  }  


}
