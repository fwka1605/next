import { Injectable } from '@angular/core';
import { Observable, Subject } from 'rxjs';
import { WebApiUrl, WebApiSaffix } from 'src/app/common/const/http.const';
import { HttpRequestService } from '../common/http-request.service';
import { Company } from 'src/app/model/company.model';
import { CompanySearch } from 'src/app/model/company-search.model';
import { UserInfoService } from '../common/user-info.service';
import { CompanySource } from 'src/app/model/company-source.model';

@Injectable({
  providedIn: 'root'
})
export class CompanyMasterService {

  private company: Company;
  public get Company(): Company {
    return this.company;
  }
  public set Company(value: Company) {
    this.company = value;
  }


  constructor(
    private httpRequestService:HttpRequestService,
    private userInfoService: UserInfoService,
  ) { }

  public GetItems(companyCode: string): Observable<any> {
    let companySearch = new CompanySearch();
    companySearch.code = companyCode;
    return this.httpRequestService.postReqest(
      WebApiUrl + 'Company' + WebApiSaffix + '/GetItems', companySearch);
  }

  public Save(company : Company) {
    return this.httpRequestService.postReqest(
      WebApiUrl + 'Company' + WebApiSaffix + '/Save', company);
  }

  public Initialize(companySource : CompanySource): Observable<any> {
    return this.httpRequestService.postReqest(
      WebApiUrl + 'Company' + WebApiSaffix + '/Initialize', companySource);
  }

}
