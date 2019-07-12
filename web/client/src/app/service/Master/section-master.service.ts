import { SectionsResult} from '../../model/sections-result.model';
import { Injectable } from '@angular/core';
import { SectionResults } from 'src/app/model/section-results.model';
import { ProcessResult } from 'src/app/model/process-result.model';
import { Section } from 'src/app/model/section.model';
import { Observable } from 'rxjs';
import { MasterSearchOption } from 'src/app/model/master-search-option';
import { UserInfoService } from '../common/user-info.service';
import { HttpRequestService } from '../common/http-request.service';
import { WebApiUrl, WebApiSaffix } from 'src/app/common/const/http.const';
import { SectionSearch } from 'src/app/model/section-search.model';
import { StringUtil } from 'src/app/common/util/string-util';
import { MasterImportSource } from 'src/app/model/master-import-source.model';

@Injectable({
  providedIn: 'root'
})
export class SectionMasterService {

  constructor(
    private userInfoService: UserInfoService,
    private httpRequestService: HttpRequestService
  ) { }

  public GetItems(sectionCode: string="", loginUserId:number=null): Observable<any> {

    let sectionSearch = new SectionSearch();
    sectionSearch.companyId = this.userInfoService.Company.id;
    if(!StringUtil.IsNullOrEmpty(sectionCode)){
      sectionSearch.codes = new Array<string>();
      sectionSearch.codes.push(sectionCode);
    }
    if(loginUserId != undefined){
      sectionSearch.loginUserId = loginUserId;
    }

    return this.httpRequestService.postReqest(WebApiUrl + 'Section' + WebApiSaffix + '/GetItems', sectionSearch);
  }

  public GetItemsById(sectionId: number): Observable<any> {

    let sectionSearch = new SectionSearch();
    sectionSearch.companyId = this.userInfoService.Company.id;
    sectionSearch.ids = new Array<number>();
    sectionSearch.ids.push(sectionId);
    return this.httpRequestService.postReqest(WebApiUrl + 'Section' + WebApiSaffix + '/GetItems', sectionSearch);
  }

  public Save(section: Section) {
    section.companyId = this.userInfoService.Company.id;
    section.updateBy = this.userInfoService.LoginUser.id;

    return this.httpRequestService.postReqest(
      WebApiUrl + 'Section' + WebApiSaffix + '/Save', section);
  }  


  public Delete(section: Section) {
    return this.httpRequestService.postReqest(
      WebApiUrl + 'Section' + WebApiSaffix + '/Delete', section);
  }

  public Import(source: MasterImportSource) {
    return this.httpRequestService.postReqest(
      WebApiUrl + 'Section' + WebApiSaffix + '/Import', source);
  }


}
