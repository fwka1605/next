import { Injectable } from '@angular/core';
import { BankAccount } from 'src/app/model/bank-account.model';
import { BankAccountsResult } from 'src/app/model/bank-accounts-result.model';
import { ProcessResult } from 'src/app/model/process-result.model';
import { UserInfoService } from '../common/user-info.service';
import { HttpRequestService } from '../common/http-request.service';
import { Observable } from 'rxjs';
import { BankAccountSearch } from 'src/app/model/bank-account-search.model';
import { WebApiUrl, WebApiSaffix } from 'src/app/common/const/http.const';
import { MasterImportSource } from 'src/app/model/master-import-source.model';

@Injectable({
  providedIn: 'root'
})
export class BankAccountMasterService {

  constructor(
    private userInfoService: UserInfoService,
    private httpRequestService: HttpRequestService
  ) { }

  public GetByCode(bankCode: string): Observable<any> {

    let bankAccount = new BankAccount();

    bankAccount.companyId = this.userInfoService.Company.id;
    bankAccount.bankCode = bankCode;

    return this.httpRequestService.postReqest(WebApiUrl + 'BankAccount' + WebApiSaffix + '/GetByCode', bankAccount);
  }

  public GetItems(): Observable<any> {

    let bankAccountSearch = new BankAccountSearch();
    bankAccountSearch.companyId = this.userInfoService.Company.id;
    return this.httpRequestService.postReqest(WebApiUrl + 'BankAccount' + WebApiSaffix + '/GetItems', bankAccountSearch);
  }

  public Save(bankBranch: BankAccount) {
    bankBranch.companyId = this.userInfoService.Company.id;
    bankBranch.updateBy = this.userInfoService.LoginUser.id;

    return this.httpRequestService.postReqest(
      WebApiUrl + 'BankAccount' + WebApiSaffix + '/Save', bankBranch);
  }

  public Delete(account: BankAccount) {
    return this.httpRequestService.postReqest(
      WebApiUrl + 'BankAccount' + WebApiSaffix + '/Delete', account);
  }

  public Import(source: MasterImportSource) {
    return this.httpRequestService.postReqest(
      WebApiUrl + 'BankAccount' + WebApiSaffix + '/Import', source);
  }


  ///// Exist //////////////////////////////////////////////////////////////////////
  public ExistCategory(categoryId: number) {
    return this.httpRequestService.postReqest(
      WebApiUrl + 'BankAccount' + WebApiSaffix + '/ExistCategory', categoryId);
  }

  public ExistSection(sectionId: number) {
    return this.httpRequestService.postReqest(
      WebApiUrl + 'BankAccount' + WebApiSaffix + '/ExistSection', sectionId);
  }

}
