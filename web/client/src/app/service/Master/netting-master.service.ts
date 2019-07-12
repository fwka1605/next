import { Injectable } from '@angular/core';
import { HttpRequestService } from '../common/http-request.service';
import { UserInfoService } from '../common/user-info.service';
import { Observable } from 'rxjs';
import { WebApiUrl, WebApiSaffix } from 'src/app/common/const/http.const';

@Injectable({
  providedIn: 'root'
})
export class NettingMasterService {

  constructor(
    private httpRequestService: HttpRequestService,
    private userInfoService: UserInfoService
  ) { }

  ///// Exist ///////////////////////////////////////////////////////////////////////////////  
  public ExistCustomer(customerId: number): Observable<any> {
    return this.httpRequestService.postReqest(
      WebApiUrl + 'Netting' + WebApiSaffix + '/ExistCustomer', customerId);
  }

  public ExistReceiptCategory(categoryId: number) {
    return this.httpRequestService.postReqest(
      WebApiUrl + 'Netting' + WebApiSaffix + '/ExistReceiptCategory', categoryId);
  }

}
