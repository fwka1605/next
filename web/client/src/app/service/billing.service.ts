import { Injectable } from '@angular/core';
import { Billing } from '../model/billing.model';
import { BillingsResult } from '../model/billings-result.model';
import { BillingSearch } from '../model/billing-search.model';
import { BillingDueAtModify } from '../model/billing-due-at-modify.model';
import { JournalizingSummary } from '../model/journalizing-summary.model';
import { JournalizingSummariesResult } from '../model/journalizing-summaries-result.model';
import { ProcessResult } from '../model/process-result.model';
import { WebApiUrl, WebApiSaffix } from 'src/app/common/const/http.const';
import { Observable } from 'rxjs';
import { HttpRequestService } from './common/http-request.service';
import { UserInfoService } from './common/user-info.service';
import { JournalizingOption } from '../model/journalizing-option.model';
import { BillingDeleteSource } from '../model/billing-delete-source.model';
import { BillingJournalizingOption } from '../model/billing-journalizing-option.model';
import { OmitSource } from '../model/omit-source.model';
import { TransactionImportSource } from '../model/transaction-import-source.model';
import { FormGroup } from '@angular/forms';
import { BillingMemo } from '../model/billing-memo.model';

@Injectable({
  providedIn: 'root'
})
export class BillingService {

  private billingsSearch: BillingSearch;
  public get BillingSearch(): BillingSearch {
    return this.billingsSearch;
  }
  public set BillingSearch(value: BillingSearch) {
    this.billingsSearch = value;
  }
  
  private billingSearchFormGroup: FormGroup;
  public get BillingSearchFormGroup(): FormGroup {
    return this.billingSearchFormGroup;
  }
  public set BillingSearchFormGroup(value: FormGroup) {
    this.billingSearchFormGroup = value;
  }

  constructor(
    private httpRequestService: HttpRequestService,
    private userInfoService: UserInfoService,
  ) { }


  ///// Get //////////////////////////////////////////////////////////////////////
  public GetItems(billingSearch: BillingSearch): Observable<any> {
    return this.httpRequestService.postReqest(
      WebApiUrl + 'Billing' + WebApiSaffix + '/GetItems', billingSearch);
  }

  public GetDueAtModifyItems(billingSearch: BillingSearch): Observable<any> {
    return this.httpRequestService.postReqest(
      WebApiUrl + 'Billing' + WebApiSaffix + '/GetDueAtModifyItems', billingSearch);
  }

  public GetBillingJournalizingSummary(journalizingOption: BillingJournalizingOption): Observable<any> {
    return this.httpRequestService.postReqest(
      WebApiUrl + 'Billing' + WebApiSaffix + '/GetBillingJournalizingSummary', journalizingOption);
  }

  public GetJournalizingReport(journalizingOption: BillingJournalizingOption): Observable<any> {
    return this.httpRequestService.postReportReqest(
      WebApiUrl + 'Billing' + WebApiSaffix + '/GetJournalizingReport', journalizingOption);
  }

  public GetMemo(id: number): Observable<any> {
    return this.httpRequestService.postReqest(
      WebApiUrl + 'Billing' + WebApiSaffix + '/GetMemo', id);
  }

  public Get(ids: number[]): Observable<any> {
    return this.httpRequestService.postReqest(
      WebApiUrl + 'Billing' + WebApiSaffix + '/Get', ids);
  }

  public GetImportNewCustomer(transactionImportSource: TransactionImportSource): Observable<any>{
    return this.httpRequestService.postReqest(
      WebApiUrl + 'Billing' + WebApiSaffix + '/GetImportNewCustomer', transactionImportSource);    
  }

  ///// Other //////////////////////////////////////////////////////////////////////
  public Save(billings: Billing[]): Observable<any> {
    return this.httpRequestService.postReqest(
      WebApiUrl + 'Billing' + WebApiSaffix + '/Save', billings);
  }

  public SaveMemo(billingMemo: BillingMemo): Observable<any> {
    return this.httpRequestService.postReqest(
      WebApiUrl + 'Billing' + WebApiSaffix + '/SaveMemo', 
      billingMemo);
  }

