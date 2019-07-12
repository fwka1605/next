import { Injectable } from '@angular/core';
import { UserInfoService } from '../common/user-info.service';
import { HttpRequestService } from '../common/http-request.service';
import { Observable } from 'rxjs';
import { WebApiUrl, WebApiSaffix } from 'src/app/common/const/http.const';
import { SectionWithDepartment } from 'src/app/model/section-with-department.model';
import { SectionWithDepartmentSearch } from 'src/app/model/section-with-department-search.model';
import { MasterImportData } from 'src/app/model/master-import-data.model';
import { MasterImportSource } from 'src/app/model/master-import-source.model';

@Injectable({
  providedIn: 'root'
})
export class SectionWithDepartmentMasterService {

  constructor(
    private userInfoService: UserInfoService,
    private httpRequestService: HttpRequestService
  ) { }

  public GetItems(sectionWithDepartmentSearch: SectionWithDepartmentSearch = new SectionWithDepartmentSearch()): Observable<any> {
    sectionWithDepartmentSearch.companyId = this.userInfoService.Company.id;
    return this.httpRequestService.postReqest(
      WebApiUrl + 'SectionWithDepartment' + WebApiSaffix + '/GetItems', sectionWithDepartmentSearch);
  }

  public Save(masterImportData: MasterImportData<SectionWithDepartment>): Observable<any> {
    return this.httpRequestService.postReqest(
      WebApiUrl + 'SectionWithDepartment' + WebApiSaffix + '/Save', masterImportData);
  }

  public Import(source: MasterImportSource) {
    return this.httpRequestService.postReqest(
      WebApiUrl + 'SectionWithDepartment' + WebApiSaffix + '/Import', source);
  }

  ///// Exist ////////////////////////////////////////////////////////////////////////
  public ExistDepartment(departmentId: number): Observable<any> {
    return this.httpRequestService.postReqest(
      WebApiUrl + 'SectionWithDepartment' + WebApiSaffix + '/ExistDepartment', departmentId);
  }

  public ExistSection(sectionId: number) {
    return this.httpRequestService.postReqest(
      WebApiUrl + 'SectionWithDepartment' + WebApiSaffix + '/ExistSection', sectionId);
  }
}
