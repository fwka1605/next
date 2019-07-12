import { Injectable } from '@angular/core';
import { UserInfoService } from '../common/user-info.service';
import { HttpRequestService } from '../common/http-request.service';
import { Observable } from 'rxjs';
import { WebApiUrl, WebApiSaffix } from 'src/app/common/const/http.const';
import { CustomerFeeSearch } from 'src/app/model/customer-fee-search.model';
import { MasterImportSource } from 'src/app/model/master-import-source.model';
import { CustomerFee } from 'src/app/model/customer-fee.model';

@Injectable({
  providedIn: 'root'
})
export class CustomerFeeMasterService {

  constructor(
    private userInfoService: UserInfoService,
    private httpRequestService: HttpRequestService
  ) { }


  public Get(option: CustomerFeeSearch): Observable<any> {
    option.companyId = this.userInfoService.Company.id;
    return this.httpRequestService.postReqest(
      WebApiUrl + 'CustomerFee' + WebApiSaffix + '/Get', option);
  }

  public Save(customerFees: Array<CustomerFee>): Observable<any> {
    return this.httpRequestService.postReqest(
      WebApiUrl + 'CustomerFee' + WebApiSaffix + '/Save', customerFees);
  }

  public Import(source: MasterImportSource): Observable<any> {
    source.companyId = this.userInfoService.Company.id;
    return this.httpRequestService.postReqest(
      WebApiUrl + 'CustomerFee' + WebApiSaffix + '/Import', source);
  }

}
