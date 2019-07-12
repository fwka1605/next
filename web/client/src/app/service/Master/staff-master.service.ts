import { Injectable } from '@angular/core';
import { Staff } from 'src/app/model/staff.model';
import { StaffResult } from 'src/app/model/staff-result.model';
import { StaffsResult } from 'src/app/model/staffs-result.model';
import { HttpRequestService } from '../common/http-request.service';
import { WebApiUrl, WebApiSaffix } from 'src/app/common/const/http.const';
import { Observable } from 'rxjs';
import { ProcessResult } from 'src/app/model/process-result.model';
import { UserInfoService } from '../common/user-info.service';
import { StaffSearch } from 'src/app/model/staff-search.model';
import { MasterSearchOption } from 'src/app/model/master-search-option';
import { StringUtil } from 'src/app/common/util/string-util';
import { MasterImportSource } from 'src/app/model/master-import-source.model';

@Injectable({
  providedIn: 'root'
})
export class StaffMasterService {

  constructor(
    private httpRequestService: HttpRequestService,
    private userInfoService: UserInfoService,

  ) { }

  public GetItems(staffCode: string = ""): Observable<any> {

    let staffSearch = new StaffSearch();
    staffSearch.companyId = this.userInfoService.Company.id;

    if (!StringUtil.IsNullOrEmpty(staffCode)) {
      staffSearch.codes = new Array<string>();
      staffSearch.codes.push(staffCode);
    }

    return this.httpRequestService.postReqest(
      WebApiUrl + 'Staff' + WebApiSaffix + '/GetItems', staffSearch);
  }

  public GetItemsbyStaffSearch(staffSearch: StaffSearch): Observable<any> {
    staffSearch.companyId = this.userInfoService.Company.id;

    return this.httpRequestService.postReqest(
      WebApiUrl + 'Staff' + WebApiSaffix + '/GetItems', staffSearch);
  }  

  public Save(staff: Staff) {
    staff.companyId = this.userInfoService.Company.id;
    staff.updateBy = this.userInfoService.LoginUser.id;

    return this.httpRequestService.postReqest(
      WebApiUrl + 'Staff' + WebApiSaffix + '/Save', staff);
  }

  public Delete(staff: Staff) {
    return this.httpRequestService.postReqest(
      WebApiUrl + 'Staff' + WebApiSaffix + '/Delete', staff);
  }

  public Import(source: MasterImportSource) {
    return this.httpRequestService.postReqest(
      WebApiUrl + 'Staff' + WebApiSaffix + '/Import', source);
  }

  public GetReport() {
    return this.httpRequestService.postReportReqest(
      WebApiUrl + 'Staff' + WebApiSaffix + '/GetReport', this.userInfoService.Company.id);
  }


  ///// Exist //////////////////////////////////////////////////////////////////////
  public ExistDepartment(departmentId: number): Observable<any> {
    return this.httpRequestService.postReqest(
      WebApiUrl + 'Staff' + WebApiSaffix + '/ExistDepartment', departmentId);
  }

}
