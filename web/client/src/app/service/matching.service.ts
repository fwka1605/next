import { Injectable } from '@angular/core';
import {  MatchingJournalizingDetail } from '../model/matching-journalizing-detail.model';
import { MatchingJournalizingDetailsResult } from '../model/matching-journalizing-details-result.model';
import { ProcessResult } from '../model/process-result.model';
import { HttpRequestService } from './common/http-request.service';
import { UserInfoService } from './common/user-info.service';
import { Observable } from 'rxjs';
import { WebApiUrl, WebApiSaffix } from '../common/const/http.const';
import { JournalizingOption } from '../model/journalizing-option.model';
import { MatchingJournalizingReportSource } from '../model/matching-journalizing-report-source.model';
import { MatchingJournalizingCancellationReportSource } from '../model/matching-journalizing-cancellation-report-source.model';
import { WorkDepartmentTargetSource } from '../model/work-department-target-source.model';
import { WorkSectionTargetSource } from '../model/work-section-target-source.model';
import { CollationSearch } from '../model/collation-search.model';
import { Collation } from '../model/collation.model';
import { FormGroup } from '@angular/forms';
import { MatchingSequentialSource } from '../model/matching-sequential-source.model';
import { MatchingCancelSource } from '../model/matching-cancel-source.model';
import { CollationInfo } from '../model/collation/collation-info';
import { MatchingBillingSearch } from '../model/matching-billing-search.model';
import { MatchingReceiptSearch } from '../model/matching-receipt-search.model';
import { Billing } from '../model/billing.model';
import { MatchingSimulationSource } from '../model/matching-simulation-source.model';
import { MatchingSequentialReportSource } from 'src/app/model/matching-sequential-report-source.model';
import { MatchingIndividualReportSource } from '../model/matching-individual-report-source.model';
import { MatchingSource } from '../model/matching-source.model';

@Injectable({
  providedIn: 'root'
})
export class MatchingService {
  constructor(
    private httpRequestService: HttpRequestService,
    private userInfoService: UserInfoService,
  ) { }

  public collationInfo:CollationInfo;   // 照合・表示結果
  public Pe0101myFormGroup:FormGroup;   // 検索条件


  public GetMatchingJournalizingSummary(journalizingOption:JournalizingOption): Observable<any> {
    return  this.httpRequestService.postReqest(
              WebApiUrl + 'Matching' + WebApiSaffix + '/GetMatchingJournalizingSummary',
              journalizingOption);
  }  
  

  public ExtractMatchingJournalizing(journalizingOption:JournalizingOption): Observable<any> {
    return  this.httpRequestService.postReqest(
              WebApiUrl + 'Matching' + WebApiSaffix + '/ExtractMatchingJournalizing', 
              journalizingOption);
  }  

  
  public GetJournalizingReport(matchingJournalizingReportSource:MatchingJournalizingReportSource ): Observable<any> {
    return  this.httpRequestService.postReportReqest(
              WebApiUrl + 'Matching' + WebApiSaffix + '/GetJournalizingReport', 
              matchingJournalizingReportSource);
  }   

  public UpdateOutputAt(journalizingOption:JournalizingOption ): Observable<any> {
    return  this.httpRequestService.postReportReqest(
              WebApiUrl + 'Matching' + WebApiSaffix + '/UpdateOutputAt', 
              journalizingOption);
  }        

  public CancelMatchingJournalizing(journalizingOption:JournalizingOption ): Observable<any> {
    return  this.httpRequestService.postReqest(
              WebApiUrl + 'Matching' + WebApiSaffix + '/CancelMatchingJournalizing', 
              journalizingOption);
  }     

  public CancelMatchingJournalizingDetail(matchingJournalizingDetails:Array<MatchingJournalizingDetail> ): Observable<any> {
    return  this.httpRequestService.postReqest(
              WebApiUrl + 'Matching' + WebApiSaffix + '/CancelMatchingJournalizingDetail', 
              matchingJournalizingDetails);
  }     



  public GetMatchingJournalizingDetail(journalizingOption:JournalizingOption ): Observable<any> {
    return  this.httpRequestService.postReqest(
              WebApiUrl + 'Matching' + WebApiSaffix + '/GetMatchingJournalizingDetail', 
              journalizingOption);
  }     

  
  public GetJournalizingCancelReport(matchingJournalizingCancellationReportSource:MatchingJournalizingCancellationReportSource ): Observable<any> {
    return  this.httpRequestService.postReportReqest(
              WebApiUrl + 'Matching' + WebApiSaffix + '/GetJournalizingCancelReport', 
              matchingJournalizingCancellationReportSource);
  }   

