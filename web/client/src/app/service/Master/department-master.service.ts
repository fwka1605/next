import { Injectable } from '@angular/core';
import { Department } from 'src/app/model/department.model';
import { DepartmentResult } from 'src/app/model/department-result.model';
import { DepartmentsResult } from 'src/app/model/departments-result.model';
import { ProcessResult } from 'src/app/model/process-result.model';
import { DepartmentSearch } from 'src/app/model/department-search.model';
import { MasterSearchOption } from 'src/app/model/master-search-option';
import { UserInfoService } from '../common/user-info.service';
import { HttpRequestService } from '../common/http-request.service';
import { WebApiUrl, WebApiSaffix } from 'src/app/common/const/http.const';
import { Observable } from 'rxjs';
import { StringUtil } from 'src/app/common/util/string-util';
import { MasterImportData } from 'src/app/model/master-import-data.model';
import { MasterImportSource } from 'src/app/model/master-import-source.model';

@Injectable({
  providedIn: 'root'
})
export class DepartmentMasterService {

  constructor(
    private userInfoService: UserInfoService,
    private httpRequestService: HttpRequestService
  ) { }


  public GetItems(departmentCode: string = "", loginUserId:number=null): Observable<any> {
    let departmentSearch = new DepartmentSearch();
    departmentSearch.companyId = this.userInfoService.Company.id;

    if (!StringUtil.IsNullOrEmpty(departmentCode)) {
      departmentSearch.codes = new Array<string>();
      departmentSearch.codes.push(departmentCode);
    }
    if(loginUserId != undefined){
      departmentSearch.loginUserId = loginUserId;
    }
    return this.httpRequestService.postReqest(WebApiUrl + 'Department' + WebApiSaffix + '/GetItems', departmentSearch);

  }

  public GetItemsByDepartmentSearch(departmentSearch: DepartmentSearch = new DepartmentSearch()): Observable<any> {
    departmentSearch.companyId = this.userInfoService.Company.id;
    return this.httpRequestService.postReqest(
      WebApiUrl + 'Department' + WebApiSaffix + '/GetItems', departmentSearch);
  }  

  public GetReport(): Observable<any> {
    return this.httpRequestService.postReportReqest(WebApiUrl + 'Department' + WebApiSaffix + '/GetReport', this.userInfoService.Company.id);
  }

  public Save(departments: Array<Department>): Observable<any> {
    return this.httpRequestService.postReqest(
      WebApiUrl + 'Department' + WebApiSaffix + '/Save', departments);
  }

  public Delete(department: Department): Observable<any> {
    return this.httpRequestService.postReqest(
      WebApiUrl + 'Department' + WebApiSaffix + '/Delete', department);
  }

  public Import(masterImportSource: MasterImportSource): Observable<any> {
    return this.httpRequestService.postReqest(
      WebApiUrl + 'Department' + WebApiSaffix + '/Import', masterImportSource);
  }

  // /api/Department/GetItems


  // /api/Department/GetImportItemsSectionWithDepartment
  public GetImportItemsSectionWithDepartment(masterSearchOption: MasterSearchOption): Observable<any> {
    masterSearchOption.companyId = this.userInfoService.Company.id;
    masterSearchOption.codes = ["A001"];
    return this.httpRequestService.postReqest(
      WebApiUrl + 'Department' + WebApiSaffix + '/GetImportItemsSectionWithDepartment', masterSearchOption);
  }

}
