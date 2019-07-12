import { Injectable } from '@angular/core';
import { DestinationsResult } from 'src/app/model/destinations-result.model';
import { ProcessResult } from 'src/app/model/process-result.model';
import { Destination } from 'src/app/model/destination.model';
import { Observable } from 'rxjs';
import { StaffSearch } from 'src/app/model/staff-search.model';
import { UserInfoService } from '../common/user-info.service';
import { HttpRequestService } from '../common/http-request.service';
import { StringUtil } from 'src/app/common/util/string-util';
import { WebApiUrl, WebApiSaffix } from 'src/app/common/const/http.const';
import { DestinationSearch } from 'src/app/model/destination-search.model';

@Injectable({
  providedIn: 'root'
})
export class DestinationMasterService {

  private destinationsResult:DestinationsResult;
  constructor(
    private userInfoService:UserInfoService,
    private httpRequestService:HttpRequestService,
  ) { }

  public GetItems(destinationCode: string = ""): Observable<any> {

    let destinationSearch = new DestinationSearch();
    destinationSearch.companyId = this.userInfoService.Company.id;

    if (!StringUtil.IsNullOrEmpty(destinationCode)) {
      destinationSearch.codes = new Array<string>();
      destinationSearch.codes.push(destinationCode);
    }

    return this.httpRequestService.postReqest(WebApiUrl + 'Destination' + WebApiSaffix + '/GetItems', destinationSearch);

  }  

}
