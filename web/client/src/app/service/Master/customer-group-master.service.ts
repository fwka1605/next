import { CustomerGroupSearch } from '../../model/customer-group-search.model';
import { Injectable } from '@angular/core';
import { HttpRequestService } from '../common/http-request.service';
import { Observable } from 'rxjs';
import { UserInfoService } from '../common/user-info.service';
import { WebApiUrl, WebApiSaffix } from 'src/app/common/const/http.const';
import { MasterImportData } from 'src/app/model/master-import-data.model';
import { CustomerGroup } from 'src/app/model/customer-group.model';
import { MasterImportSource } from 'src/app/model/master-import-source.model';

@Injectable({
  providedIn: 'root'
})
export class CustomerGroupMasterService {

  constructor(
    private httpRequestService: HttpRequestService,
    private userInfoService: UserInfoService
  ) { }


  public GetItems(parentId: number = 0): Observable<any> {

    let customerGroupSearch = new CustomerGroupSearch();
    customerGroupSearch.companyId = this.userInfoService.Company.id;

    if (parentId > 0) {
      customerGroupSearch.parentIds = new Array<number>();
      customerGroupSearch.parentIds.push(parentId);
    }

    return this.httpRequestService.postReqest(
      WebApiUrl + 'CustomerGroup' + WebApiSaffix + '/GetItems', customerGroupSearch);
  }

  public Save(importData: MasterImportData<CustomerGroup>): Observable<any> {
    return this.httpRequestService.postReqest(
      WebApiUrl + 'CustomerGroup' + WebApiSaffix + '/Save', importData);
  }

  public Import(source: MasterImportSource): Observable<any> {
    return this.httpRequestService.postReqest(
      WebApiUrl + 'CustomerGroup' + WebApiSaffix + '/Import', source);
  }

  public HasChild(customerCode: number): Observable<any> {
    return this.httpRequestService.postReqest(
      WebApiUrl + 'CustomerGroup' + WebApiSaffix + '/HasChild', customerCode);
  }  

  public GetReport(): Observable<any> {
    return this.httpRequestService.postReportReqest(
      WebApiUrl + 'CustomerGroup' + WebApiSaffix + '/GetReport', this.userInfoService.Company.id);
  }

  ///// Exist //////////////////////////////////////////////////////////////////////
  public ExistCustomer(customerId: number): Observable<any> {
    return this.httpRequestService.postReqest(
      WebApiUrl + 'CustomerGroup' + WebApiSaffix + '/ExistCustomer', customerId);
  }

}
