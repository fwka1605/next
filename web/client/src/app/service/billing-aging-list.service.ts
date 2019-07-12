import { Injectable } from '@angular/core';
import { FormGroup } from '@angular/forms';
import { BillingAgingList } from '../model/billing-aging-list.model';
import {  BillingAgingListsResult } from '../model/billing-aging-lists-result.model';
import { BillingAgingListSearch } from '../model/billing-aging-list-search.model';
import { ProcessResult } from '../model/process-result.model';
import { HttpRequestService } from './common/http-request.service';
import { UserInfoService } from './common/user-info.service';
import { Observable } from 'rxjs';
import { WebApiSaffix, WebApiUrl } from '../common/const/http.const';
import { BillingAgingListDetail } from 'src/app/model/billing-aging-list-detail.model';


@Injectable({
  providedIn: 'root'
})
export class BillingAgingListService {

  private billingAgingListDetails: BillingAgingListDetail[];
  public get BillingAgingListDetails(): BillingAgingListDetail[] {
    return this.billingAgingListDetails;
  }
  public set BillingAgingListDetails(value: BillingAgingListDetail[]) {
    this.billingAgingListDetails = value;
  }

  private optionInfo: BillingAgingListSearch;
  public get OptionInfo(): BillingAgingListSearch {
    return this.optionInfo;
  }
  public set OptionInfo(value: BillingAgingListSearch) {
    this.optionInfo = value;
  }

  private billingAgingListFormGroup: FormGroup;
  public get BillingAgingListFormGroup(): FormGroup {
    return this.billingAgingListFormGroup;
  }
  public set BillingAgingListFormGroup(value: FormGroup) {
    this.billingAgingListFormGroup = value;
  }

  constructor(
    private httpRequestService: HttpRequestService,
    private userInfoService: UserInfoService,

    ) { }

  public Get(billingAgingListSearch: BillingAgingListSearch): Observable<any> {

    return this.httpRequestService.postReqest(
      WebApiUrl + 'BillingAgingList' + WebApiSaffix + '/Get', billingAgingListSearch );
  }

  public GetReport(billingAgingListSearch: BillingAgingListSearch): Observable<any> {

    return this.httpRequestService.postReportReqest(
      WebApiUrl + 'BillingAgingList' + WebApiSaffix + '/GetReport', billingAgingListSearch );
  }

  public GetSpreadsheet(billingAgingListSearch: BillingAgingListSearch): Observable<any> {

    return this.httpRequestService.postSpreadSheetReqest(
      WebApiUrl + 'BillingAgingList' + WebApiSaffix + '/GetSpreadsheet', billingAgingListSearch );
  }  
  
  public GetDetailsAsync(billingAgingListSearch: BillingAgingListSearch): Observable<any> {

    return this.httpRequestService.postReqest(
      WebApiUrl + 'BillingAgingList' + WebApiSaffix + '/GetDetailsAsync', billingAgingListSearch );
  }  
}
