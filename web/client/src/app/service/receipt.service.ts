import { Injectable } from '@angular/core';
import { ReceiptExclude } from '../model/receipt-exclude.model';
import { ReceiptSearch } from '../model/receipt-search.model';
import { HttpRequestService } from './common/http-request.service';
import { UserInfoService } from './common/user-info.service';
import { Observable } from 'rxjs';
import { WebApiUrl, WebApiSaffix } from '../common/const/http.const';
import { ReceiptSaveItem } from '../model/receipt-save-item.model';
import { JournalizingOption } from '../model/journalizing-option.model';
import { ReceiptApportion } from '../model/receipt-apportion.model';
import { FormGroup } from '@angular/forms';
import { AdvanceReceived } from '../model/advance-received.model';
import { OmitSource } from '../model/omit-source.model';
import { TransactionImportSource } from '../model/transaction-import-source.model';
import { ReceiptMemo } from 'src/app/model/receipt-memo.model';
import { ReceiptSectionTransfer } from '../model/receipt-section-transfer.model';
import { AdvanceReceivedSplitSource } from 'src/app/model/advance-received-split-source.model'


@Injectable({
  providedIn: 'root'
})
export class ReceiptService {

  private receiptsSearch: ReceiptSearch;
  public get ReceiptSearch(): ReceiptSearch {
    return this.receiptsSearch;
  }
  public set ReceiptSearch(value: ReceiptSearch) {
    this.receiptsSearch = value;
  }

  private receiptSearchFormGroup: FormGroup;
  public get ReceiptSearchFormGroup(): FormGroup {
    return this.receiptSearchFormGroup;
  }
  public set ReceiptSearchFormGroup(value: FormGroup) {
    this.receiptSearchFormGroup = value;
  }

  private receiptInputFormGroup: FormGroup;
  public get ReceiptInputFormGroup(): FormGroup {
    return this.receiptInputFormGroup;
  }
  public set ReceiptInputFormGroup(value: FormGroup) {
    this.receiptInputFormGroup = value;
  }

  private advanceReceivedFormGroup: FormGroup;
  public get AdvanceReceivedFormGroup(): FormGroup {
    return this.advanceReceivedFormGroup;
  }
  
  public set AdvanceReceivedFormGroup(value: FormGroup) {
    this.advanceReceivedFormGroup = value;
  }

  constructor(
    private httpRequestService: HttpRequestService,
    private userInfoService: UserInfoService,
  ) { }


  ///// Get ////////////////////////////////////////////////////////////////////////
  public GetHeaderItems(): Observable<any> {
    return this.httpRequestService.postReqest(
      WebApiUrl + 'Receipt' + WebApiSaffix + '/GetHeaderItems',
      this.userInfoService.Company.id);
  }

  public GetItems(receiptSearch: ReceiptSearch): Observable<any> {
    return this.httpRequestService.postReqest(
      WebApiUrl + 'Receipt' + WebApiSaffix + '/GetItems',
      receiptSearch);
  }

  public GetApportionItems(headerIds: number[]): Observable<any> {
    return this.httpRequestService.postReqest(
      WebApiUrl + 'Receipt' + WebApiSaffix + '/GetApportionItems', headerIds);
  }

  public Get(id: number): Observable<any> {
    let ids = new Array<Number>();
    ids.push(id);

    return this.httpRequestService.postReqest(
      WebApiUrl + 'Receipt' + WebApiSaffix + '/Get', ids);
  }

  public GetMemo(id: number): Observable<any> {
    return this.httpRequestService.postReqest(
      WebApiUrl + 'Receipt' + WebApiSaffix + '/GetMemo', id);
  }


  public ExtractReceiptJournalizingAsync(journalizingOption: JournalizingOption): Observable<any> {

    return this.httpRequestService.postReqest(
      WebApiUrl + 'Receipt' + WebApiSaffix + '/ExtractReceiptJournalizingAsync',
      journalizingOption);
  }

  public GetReceiptJournalizingSummary(journalizingOption: JournalizingOption): Observable<any> {

    return this.httpRequestService.postReqest(
      WebApiUrl + 'Receipt' + WebApiSaffix + '/GetReceiptJournalizingSummaryAsync',
      journalizingOption);
  }

  public Apportion(receiptApportions: Array<ReceiptApportion>) {

    return this.httpRequestService.postReqest(
      WebApiUrl + 'Receipt' + WebApiSaffix + '/Apportion',
      receiptApportions);
  }

  public GetAdvanceReceipts(originalReceiptId: number): Observable<any> {
    return this.httpRequestService.postReqest(
      WebApiUrl + 'Receipt' + WebApiSaffix + '/GetAdvanceReceipts',
      originalReceiptId);
  }

  ///// Save ///////////////////////////////////////////////////////////////////////
  public Save(receiptSaveItem: ReceiptSaveItem): Observable<any> {
    return this.httpRequestService.postReqest(
      WebApiUrl + 'Receipt' + WebApiSaffix + '/Save', receiptSaveItem);
  }

  public SaveMemo(receiptMemo: ReceiptMemo): Observable<any> {
    return this.httpRequestService.postReqest(
      WebApiUrl + 'Receipt' + WebApiSaffix + '/SaveMemo', receiptMemo);
  }

