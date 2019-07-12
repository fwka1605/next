
import { Injectable } from '@angular/core';
import { Customer } from 'src/app/model/customer.model';
import { CustomersResult } from 'src/app/model/customers-result.model';
import { CustomerResult } from 'src/app/model/customer-result.model';
import { ProcessResult } from 'src/app/model/process-result.model';
import { Observable } from 'rxjs';
import { CustomerSearch } from 'src/app/model/customer-search.model';
import { UserInfoService } from '../common/user-info.service';
import { HttpRequestService } from '../common/http-request.service';
import { WebApiUrl, WebApiSaffix } from 'src/app/common/const/http.const';
import { MasterSearchOption } from 'src/app/model/master-search-option';
import { StringUtil } from 'src/app/common/util/string-util';
import { CustomerFeeSearch } from 'src/app/model/customer-fee-search.model';
import { MasterImportSource } from 'src/app/model/master-import-source.model';

@Injectable({
  providedIn: 'root'
})
export class CustomerMasterService {

  public selectCustmer: Customer = new Customer();;

  constructor(
    private userInfoService: UserInfoService,
    private httpRequestService: HttpRequestService
  ) { }

  public GetItems(customerCode: string = "", isParent: number = -1): Observable<any> {

    let customerSearch = new CustomerSearch();
    customerSearch.companyId = this.userInfoService.Company.id;
    if (!StringUtil.IsNullOrEmpty(customerCode)) {
      customerSearch.codes = new Array<string>();
      customerSearch.codes.push(customerCode);
    }

    if (isParent >= 0) {
      customerSearch.isParent = isParent;
    }
    return this.httpRequestService.postReqest(WebApiUrl + 'Customer' + WebApiSaffix + '/GetItems', customerSearch);
  }

  public GetItemsById(customerid: number, isParent: number = -1): Observable<any> {

    let customerSearch = new CustomerSearch();
    customerSearch.companyId = this.userInfoService.Company.id;
    customerSearch.ids = new Array<number>();
    customerSearch.ids.push(customerid);

    if (isParent >= 0) {
      customerSearch.isParent = isParent;
    }
    return this.httpRequestService.postReqest(WebApiUrl + 'Customer' + WebApiSaffix + '/GetItems', customerSearch);
  }

  public GetItemsByIds(customerids: Array<number>, isParent: number = -1): Observable<any> {

    let customerSearch = new CustomerSearch();
    customerSearch.companyId = this.userInfoService.Company.id;
    customerSearch.ids = new Array<number>();
    customerids.forEach(element=>{customerSearch.ids.push(element)});

    if (isParent >= 0) {
      customerSearch.isParent = isParent;
    }
    return this.httpRequestService.postReqest(WebApiUrl + 'Customer' + WebApiSaffix + '/GetItems', customerSearch);
  }


  public GetItemsByCustomerSearch(option: CustomerSearch): Observable<any> {
    option.companyId = this.userInfoService.Company.id;
    return this.httpRequestService.postReqest(
      WebApiUrl + 'Customer' + WebApiSaffix + '/GetItems', option);
  }

  public Save(customer: Customer): Observable<any> {
    customer.companyId = this.userInfoService.Company.id;
    customer.updateBy = this.userInfoService.LoginUser.id;
    if (customer.id == 0) {
      customer.createBy = this.userInfoService.LoginUser.id;
    }

    return this.httpRequestService.postReqest(
      WebApiUrl + 'Customer' + WebApiSaffix + '/Save', customer);
  }

  public Delete(customerId: number): Observable<any> {
    return this.httpRequestService.postReqest(
      WebApiUrl + 'Customer' + WebApiSaffix + '/Delete', customerId);
  }

  public GetReport(option: CustomerSearch): Observable<any> {
    option.companyId = this.userInfoService.Company.id;

    return this.httpRequestService.postReportReqest(
      WebApiUrl + 'Customer' + WebApiSaffix + '/GetReport', option);
  }

  public GetRegisterReport(option: CustomerSearch): Observable<any> {
    option.companyId = this.userInfoService.Company.id;

    return this.httpRequestService.postReportReqest(
      WebApiUrl + 'Customer' + WebApiSaffix + '/GetRegisterReport', option);
  }

  public GetFeeReport(option: CustomerFeeSearch): Observable<any> {
    option.companyId = this.userInfoService.Company.id;
    option.forPrint = true;
    return this.httpRequestService.postReportReqest(
      WebApiUrl + 'Customer' + WebApiSaffix + '/GetFeeReport', option);
  }

  public Import(source: MasterImportSource): Observable<any> {
    source.companyId = this.userInfoService.Company.id;
    source.loginUserId = this.userInfoService.LoginUser.id;
    return this.httpRequestService.postReqest(
      WebApiUrl + 'Customer' + WebApiSaffix + '/Import', source);
  }


  ///// Exist //////////////////////////////////////////////////////////////////////
  public ExistStaff(staffId: number) {
    return this.httpRequestService.postReqest(
      WebApiUrl + 'Customer' + WebApiSaffix + '/ExistStaff', staffId);
  }

  public ExistCategory(categoryId: number) {
    return this.httpRequestService.postReqest(
      WebApiUrl + 'Customer' + WebApiSaffix + '/ExistCategory', categoryId);
  }

  getCustomerMin(): Observable<any> {
    return this.httpRequestService.postReqest(
      WebApiUrl + 'Customer' + WebApiSaffix + '/GetMinItems', this.userInfoService.Company.id);
  }


}
