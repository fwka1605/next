import { Injectable } from '@angular/core';
import { HttpRequestService } from '../common/http-request.service';
import { UserInfoService } from '../common/user-info.service';
import { PaymentAgencyFeeSearch } from 'src/app/model/payment-agency-fee-search.model';
import { WebApiUrl, WebApiSaffix } from 'src/app/common/const/http.const';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class PaymentAgencyFeeMasterService {


  constructor(
    private httpRequestService: HttpRequestService,
    private userInfoService: UserInfoService,

  ) { }


  public GetItems(paymentAgencyFeeSearch:PaymentAgencyFeeSearch): Observable<any> {

    return this.httpRequestService.postReqest(
        WebApiUrl + 'PaymentAgencyFee' + WebApiSaffix + '/GetItems', 
        paymentAgencyFeeSearch);

  }
  
}
