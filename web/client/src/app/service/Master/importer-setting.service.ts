import { Injectable } from '@angular/core';
import { ImporterSettingDetail } from 'src/app/model/importer-setting-detail.model';
import { ImporterSettingDetailsResult } from 'src/app/model/importer-setting-details-result.model';
import { ProcessResult } from 'src/app/model/process-result.model';
import { UserInfoService } from '../common/user-info.service';
import { HttpRequestService } from '../common/http-request.service';
import { ImporterSetting } from 'src/app/model/importer-setting.model';
import { Observable } from 'rxjs';
import { WebApiUrl, WebApiSaffix } from 'src/app/common/const/http.const';
import { FreeImporterFormatType } from 'src/app/common/common-const';

@Injectable({
  providedIn: 'root'
})
export class ImporterSettingService {

  constructor(
    private userInfoService: UserInfoService,
    private httpRequestService: HttpRequestService
  ) { }

  

  public GetHeader(formatType:number,code:string=""): Observable<any> {

    let importerSetting = new ImporterSetting();
    importerSetting.companyId = this.userInfoService.Company.id;
    importerSetting.code=code;
    importerSetting.formatId = formatType;

    return this.httpRequestService.postReqest(WebApiUrl + 'ImporterSetting' + WebApiSaffix + '/GetHeader', importerSetting);
  }

  public GetDetail(formatType: number, patternNo: string): Observable<any> {

    let importerSetting = new ImporterSetting();
    importerSetting.companyId = this.userInfoService.Company.id;
    importerSetting.formatId = formatType;
    importerSetting.code = patternNo;

    return this.httpRequestService.postReqest(WebApiUrl + 'ImporterSetting' + WebApiSaffix + '/GetDetail', importerSetting);

  }


  public Save(importerSetting:ImporterSetting): Observable<any> {

    return this.httpRequestService.postReqest(WebApiUrl + 'ImporterSetting' + WebApiSaffix + '/Save', importerSetting);

  }  

  public Delete(importerSetting:ImporterSetting): Observable<any> {

    return this.httpRequestService.postReqest(WebApiUrl + 'ImporterSetting' + WebApiSaffix + '/Delete', importerSetting);

  }  

}
