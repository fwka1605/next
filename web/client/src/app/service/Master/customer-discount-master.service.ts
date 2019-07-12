import { Injectable } from '@angular/core';
import { HttpRequestService } from '../common/http-request.service';
import { UserInfoService } from '../common/user-info.service';
import { Observable } from 'rxjs';
import { WebApiUrl, WebApiSaffix } from 'src/app/common/const/http.const';

@Injectable({
  providedIn: 'root'
})
export class CustomerDiscountMasterService {

  constructor(
    private httpRequestService: HttpRequestService,
    private userInfoService: UserInfoService,
  ) { }

  ///// Exist //////////////////////////////////////////////////////////////////////
  public ExistAccountTitle(acccountTitleId: number): Observable<any> {
    return this.httpRequestService.postReqest(
      WebApiUrl + 'CustomerDiscount' + WebApiSaffix + '/ExistAccountTitle', acccountTitleId);
  }

}
