import { Injectable } from '@angular/core';
import { BankBranch } from 'src/app/model/bank-branch.model';
import { BankBranchResult } from 'src/app/model/bank-branch-result.model';
import { BankBranchsResult } from 'src/app/model/bank-branchs-result.model';
import { ProcessResult } from 'src/app/model/process-result.model';
import { Observable } from 'rxjs';
import { WebApiUrl, WebApiSaffix } from 'src/app/common/const/http.const';
import { HttpRequestService } from '../common/http-request.service';
import { UserInfoService } from '../common/user-info.service';
import { BankBranchSearch } from 'src/app/model/bank-branch-search.model';
import { MasterImportSource } from 'src/app/model/master-import-source.model';

@Injectable({
  providedIn: 'root'
})
export class BankBranchMasterService {

  constructor(
    private httpRequestService: HttpRequestService,
    private userInfoService: UserInfoService,
  ) { }

  public GetItems(bankBranchSearch: BankBranchSearch = new BankBranchSearch()): Observable<any> {
    bankBranchSearch.companyId = this.userInfoService.Company.id;
    return this.httpRequestService.postReqest(WebApiUrl + 'BankBranch' + WebApiSaffix + '/GetItems', bankBranchSearch);
  }

  public Save(bankBranch: BankBranch): Observable<any> {
    bankBranch.companyId = this.userInfoService.Company.id;
    bankBranch.updateBy = this.userInfoService.LoginUser.id;
    
    return this.httpRequestService.postReqest(
      WebApiUrl + 'BankBranch' + WebApiSaffix + '/Save', bankBranch);
  }

  public Delete(bankBranch: BankBranch): Observable<any> {
    return this.httpRequestService.postReqest(
      WebApiUrl + 'BankBranch' + WebApiSaffix + '/Delete', bankBranch);
  }

  public Import(source: MasterImportSource): Observable<any> {
    return this.httpRequestService.postReqest(
      WebApiUrl + 'BankBranch' + WebApiSaffix + '/Import', source);
  }

}