  public SaveWorkDepartmentTarget(clientKey:string,departmentIds:number[] ): Observable<any> {
    let workDepartmentTargetSource = new WorkDepartmentTargetSource();
    workDepartmentTargetSource.clientKey = clientKey;
    workDepartmentTargetSource.departmentIds = departmentIds;
    workDepartmentTargetSource.companyId = this.userInfoService.Company.id;

    return  this.httpRequestService.postReqest(
              WebApiUrl + 'Matching' + WebApiSaffix + '/SaveWorkDepartmentTarget', 
              workDepartmentTargetSource);
  }     

  public SaveWorkSectionTarget(clientKey:string,sections:number[] ): Observable<any> {
    let workSectionTargetSource = new WorkSectionTargetSource();
    workSectionTargetSource.clientKey = clientKey;
    workSectionTargetSource.sectionIds = sections;
    workSectionTargetSource.companyId = this.userInfoService.Company.id;

    return  this.httpRequestService.postReqest(
              WebApiUrl + 'Matching' + WebApiSaffix + '/SaveWorkSectionTarget', 
              workSectionTargetSource);
  }     

  public Collate(collationSearch :CollationSearch  ): Observable<any> {
    return  this.httpRequestService.postReqest(
              WebApiUrl + 'Matching' + WebApiSaffix + '/Collate', 
              collationSearch);
  }     

  public SearchMatchedData(collationSearch :CollationSearch  ): Observable<any> {
    return  this.httpRequestService.postReqest(
              WebApiUrl + 'Matching' + WebApiSaffix + '/SearchMatchedData', 
              collationSearch);
  }    
  
  public SequentialMatching(matchingSequentialSource :MatchingSequentialSource  ): Observable<any> {
    return  this.httpRequestService.postReqest(
              WebApiUrl + 'Matching' + WebApiSaffix + '/SequentialMatching', 
              matchingSequentialSource);
  }    

  public CancelMatching(mtchingCancelSource :MatchingCancelSource  ): Observable<any> {
    return  this.httpRequestService.postReqest(
              WebApiUrl + 'Matching' + WebApiSaffix + '/CancelMatching', 
              mtchingCancelSource);
  }    
    
  public SearchBillingData(option:MatchingBillingSearch): Observable<any> {
    return  this.httpRequestService.postReqest(
              WebApiUrl + 'Matching' + WebApiSaffix + '/SearchBillingData', 
              option);
  }

  public SearchReceiptData(option:MatchingReceiptSearch): Observable<any> {
    return  this.httpRequestService.postReqest(
              WebApiUrl + 'Matching' + WebApiSaffix + '/SearchReceiptData', 
              option);
  }  

  public Simulate(billings:Billing[], targetAmount:number): Observable<any> {

    let matchingSimulationSource:MatchingSimulationSource = new MatchingSimulationSource();
    matchingSimulationSource.billings=billings;
    matchingSimulationSource.targetAmount=targetAmount;
    
    return  this.httpRequestService.postReqest(
              WebApiUrl + 'Matching' + WebApiSaffix + '/Simulate', 
              matchingSimulationSource);
  }

  public GetSequentialReport(matchingSequentialReportSource:MatchingSequentialReportSource ): Observable<any> {
    return  this.httpRequestService.postReportReqest(
              WebApiUrl + 'Matching' + WebApiSaffix + '/GetSequentialReport', 
              matchingSequentialReportSource);
  }

  public GetIndividualReport(matchingIndividualReportSource:MatchingIndividualReportSource ): Observable<any> {
    return  this.httpRequestService.postReportReqest(
              WebApiUrl + 'Matching' + WebApiSaffix + '/GetIndividualReport', 
              matchingIndividualReportSource);
  }

  public Solve(requestSource:MatchingSource): Observable<any> {

    return  this.httpRequestService.postReqest(
              WebApiUrl + 'Matching' + WebApiSaffix + '/Solve', 
              requestSource);
  }  
  
  public MatchingIndividually(requestSource:MatchingSource): Observable<any> {

    return  this.httpRequestService.postReqest(
              WebApiUrl + 'Matching' + WebApiSaffix + '/MatchingIndividually', 
              requestSource);
  }  
  
  public MFExtractMatchingJournalizing(journalizingOption:JournalizingOption): Observable<any> {
    return  this.httpRequestService.postReqest(
              WebApiUrl + 'Matching' + WebApiSaffix + '/MFExtractMatchingJournalizing', 
              journalizingOption);
  }
  
  public GetMatchedReceipt(journalizingOption:JournalizingOption): Observable<any> {
    return  this.httpRequestService.postReqest(
              WebApiUrl + 'Matching' + WebApiSaffix + '/GetMatchedReceipt',
              journalizingOption);
  }  
  
}