  public SaveExcludeAmount(receiptExcludes: Array<ReceiptExclude>): Observable<any> {
    return this.httpRequestService.postReqest(
      WebApiUrl + 'Receipt' + WebApiSaffix + '/SaveExcludeAmount', receiptExcludes);
  }

  public SaveReceiptSectionTransfer(receiptSectionTransfers: Array<ReceiptSectionTransfer>): Observable<any> {
    return this.httpRequestService.postReqest(
      WebApiUrl + 'Receipt' + WebApiSaffix + '/SaveReceiptSectionTransfer', receiptSectionTransfers);
  }
  
  public AdvanceReceivedDataSplit(source: AdvanceReceivedSplitSource): Observable<any> {
    return this.httpRequestService.postReqest(
      WebApiUrl + 'Receipt' + WebApiSaffix + '/AdvanceReceivedDataSplit', source);
  }

  ///// Other ///////////////////////////////////////////////////////////////////////
  public Delete(receiptId: number): Observable<any> {

    return this.httpRequestService.postReqest(
      WebApiUrl + 'Receipt' + WebApiSaffix + '/Delete',
      receiptId);
  }

  public CancelAdvanceReceived(updateItems: Array<AdvanceReceived>): Observable<any> {

    return this.httpRequestService.postReqest(
      WebApiUrl + 'Receipt' + WebApiSaffix + '/CancelAdvanceReceived',
      updateItems);
  }

  public SaveAdvanceReceived(updateItems: Array<AdvanceReceived>): Observable<any> {

    return this.httpRequestService.postReqest(
      WebApiUrl + 'Receipt' + WebApiSaffix + '/SaveAdvanceReceived',
      updateItems);
  }

  public CancelReceiptJournalizingAsync(journalizingOption: JournalizingOption): Observable<any> {

    return this.httpRequestService.postReqest(
      WebApiUrl + 'Receipt' + WebApiSaffix + '/CancelReceiptJournalizingAsync',
      journalizingOption);
  }

  public GetJournalizingReport(journalizingOption: JournalizingOption): Observable<any> {

    return this.httpRequestService.postReportReqest(
      WebApiUrl + 'Receipt' + WebApiSaffix + '/GetJournalizingReport',
      journalizingOption);
  }

  public Omit(omitSource: OmitSource): Observable<any> {
    return this.httpRequestService.postReqest(
      WebApiUrl + 'Receipt' + WebApiSaffix + '/Omit',
      omitSource);

  }

  public Read(transactionImportSource: TransactionImportSource): Observable<any> {
    return this.httpRequestService.postReqest(
      WebApiUrl + 'Receipt' + WebApiSaffix + '/Read',
      transactionImportSource);

  }

  public Import(transactionImportSource: TransactionImportSource): Observable<any> {
    return this.httpRequestService.postReqest(
      WebApiUrl + 'Receipt' + WebApiSaffix + '/Import',
      transactionImportSource);

  }

  public ExistOriginalReceipt(receiptId: number): Observable<any> {
    return this.httpRequestService.postReqest(
      WebApiUrl + 'Receipt' + WebApiSaffix + '/ExistOriginalReceipt',
      receiptId);
  }

  public CancelAdvanceReceivedDataSplit(source: AdvanceReceivedSplitSource): Observable<any> {
    return this.httpRequestService.postReqest(
      WebApiUrl + 'Receipt' + WebApiSaffix + '/CancelAdvanceReceivedDataSplit', source);
  }

  ///// PDF ////////////////////////////////////////////////////////////////////////
  public GetReport(receiptSearch: ReceiptSearch): Observable<any> {

    return this.httpRequestService.postReportReqest(
      WebApiUrl + 'Receipt' + WebApiSaffix + '/GetReport',
      receiptSearch);
  }

  public ExtractReceiptJournalizing(journalizingOption: JournalizingOption): Observable<any> {

    return this.httpRequestService.postReportReqest(
      WebApiUrl + 'Receipt' + WebApiSaffix + '/ExtractReceiptJournalizing',
      journalizingOption);
  }

  public GetImportValidationReport(transactionImportSource: TransactionImportSource): Observable<any> {

    return this.httpRequestService.postReportReqest(
      WebApiUrl + 'Receipt' + WebApiSaffix + '/GetImportValidationReport',
      transactionImportSource);
  }

  public UpdateOutputAtAsync(journalizingOption: JournalizingOption): Observable<any> {
    return this.httpRequestService.postReportReqest(
      WebApiUrl + 'Receipt' + WebApiSaffix + '/UpdateOutputAtAsync',
      journalizingOption);

  }


  ///// Exist ////////////////////////////////////////////////////////////////////////
  public ExistCustomer(customerId: number): Observable<any> {
    return this.httpRequestService.postReportReqest(
      WebApiUrl + 'Receipt' + WebApiSaffix + '/ExistCustomer', customerId);
  }

  public ExistReceiptCategory(categoryId: number) {
    return this.httpRequestService.postReqest(
      WebApiUrl + 'Receipt' + WebApiSaffix + '/ExistReceiptCategory', categoryId);
  }

  public ExistExcludeCategory(categoryId: number) {
    return this.httpRequestService.postReqest(
      WebApiUrl + 'Receipt' + WebApiSaffix + '/ExistExcludeCategory', categoryId);
  }

  public ExistSection(sectionId: number) {
    return this.httpRequestService.postReqest(
      WebApiUrl + 'Receipt' + WebApiSaffix + '/ExistSection', sectionId);
  }

}
