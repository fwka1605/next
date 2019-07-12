import { Injectable } from '@angular/core';
import { UserInfoService } from '../common/user-info.service';
import { HttpRequestService } from '../common/http-request.service';
import { Observable } from 'rxjs';
import { WebApiUrl, WebApiSaffix } from 'src/app/common/const/http.const';
import { CustomerPaymentContract } from 'src/app/model/customer-payment-contract.model';

@Injectable({
  providedIn: 'root'
})
export class CustomerPaymentContractMasterService {

  constructor(
    private userInfoService: UserInfoService,
    private httpRequestService: HttpRequestService
  ) { }

  public GetItems(customerIds: Array<number>): Observable<any> {
    return this.httpRequestService.postReqest(
    WebApiUrl + 'CustomerPaymentContract' + WebApiSaffix + '/GetItems', customerIds);
  }

  public Save(contract: CustomerPaymentContract): Observable<any> {
    return this.httpRequestService.postReqest(
      WebApiUrl + 'CustomerPaymentContract' + WebApiSaffix + '/Save', contract);
  }

  public Delete(customerId: number): Observable<any> {
    return this.httpRequestService.postReqest(
      WebApiUrl + 'CustomerPaymentContract' + WebApiSaffix + '/Delete', customerId);
  }


  ///// Exist //////////////////////////////////////////////////////////////////////
  public ExistCategory(categoryId: number) {
    return this.httpRequestService.postReqest(
      WebApiUrl + 'CustomerPaymentContract' + WebApiSaffix + '/ExistCategory', categoryId);
  }  
}
