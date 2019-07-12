import { Injectable } from '@angular/core';
import { IgnoreKana } from 'src/app/model/ignore-kana.model';
import { IgnoreKanasResult } from 'src/app/model/ignore-kanas-result.model';
import { ProcessResult } from 'src/app/model/process-result.model';
import { HttpRequestService } from '../common/http-request.service';
import { UserInfoService } from '../common/user-info.service';
import { WebApiUrl, WebApiSaffix } from 'src/app/common/const/http.const';
import { MasterImportSource } from 'src/app/model/master-import-source.model';

@Injectable({
  providedIn: 'root'
})
export class IgnoreKanaMasterService {

  constructor(
    private httpRequestService: HttpRequestService,
    private userInfoService: UserInfoService
  ) { }

  public GetItems(kana: IgnoreKana) {
    kana.companyId = this.userInfoService.Company.id;

    return this.httpRequestService.postReqest(
      WebApiUrl + 'IgnoreKana' + WebApiSaffix + '/GetItems', kana);
  }

  public Save(kana: IgnoreKana) {
    kana.companyId = this.userInfoService.Company.id;
    kana.updateBy = this.userInfoService.LoginUser.id;

    return this.httpRequestService.postReqest(
      WebApiUrl + 'IgnoreKana' + WebApiSaffix + '/Save', kana);
  }

  public Delete(kana: IgnoreKana) {
    return this.httpRequestService.postReqest(
      WebApiUrl + 'IgnoreKana' + WebApiSaffix + '/Delete', kana);
  }

  public Import(source: MasterImportSource) {
    return this.httpRequestService.postReqest(
      WebApiUrl + 'IgnoreKana' + WebApiSaffix + '/Import', source);
  }


  ///// Exist //////////////////////////////////////////////////////////////////////
  public ExistCategory(categoryId: number) {
    return this.httpRequestService.postReqest(
      WebApiUrl + 'IgnoreKana' + WebApiSaffix + '/ExistCategory', categoryId);
  }


}