  public Delete(billingDeleteSource: BillingDeleteSource): Observable<any> {
    return this.httpRequestService.postReqest(
      WebApiUrl + 'Billing' + WebApiSaffix + '/Delete', billingDeleteSource);
  }

  public UpdateDueAt(billingDueAtModify: BillingDueAtModify[]): Observable<any> {
    return this.httpRequestService.postReqest(
      WebApiUrl + 'Billing' + WebApiSaffix + '/UpdateDueAt', billingDueAtModify);
  }

  public UpdateOutputAt(journalizingOption: BillingJournalizingOption): Observable<any> {
    return this.httpRequestService.postReqest(
      WebApiUrl + 'Billing' + WebApiSaffix + '/UpdateOutputAt', journalizingOption);
  }

  public ExtractBillingJournalizing(journalizingOption: BillingJournalizingOption): Observable<any> {
    return this.httpRequestService.postReqest(
      WebApiUrl + 'Billing' + WebApiSaffix + '/ExtractBillingJournalizing', journalizingOption);
  }

  public CancelBillingJournalizing(journalizingOption: BillingJournalizingOption): Observable<any> {
    return this.httpRequestService.postReqest(
      WebApiUrl + 'Billing' + WebApiSaffix + '/CancelBillingJournalizing', journalizingOption);
  }

  public Omit(omitSource:OmitSource): Observable<any> {
    return this.httpRequestService.postReqest(
      WebApiUrl + 'Billing' + WebApiSaffix + '/Omit', omitSource);
  }

  public Read(transactionImportSource: TransactionImportSource):Observable<any> {
    return this.httpRequestService.postReqest(
      WebApiUrl + 'Billing' + WebApiSaffix + '/Read', 
      transactionImportSource);
  }

  public Import(transactionImportSource: TransactionImportSource):Observable<any> {
    return this.httpRequestService.postReqest(
      WebApiUrl + 'Billing' + WebApiSaffix + '/Import', 
      transactionImportSource);
  }

  ///// Exist //////////////////////////////////////////////////////////////////////
  public ExistCustomer(customerId: number): Observable<any> {
    return this.httpRequestService.postReqest(
      WebApiUrl + 'Billing' + WebApiSaffix + '/ExistCustomer', customerId);
  }

  public ExistDepartment(departmentId: number): Observable<any> {
    return this.httpRequestService.postReqest(
      WebApiUrl + 'Billing' + WebApiSaffix + '/ExistDepartment', departmentId);
  }

  public ExistStaff(staffId: number) {
    return this.httpRequestService.postReqest(
      WebApiUrl + 'Billing' + WebApiSaffix + '/ExistStaff', staffId);
  }

  public ExistAccountTitle(acccountTitleId: number): Observable<any> {
    return this.httpRequestService.postReqest(
      WebApiUrl + 'Billing' + WebApiSaffix + '/ExistAccountTitle', acccountTitleId);
  }

  public ExistBillingCategory(categoryId: number) {
    return this.httpRequestService.postReqest(
      WebApiUrl + 'Billing' + WebApiSaffix + '/ExistBillingCategory', categoryId);
  }

  public ExistCollectCategory(categoryId: number) {
    return this.httpRequestService.postReqest(
      WebApiUrl + 'Billing' + WebApiSaffix + '/ExistCollectCategory', categoryId);
  }

    ///// PDF ////////////////////////////////////////////////////////////////////////
    public GetReport(billingSearch :BillingSearch ): Observable<any>{
      return this.httpRequestService.postReportReqest(
          WebApiUrl + 'Billing' + WebApiSaffix + '/GetReport', billingSearch);
    }
  
    public GetImportValidationReport(transactionImportSource: TransactionImportSource): Observable<any> {
      return this.httpRequestService.postReportReqest(
        WebApiUrl + 'Billing' + WebApiSaffix + '/GetImportValidationReport', 
        transactionImportSource);
    }

    public GetImportNewCustomerReport(transactionImportSource: TransactionImportSource): Observable<any> {
      return this.httpRequestService.postReportReqest(
        WebApiUrl + 'Billing' + WebApiSaffix + '/GetImportNewCustomerReport', 
        transactionImportSource);
    }

}
